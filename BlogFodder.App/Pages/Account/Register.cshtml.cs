using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogFodder.App.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly IMediator _mediator;

        public RegisterModel(IMediator mediator, ILogger<RegisterModel> logger,
                                SignInManager<User> signInManager)
        {
            _mediator = mediator;
            _logger = logger;
            _signInManager = signInManager;
        }

        [BindProperty]
        public RegisterUserCommand RegisterUserCommand { get; set; } = new();

        public string ValidationSummaryStyles { get; set; } = "font-medium text-red-400 text-sm text-center";

        public async Task<IActionResult> OnGetAsync(string returnUrl)
        {
            RegisterUserCommand.ReturnUrl = returnUrl ?? Url.Content("~/");

            if (User.Identity.IsAuthenticated)
            {
                return Redirect("~/");
            }

            await SetExternalLogins().ConfigureAwait(false);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await SetExternalLogins().ConfigureAwait(false);

            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(RegisterUserCommand).ConfigureAwait(false);

                if (result.Success)
                {
                    if (!result.NavigateToUrl.IsNullOrWhiteSpace())
                    {
                        return LocalRedirect(result.NavigateToUrl);
                    }

                    // Show success message. Would be nice to change the styles
                    ValidationSummaryStyles = "font-medium text-green-400 text-base text-center";
                    ModelState.AddModelError("SuccessMessage", result.Messages.SuccessMessages().FirstOrDefault()?.Message!);
                }
                else
                {
                    foreach (var identityError in result.Messages.ErrorMessages())
                    {
                        if (identityError.Message != null)
                        {
                            ModelState.AddModelError("IdentityResultError", identityError.Message);
                        }
                    }
                }
            }

            return Page();
        }

        private async Task SetExternalLogins()
        {
            RegisterUserCommand.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)).ToList();
        }
    }
}