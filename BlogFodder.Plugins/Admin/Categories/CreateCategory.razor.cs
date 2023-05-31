using BlogFodder.Core.Categories.Commands;
using BlogFodder.Core.Categories.Validation;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Providers;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace BlogFodder.Plugins.Admin.Categories;

public partial class CreateCategory : ComponentBase
{
    [Inject] public ExtensionManager ExtensionManager { get; set; } = default!;
    [Inject] public BlogFodderDbContext DbContext { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] public IMediator Mediator { get; set; } = default!;
    [Inject] public ProviderService ProviderService { get; set; } = default!;

    [Parameter] public Guid? Id { get; set; }
    
    private CreateUpdateCategoryCommand CategoryCommand { get; set; } = new();
    private MudForm Form { get; set; } = default!;
    private CreateUpdateCategoryCommandValidator CommandValidator { get; set; } = new();
    private string?[] Errors { get; set; } =  Array.Empty<string>();
    
    protected override void OnInitialized()
    {
        // See if this is an edit or not
        if (Id != null)
        {
            // Yes, should probably be in a service or Mediatr call
            var dbCategory = DbContext.Categories
                .Include(x => x.SocialImage)
                .FirstOrDefault(x => x.Id == Id.Value);
            
            if (dbCategory != null)
            {
                CategoryCommand.Category = dbCategory;
                CategoryCommand.IsUpdate = true;
            }
        }
    }
    
    /// <summary>
    /// Removes the selected social images
    /// </summary>
    private void RemoveSelectedSocialImage()
    {
        CategoryCommand.SocialImage = null;
        StateHasChanged();
    }
    
    /// <summary>
    /// Removes the saved social image
    /// </summary>
    private void RemoveSocialImage()
    {
        CategoryCommand.Category.SocialImageId = null;
        CategoryCommand.Category.SocialImage = null;
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
    /// Submits the form and saves the post
    /// </summary>
    private async Task SubmitForm()
    {
        await Form.Validate();
        if (Form.IsValid)
        {
            // Call mediatr and return and check for errors
            // Send the email
            var result = await Mediator.Send(CategoryCommand).ConfigureAwait(false);
            if (result.Success)
            {
                CategoryCommand.Category = result.Entity;
                CategoryCommand.SocialImage = null;

                var correctText = CategoryCommand.IsUpdate ? "Updated" : "Created";
                Snackbar.Add("Category " + correctText, Severity.Success);

                if (!CategoryCommand.IsUpdate)
                {
                    CategoryCommand.IsUpdate = true;
                }
            }
            else
            {
                Errors = result.Messages.ErrorMessagesToList().ToArray();
            }
        }
    }
}