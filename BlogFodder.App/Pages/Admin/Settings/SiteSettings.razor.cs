using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Settings.Commands;
using BlogFodder.Core.Settings.Validation;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace BlogFodder.App.Pages.Admin.Settings;

public partial class SiteSettings : ComponentBase
{
        [Inject]
    public BlogFodderDbContext DbContext { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    public IMediator Mediator { get; set; } = default!;

    [Inject] public ProviderService ProviderService { get; set; } = default!;

    
    private MudForm Form { get; set; } = default!;
    private bool IsUpdate { get; set; }
    private string?[] Errors { get; set; } = Array.Empty<string>();
    private CreateUpdateSiteSettingsCommand SiteSettingsCommand { get; set; } = new();
    
    private CreateUpdateSiteSettingsCommandValidator CommandValidator { get; set; } = new();
    
    protected override async Task OnInitializedAsync()
    {
        var settings = await DbContext.SiteSettings.FirstOrDefaultAsync();
        if (settings != null)
        {
            SiteSettingsCommand.IsUpdate = true;
            SiteSettingsCommand.SiteSettings = settings;
        }
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
    /// Clears the saved the logo
    /// </summary>
    private void RemoveLogo()
    {
        SiteSettingsCommand.SiteSettings.LogoId = null;
        SiteSettingsCommand.SiteSettings.Logo = null;
        StateHasChanged();
    }
    
    /// <summary>
    /// Removes the selected logo
    /// </summary>
    private void RemoveSelectedLogo()
    {
        SiteSettingsCommand.Logo = null;
        StateHasChanged();
    }
    
    private async Task SubmitForm()
    {
        await Form.Validate();
        if (Form.IsValid)
        {
            var result = await Mediator.Send(SiteSettingsCommand);
            if (result.Success)
            {
                var correctText = IsUpdate ? "Updated" : "Created";
                Snackbar.Add("Site Settings " + correctText, Severity.Success);

                if (!IsUpdate)
                {
                    IsUpdate = true;
                }
            }
            else
            {
                Errors = result.Messages.ErrorMessagesToList().ToArray();
            }
        }
    }
}