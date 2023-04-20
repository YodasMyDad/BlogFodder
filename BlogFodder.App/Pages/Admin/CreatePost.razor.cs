using BlogFodder.App.Pages.Admin.Dialogs;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Posts.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MudBlazor.Utilities;

namespace BlogFodder.App.Pages.Admin;

public partial class CreatePost : ComponentBase
{
    public Post Post { get; set; } = new();
    public DateTime? DateCreated { get; set; } = DateTime.Now;
    public DateTime? DateUpdated { get; set; } = DateTime.Now;
    public Dictionary<string, IPlugin> AvailableEditorPlugins { get; set; } = new();
    
    private MudDropContainer<PostContentItem> _container;
    
    [Inject] public ExtensionManager ExtensionManager { get; set; }
    
    /*private EditContext editContext;*/
    public string? AliasFromDialog { get; set; }
    public string? SelectedEditorAlias { get; set; }
    
    private void RefreshContainer()
    {
        //update the binding to the container
        StateHasChanged();

        //the container refreshes the internal state
        _container.Refresh();
    }

    
    private void OnContentItemChanged(string alias)
    {
        // Add in 
        SelectedEditorAlias = alias;
    }
    
    private void ItemUpdated(MudItemDropInfo<PostContentItem> dropItem)
    {
        if (dropItem.Item != null) dropItem.Item.Selector = dropItem.DropzoneIdentifier;
        Post.ContentItems.UpdateOrder(dropItem, item => item.SortOrder, Post.ContentItems.Count);
    }

    
    private void AddNewContentItem()
    {
        if (SelectedEditorAlias != null && AvailableEditorPlugins.TryGetValue(SelectedEditorAlias, out var plugin))
        {
            Post.ContentItems.Add(new PostContentItem
            {
                PluginAlias = plugin.Alias,
                Selector = "plugins"
            });
            RefreshContainer();
        }
    }

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
            contentItem.PluginData = result.Data.ToString();
            StateHasChanged();
        }
    }
    
    protected override void OnInitialized()
    {
        // Get the Aliases of all Editor plugins
        var allPlugins = ExtensionManager.GetInstances<IPlugin>(true).Where(x => x?.Editor != null);
        foreach (var plugin in allPlugins)
        {
            if (plugin != null)
            {
                AvailableEditorPlugins.Add(plugin.Alias, plugin);
            }
        }        
        //editContext = new EditContext(Post);
    }

    public IBrowserFile FeaturedImage { get; set; }
    private void SetFeaturedImage(IBrowserFile file)
    {
        FeaturedImage = file;
        //TODO upload the files to the server
    }
    
    public IBrowserFile SocialImage { get; set; }
    private void SetSocialImage(IBrowserFile file)
    {
        SocialImage = file;
        //TODO upload the files to the server
    }

    private void HandleIntervalElapsed(string debouncedText)
    {
        // at this stage, interval has elapsed
        Post.Url = debouncedText;
    }
    
    private async Task SubmitForm()
    {
        /*CreateUpdateGabCategoryCommand.SectionId = GetGabCategoriesCommand.SectionId!.Value;
        var result = await Mediator.Send(CreateUpdateGabCategoryCommand);
        if (result.Success)
        {
            await GetCategories();    
        }
        else
        {
            foreach (var m in result.Messages.Where(x => x.MessageType == NotificationLevel.Error))
            {
                NotificationService.ShowError(m.Message ?? string.Empty);
            }
        }
        
        //await Overlay.CloseAsync(OverlayResult.Ok(GetGabCategoriesCommand))*/
    }
}