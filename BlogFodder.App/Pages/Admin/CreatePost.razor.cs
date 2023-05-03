using BlogFodder.App.Pages.Admin.Dialogs;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Posts.Commands;
using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Posts.Validation;
using MediatR;
using Microsoft.AspNetCore.Components;
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
    [Inject] public IMediator Mediator { get; set; } = default!;
    
    [Parameter] public Guid? Id { get; set; }
    
    private CreateUpdatePostCommand PostCommand { get; set; } = new();
    private Dictionary<string, IEditorPlugin> AvailableEditorPlugins { get; set; } = new();
    private MudDropContainer<PostContentItem>? DropContainer { get; set; }
    private string? SelectedEditorAlias { get; set; }
    private MudForm Form { get; set; } = default!;
    private CreateUpdatePostCommandValidator CommandValidator { get; set; } = new();
    private string?[] Errors { get; set; } =  Array.Empty<string>();
    private const string DefaultDropZoneSelector = "plugins";
    protected override void OnInitialized()
    {

        // See if this is an edit or not
        if (Id != null)
        {
            // Yes, should probably be in a service or Mediatr call
            var dbPost = DbContext.Posts
                .Include(x => x.ContentItems)
                .FirstOrDefault(x => x.Id == Id.Value);
            
            if (dbPost != null)
            {
                PostCommand.Post = dbPost;

                // This is shite, need to look at a better way
                foreach (var postContentItem in PostCommand.Post.ContentItems)
                {
                    postContentItem.Selector = DefaultDropZoneSelector;
                }

                PostCommand.IsUpdate = true;
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
        DropContainer?.Refresh();
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
        PostCommand.Post.ContentItems.UpdateOrder(dropItem, item => item.SortOrder, PostCommand.Post.ContentItems.Count);
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
                SortOrder = PostCommand.Post.ContentItems.Count + 1,
                PostId = PostCommand.Post.Id,
                IsNew = true
            };

            if (plugin.Settings != null)
            {
                postContentItem.GlobalSettings = JsonConvert.SerializeObject(plugin.Settings.Model);
            }

            PostCommand.Post.ContentItems.Add(postContentItem);
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
            // Call mediatr and return and check for errors
            // Send the email
            var result = await Mediator.Send(PostCommand).ConfigureAwait(false);
            if (result.Success)
            {
                PostCommand.Post = result.Entity;
                
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