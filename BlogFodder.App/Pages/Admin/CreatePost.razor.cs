using BlogFodder.App.Pages.Admin.Dialogs;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Posts.Validation;
using BlogFodder.Core.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Utilities;
using Newtonsoft.Json;

namespace BlogFodder.App.Pages.Admin;

public partial class CreatePost : ComponentBase
{
    [Inject] public ExtensionManager ExtensionManager { get; set; } = default!;
    [Inject] public BlogFodderDbContext DbContext { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] public IDialogService Dialog { get; set; } = default!;
    [Inject] public ProviderService ProviderService { get; set; } = default!;

    [Parameter] public Guid? Id { get; set; }
    private Post Post { get; set; } = new();
    private DateTime? DateCreated { get; set; } = DateTime.Now;
    private DateTime? DateUpdated { get; set; } = DateTime.Now;
    private Dictionary<string, IEditorPlugin> AvailableEditorPlugins { get; set; } = new();
    private MudDropContainer<PostContentItem>? _dropContainer;
    private string? SelectedEditorAlias { get; set; }
    private SlugHelper SlugHelper { get; set; } = new();
    private bool IsUpdate { get; set; }
    public MudForm Form { get; set; } = default!;
    private const string DefaultDropZoneSelector = "plugins";
    private IBrowserFile? FeaturedImage { get; set; }
    private IBrowserFile? SocialImage { get; set; }
    public PostValidator PostValidator { get; set; } = new();
    private string[] _errors = Array.Empty<string>();
    protected override void OnInitialized()
    {
        // See if this is an edit or not
        if (Id != null)
        {
            // Yes, should probably be in a service or Mediatr call
            var dbPost = DbContext.Posts.Include(x => x.ContentItems).FirstOrDefault(x => x.Id == Id.Value);
            if (dbPost != null)
            {
                Post = dbPost;

                // This is shite, need to look at a better way
                foreach (var postContentItem in Post.ContentItems)
                {
                    postContentItem.Selector = DefaultDropZoneSelector;
                }

                IsUpdate = true;
            }
        }

        // Get the Aliases of all Editor plugins
        var editorPlugins = ExtensionManager.GetInstances<IEditorPlugin>(true).Where(x => x.Value.Editor != null);
        foreach (var plugin in editorPlugins)
        {
            AvailableEditorPlugins.Add(plugin.Value.Alias, plugin.Value);
        }
    }

    /// <summary>
    /// Refreshes the dop list to show new data
    /// </summary>
    private void RefreshDopList()
    {
        //update the binding to the container
        StateHasChanged();

        //the container refreshes the internal state
        _dropContainer?.Refresh();
    }

    /// <summary>
    /// When the editor dropdown changes this stores the currently selected editor
    /// </summary>
    /// <param name="alias"></param>
    private void SelectedEditorChanged(string alias)
    {
        // Add in 
        SelectedEditorAlias = alias;
    }

    /// <summary>
    /// Fires when the order is updated on the drop item list
    /// </summary>
    /// <param name="dropItem"></param>
    private void DropItemUpdated(MudItemDropInfo<PostContentItem> dropItem)
    {
        if (dropItem.Item != null) dropItem.Item.Selector = dropItem.DropzoneIdentifier;
        // TODO - This doesn't seem to set the correct order!
        Post.ContentItems.UpdateOrder(dropItem, item => item.SortOrder, Post.ContentItems.Count);
    }

    /// <summary>
    /// Adds a new content item based on an editor plugin alias
    /// </summary>
    private void AddNewContentItem()
    {
        if (SelectedEditorAlias != null && AvailableEditorPlugins.TryGetValue(SelectedEditorAlias, out var plugin))
        {
            var postContentItem = new PostContentItem
            {
                PluginAlias = plugin.Alias,
                Selector = DefaultDropZoneSelector,
                SortOrder = Post.ContentItems.Count + 1,
                PostId = Post.Id,
                IsNew = true
            };

            if (plugin.Settings != null)
            {
                postContentItem.GlobalSettings = JsonConvert.SerializeObject(plugin.Settings.Model);
            }

            Post.ContentItems.Add(postContentItem);
            RefreshDopList();
        }
    }

    /// <summary>
    /// Shows the popup content editor and renders the Plugin Editor
    /// </summary>
    /// <param name="contentItem"></param>
    private async Task ShowEditor(PostContentItem contentItem)
    {
        var parameters = new DialogParameters
        {
            {"ContentItem", contentItem}
        };

        var dialog = await Dialog.ShowAsync<ContentItemEditor>("", parameters,
            new DialogOptions {MaxWidth = MaxWidth.Medium, FullWidth = true});
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            // Save the data to this contentItem, or do we have to loop
            // to find the one with the Id? And set the data that way?
            contentItem = result.Data as PostContentItem ?? contentItem;
            RefreshDopList();
        }
    }

    /// <summary>
    /// Submits the form and saves the post
    /// </summary>
    private async Task SubmitForm()
    {
        await Form.Validate();
        if (Form.IsValid)
        {
            // TODO - Look to do this with the fluentvalidator
            if (!Post.ContentItems.Any())
            {
                _errors = new[] {"You need to add some content to the post"};
                return;
            }
            
            // Set any empty values like the Url
            if (Post.Url.IsNullOrWhiteSpace())
            {
                Post.Url = SlugHelper.GenerateSlug(Post.Name);
            }

            if (Post.MetaDescription.IsNullOrWhiteSpace())
            {
                Post.MetaDescription = Post.Excerpt;
            }

            if (Post.PageTitle.IsNullOrWhiteSpace())
            {
                Post.PageTitle = Post.Name;
            }

            // Have to set these as the pickers won't allowed a non null Date!?
            Post.DateCreated = DateCreated!.Value;
            Post.DateUpdated = DateUpdated!.Value;

            // Profile Image - Need to save image and then create a gabfile
            if (FeaturedImage != null)
            {
                // Save the file, create a gab file and attach it to the user
                if (ProviderService.StorageProvider != null)
                {
                    var fileSaveResult = await ProviderService.StorageProvider
                        .SaveFile(FeaturedImage, Post.Id.ToString()).ConfigureAwait(false);
                    if (!fileSaveResult.Success)
                    {
                        foreach (var errorMessage in fileSaveResult.ErrorMessages)
                        {
                            Snackbar.Add(errorMessage, Severity.Error);
                        }
                    }

                    // Create the gabfile
                    var file = await ProviderService.StorageProvider.ToBlogFodderFile(fileSaveResult)
                        .ConfigureAwait(false);

                    // Save the file first
                    DbContext.Files.Add(file);
                    //await DbContext.SaveChangesAsync().ConfigureAwait(false);

                    // Set the file to the user
                    Post.FeaturedImage = file;
                }
            }

            if (SocialImage != null)
            {
                // Save the file, create a gab file and attach it to the user
                if (ProviderService.StorageProvider != null)
                {
                    var fileSaveResult = await ProviderService.StorageProvider.SaveFile(SocialImage, Post.Id.ToString())
                        .ConfigureAwait(false);
                    if (!fileSaveResult.Success)
                    {
                        foreach (var errorMessage in fileSaveResult.ErrorMessages)
                        {
                            Snackbar.Add(errorMessage, Severity.Error);
                        }
                    }

                    // Create the gabfile
                    var file = await ProviderService.StorageProvider.ToBlogFodderFile(fileSaveResult)
                        .ConfigureAwait(false);

                    // Save the file first
                    DbContext.Files.Add(file);
                    //await DbContext.SaveChangesAsync().ConfigureAwait(false);

                    // Set the file to the user
                    Post.SocialImage = file;
                }
            }


            // TODO - This needs to be moved to a service or better just use Mediatr
            if (IsUpdate)
            {
                // Add the new content items first
                foreach (var postContentItem in Post.ContentItems.Where(x => x.IsNew))
                {
                    postContentItem.IsNew = false;
                    DbContext.PostContentItems.Add(postContentItem);
                }

                DbContext.Posts.Update(Post);
            }
            else
            {
                DbContext.Posts.Add(Post);
            }

            await DbContext.SaveChangesAsync();
            var correctText = IsUpdate ? "Updated" : "Created";
            Snackbar.Add("Post " + correctText, Severity.Success);

            if (!IsUpdate)
            {
                IsUpdate = true;
            }
        }
    }
}