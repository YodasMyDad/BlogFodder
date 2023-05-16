using System.Text.Json;
using BlogFodder.App.Pages.Admin.Posts.Dialogs;
using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Posts.Commands;
using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Posts.Validation;
using BlogFodder.Core.Providers;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Utilities;
using PostPluginEditor = BlogFodder.App.Pages.Admin.Posts.Dialogs.PostPluginEditor;

namespace BlogFodder.App.Pages.Admin.Posts;

public partial class CreatePost : ComponentBase
{
    [Inject] public ExtensionManager ExtensionManager { get; set; } = default!;
    [Inject] public BlogFodderDbContext DbContext { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] public IDialogService Dialog { get; set; } = default!;
    [Inject] public IMediator Mediator { get; set; } = default!;
    [Inject] public ProviderService ProviderService { get; set; } = default!;

    [Parameter] public Guid? Id { get; set; }

    private CreateUpdatePostCommand PostCommand { get; set; } = new();
    private Dictionary<string, IEditorPlugin> AvailableEditorPlugins { get; set; } = new();
    private Dictionary<string, IPlugin> AvailablePlugins { get; set; } = new();
    private Dictionary<string, Plugin> PluginData { get; set; } = new();
    private MudDropContainer<PostContentItem>? DropContainer { get; set; }
    private MudForm Form { get; set; } = default!;
    private CreateUpdatePostCommandValidator CommandValidator { get; set; } = new();
    private List<Category> Categories { get; set; } = new();
    private Category? SelectedCategory { get; set; }
    private IEnumerable<Category> SelectedCategories { get; set; } = new HashSet<Category>();
    private string?[] Errors { get; set; } = Array.Empty<string>();
    private const string DefaultDropZoneSelector = "plugins";
    private readonly string _customCardStyle = $"cursor: pointer; border: 1px {Colors.BlueGrey.Lighten4} solid;";
    private DialogOptions _defaultDialogOptions = new() {MaxWidth = MaxWidth.Large, FullWidth = true};

    protected override async Task OnInitializedAsync()
    {
        Categories = await DbContext.Categories.ToListAsync();

        // See if this is an edit or not
        if (Id != null)
        {
            // Yes, should probably be in a service or Mediatr call
            var dbPost = DbContext.Posts
                .Include(x => x.ContentItems)
                .Include(x => x.FeaturedImage)
                .Include(x => x.SocialImage)
                .Include(x => x.Categories)
                .FirstOrDefault(x => x.Id == Id.Value);

            if (dbPost != null)
            {
                PostCommand.Post = dbPost;

                // This is shite, need to look at a better way
                foreach (var postContentItem in PostCommand.Post.ContentItems)
                {
                    postContentItem.Selector = DefaultDropZoneSelector;
                }

                SelectedCategories = dbPost.Categories;

                PostCommand.IsUpdate = true;
            }
        }

        // Get the Aliases of all Editor plugins
        var editorPlugins = ExtensionManager.GetInstances<IEditorPlugin>(true).Where(x => x.Value.Editor != null);
        foreach (var plugin in editorPlugins)
        {
            AvailableEditorPlugins.Add(plugin.Value.Alias, plugin.Value);
        }
        
        // Get all the available plugins that have a editor
        var plugins = ExtensionManager.GetInstances<IPlugin>(true).Where(x => x.Value.Editor != null);
        foreach (var plugin in plugins)
        {
            AvailablePlugins.Add(plugin.Value.Alias, plugin.Value);
        }
        
        // Finally get the db plugin data for this post
        var pluginData = DbContext.Plugins.Where(x => x.PostId == PostCommand.Post.Id)
            .ToDictionary(x => x.PluginAlias ?? "misc", x => x);
        if (pluginData.Any())
        {
            PluginData = pluginData;
        }
    }

    private readonly Func<Category, string> _categoryToName = p => p.Name ?? "Missing Category Name";

    /// <summary>
    /// Refreshes the dop list to show new data
    /// </summary>
    private void RefreshDopList()
    {
        //update the binding to the container
        StateHasChanged();

        //the container refreshes the internal state
        DropContainer?.Refresh();
    }

    /// <summary>
    /// Removes the selected social images
    /// </summary>
    private void RemoveSelectedSocialImage()
    {
        PostCommand.SocialImage = null;
        StateHasChanged();
    }

    /// <summary>
    /// Removes the saved social image
    /// </summary>
    private void RemoveSocialImage()
    {
        PostCommand.Post.SocialImageId = null;
        PostCommand.Post.SocialImage = null;
        StateHasChanged();
    }

    /// <summary>
    /// Removes the featured image
    /// </summary>
    private void RemoveSelectedFeaturedImage()
    {
        PostCommand.FeaturedImage = null;
        StateHasChanged();
    }

    /// <summary>
    /// Removes the featured image
    /// </summary>
    private void RemoveFeaturedImage()
    {
        PostCommand.Post.FeaturedImageId = null;
        PostCommand.Post.FeaturedImage = null;
        StateHasChanged();
    }

    /// <summary>
    /// Checks the image size and notifies the user if the image selected is too large
    /// </summary>
    /// <param name="args"></param>
    private void CheckImageSize(InputFileChangeEventArgs args)
    {
        var result = ProviderService.StorageProvider?.CanUseFile(args.File).Result;
        if (result is {Success: false})
        {
            foreach (var errorMessage in result.ErrorMessages)
            {
                Snackbar.Add(errorMessage, Severity.Error);
            }
        }
    }

    /// <summary>
    /// Fires when the order is updated on the drop item list
    /// </summary>
    /// <param name="dropItem"></param>
    private void DropItemUpdated(MudItemDropInfo<PostContentItem> dropItem)
    {
        if (dropItem.Item != null) dropItem.Item.Selector = dropItem.DropzoneIdentifier;
        // TODO - This doesn't seem to set the correct order!
        PostCommand.Post.ContentItems.UpdateOrder(dropItem, item => item.SortOrder,
            PostCommand.Post.ContentItems.Count);
    }

    /// <summary>
    /// Removes a post content item from the post
    /// </summary>
    /// <param name="postContentItem"></param>
    private void RemoveContentItem(PostContentItem postContentItem)
    {
        PostCommand.Post.ContentItems.Remove(postContentItem);
        RefreshDopList();
    }

    /// <summary>
    /// Shows the dialog for the post plugins
    /// </summary>
    /// <param name="iplugin"></param>
    /// <param name="plugin"></param>
    private async Task ShowPostPlugin(IPlugin iplugin, Plugin? plugin)
    {
        var parameters = new DialogParameters
        {
            {"PostPlugin", plugin},
            {"Plugin", iplugin}
        };
        
        var dialog = await Dialog.ShowAsync<PostPluginEditor>(iplugin.Name, parameters, _defaultDialogOptions);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            // TODO - Need to save this data here as it won't be saved with the post
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

        var dialog = await Dialog.ShowAsync<ContentItemEditor>(AvailableEditorPlugins[contentItem.PluginAlias!].Name, parameters, _defaultDialogOptions);

        /*var dialog = await Dialog.ShowEx<ContentItemEditor>("", parameters, Options);*/
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            // Save the data to this contentItem, or do we have to loop
            // to find the one with the Id? And set the data that way?
            contentItem = result.Data as PostContentItem ?? contentItem;
            RefreshDopList();
        }
    }

    private async Task ShowContentEditors()
    {
        var dialog = await Dialog.ShowAsync<EditorPluginSelection>("Select Content Editor", new DialogParameters(), _defaultDialogOptions);
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            // Save the data to this contentItem, or do we have to loop
            // to find the one with the Id? And set the data that way?
            var plugin = result.Data as IEditorPlugin;
            if (plugin != null)
            {
                var postContentItem = new PostContentItem
                {
                    PluginAlias = plugin.Alias,
                    Selector = DefaultDropZoneSelector,
                    SortOrder = PostCommand.Post.ContentItems.Count + 1,
                    PostId = PostCommand.Post.Id,
                    IsNew = true
                };

                if (plugin.Settings != null)
                {
                    var globalSettingsCommand = new GetPluginSettingsCommand
                    {
                        Alias = plugin.Alias
                    };
                    var globalSettings = await Mediator.Send(globalSettingsCommand).ConfigureAwait(false);
                    postContentItem.GlobalSettings = globalSettings != null
                        ? JsonSerializer.Serialize(globalSettings.Data,
                            new JsonSerializerOptions {WriteIndented = false})
                        : JsonSerializer.Serialize(plugin.Settings.Model,
                            new JsonSerializerOptions {WriteIndented = false});
                }

                PostCommand.Post.ContentItems.Add(postContentItem);
                RefreshDopList();
            }
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
            // Couple of manual checks
            if (PostCommand.FeaturedImage == null && PostCommand.Post.FeaturedImage == null)
            {
                Errors = new[] {"You need to add a featured image"};
                return;
            }

            if (!PostCommand.Post.ContentItems.Any())
            {
                Errors = new[] {"You need to add some content"};
                return;
            }

            PostCommand.Post.Categories = SelectedCategories.ToList();

            // Call mediatr and return and check for errors
            // Send the email
            var result = await Mediator.Send(PostCommand).ConfigureAwait(false);
            if (result.Success)
            {
                PostCommand.Post = result.Entity;
                PostCommand.SocialImage = null;
                PostCommand.FeaturedImage = null;

                var correctText = PostCommand.IsUpdate ? "Updated" : "Created";
                Snackbar.Add("Post " + correctText, Severity.Success);

                if (!PostCommand.IsUpdate)
                {
                    PostCommand.IsUpdate = true;
                }
            }
            else
            {
                Errors = result.Messages.ErrorMessagesToList().ToArray();
            }
        }
    }
}