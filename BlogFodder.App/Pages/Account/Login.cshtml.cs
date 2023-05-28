using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogFodder.App.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IMediator _mediator;

        public LoginModel(IMediator mediator, SignInManager<User> signInManager)
        {
            _mediator = mediator;
            _signInManager = signInManager;
        }

        [BindProperty]
        public LoginUserCommand LoginUserCommand { get; set; } = new();

        [TempData] public string ErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(string returnUrl)
        {
            LoginUserCommand.ReturnUrl = returnUrl ?? "/";

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
                var result = await _mediator.Send(LoginUserCommand).ConfigureAwait(false);

                if (result.Success || !result.Messages.ErrorMessages().Any())
                {
                    return LocalRedirect(result.NavigateToUrl);
                }
                foreach(var error in result.Messages.ErrorMessages())
                {
                    ModelState.AddModelError(string.Empty, error.Message);
                }
            }

            return Page();
        }

        private async Task SetExternalLogins()
        {
            LoginUserCommand.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)).ToList();
        }
    }
}