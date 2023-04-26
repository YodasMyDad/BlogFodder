using BlogFodder.App.Pages.Admin.Dialogs;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Posts.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Utilities;
using Newtonsoft.Json;
using NuGet.Packaging;

namespace BlogFodder.App.Pages.Admin;

public partial class CreatePost : ComponentBase
{
    [Inject] public ExtensionManager ExtensionManager { get; set; } = default!;
    [Inject] public BlogFodderDbContext DbContext { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] public IDialogService Dialog { get; set; } = default!;

    [Parameter] public Guid? Id { get; set; }
    
    private Post Post { get; set; } = new();
    private DateTime? DateCreated { get; set; } = DateTime.Now;
    private DateTime? DateUpdated { get; set; } = DateTime.Now;
    private Dictionary<string, IEditorPlugin> AvailableEditorPlugins { get; set; } = new();
    private MudDropContainer<PostContentItem>? _dropContainer;
    private string? SelectedEditorAlias { get; set; }
    private SlugHelper SlugHelper { get; set; } = new();
    private bool IsUpdate { get; set; }

    private const string DefaultDropZoneSelector = "plugins";

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
                SortOrder = Post.ContentItems.Count+1,
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

        var dialog = await Dialog.ShowAsync<ContentItemEditor>("", parameters, new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            // Save the data to this contentItem, or do we have to loop
            // to find the one with the Id? And set the data that way?
            contentItem = result.Data as PostContentItem ?? contentItem;
            RefreshDopList();
        }
    }

    public IBrowserFile? FeaturedImage { get; set; }
    private void SetFeaturedImage(IBrowserFile file)
    {
        FeaturedImage = file;
        //TODO upload the files to the server
    }
    
    public IBrowserFile? SocialImage { get; set; }
    private void SetSocialImage(IBrowserFile file)
    {
        SocialImage = file;
        //TODO upload the files to the server
    }

    /// <summary>
    /// Creates the Url for the post
    /// </summary>
    /// <param name="debouncedText"></param>
    private void HandleUrlSlug(string debouncedText)
    {
        // Only creates the slug for a new post
        // They can manually change it themselves for an edit
        if (!IsUpdate)
        {
            Post.Url = SlugHelper.GenerateSlug(debouncedText);   
        }
    }
    
    /// <summary>
    /// Submits the form and saves the post
    /// </summary>
    /// <param name="formContext"></param>
    private async Task SubmitForm(EditContext formContext)
    {
        // TODO - Need to look at validation!
        /*var formIsValid = formContext.Validate();
        if (formIsValid == false)
            return;*/
        // Have to set these as the pickers won't allowed a non null Date!?
        Post.DateCreated = DateCreated!.Value;
        Post.DateUpdated = DateUpdated!.Value;
        
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
    }
}