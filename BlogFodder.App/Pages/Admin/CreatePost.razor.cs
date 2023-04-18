using BlogFodder.App.Pages.Admin.Dialogs;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Posts.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MudBlazor.Extensions;
using MudBlazor.Extensions.Options;

namespace BlogFodder.App.Pages.Admin;

public partial class CreatePost : ComponentBase
{
    public Post Post { get; set; } = new();
    public DateTime? DateCreated { get; set; } = DateTime.Now;
    public DateTime? DateUpdated { get; set; } = DateTime.Now;
    public List<string> AvailableEditorPlugins { get; set; } = new();
    
    [Inject] public ExtensionManager ExtensionManager { get; set; }
    
    /*private EditContext editContext;*/
    public string? AliasFromDialog { get; set; }
    public string SelectedEditorAlias { get; set; }
    
    private void OnContentItemChanged(string alias)
    {
        // Add in 
        SelectedEditorAlias = alias;
    }

    private void AddNewContentItem()
    {
        
    }
    
    protected override void OnInitialized()
    {
        // Get the Aliases of all Editor plugins
        var allPlugins = ExtensionManager.GetInstances<IPlugin>(true).Where(x => x?.Editor != null);
        foreach (var plugin in allPlugins)
        {
            if (plugin != null)
            {
                AvailableEditorPlugins.Add(plugin.Alias);
            }
        }        
        //editContext = new EditContext(Post);
    }

    DialogOptions maxWidth = new() { MaxWidth = MaxWidth.Medium, FullWidth = true };
    private async Task OpenDialog()
    {
        var parameters = new DialogParameters
        {
            {"ContentText", "Are you sure you want to remove thisguy@emailz.com from this account?"}
        };

        var dialog = await Dialog.ShowAsync<BlogContentItemsList>("Select Blog Content Item", parameters, maxWidth);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            AliasFromDialog = result.Data.ToString();
            /*//In a real world scenario we would reload the data from the source here since we "removed" it in the dialog already.
            Guid.TryParse(result.Data.ToString(), out Guid deletedServer);
            Servers.RemoveAll(item => item.Id == deletedServer);*/
        }

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