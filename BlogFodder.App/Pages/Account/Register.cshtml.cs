using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Settings.Commands;
using BlogFodder.Core.Settings.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogFodder.App.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IMediator _mediator;

        public RegisterModel(IMediator mediator, SignInManager<User> signInManager)
        {
            _mediator = mediator;
            _signInManager = signInManager;
        }

        [BindProperty]
        public RegisterUserCommand RegisterUserCommand { get; set; } = new();

        public string ValidationSummaryStyles { get; set; } = "text-danger fw-bold small";
        public SiteSettings? Settings { get; set; }
        public async Task<IActionResult> OnGetAsync(string? returnUrl)
        {
            RegisterUserCommand.ReturnUrl = returnUrl ?? Url.Content("~/");

            if (User.Identity?.IsAuthenticated == true)
            {
                return Redirect("~/");
            }

            Settings = await _mediator.Send(new GetSiteSettingsCommand());
            
            await SetExternalLogins();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Reset anything after post
            await SetExternalLogins();
            Settings = await _mediator.Send(new GetSiteSettingsCommand());
            
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(RegisterUserCommand);

                if (result.Success)
                {
                    if (!result.NavigateToUrl.IsNullOrWhiteSpace())
                    {
                        return LocalRedirect(result.NavigateToUrl);
                    }

                    // Show success message. Would be nice to change the styles
                    ValidationSummaryStyles = "text-success fw-bold small";
                    ModelState.AddModelError("SuccessMessage", result.Messages.SuccessMessages().FirstOrDefault()?.Message!);
                }
                else
                {
                    foreach (var identityError in result.Messages.ErrorMessages())
                    {
                        if (identityError.Message != null)
                        {
                            ModelState.AddModelError("", identityError.Message);
                        }
                    }
                }
            }

            return Page();
        }

        private async Task SetExternalLogins()
        {
            RegisterUserCommand.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
    }
}