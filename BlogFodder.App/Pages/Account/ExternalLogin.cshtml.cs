using BlogFodder.Core;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogFodder.App.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IMediator _mediator;

        public ExternalLoginModel(
            SignInManager<User> signInManager, IMediator mediator)
        {
            _signInManager = signInManager;
            _mediator = mediator;
        }

        public ExternalLoginCommand ExternalLoginCommand { get; set; } = new ExternalLoginCommand();

        public AuthenticationResult Result { get; set; } = new AuthenticationResult();

        [TempData] public string ErrorMessage { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            return RedirectToPage(Constants.Urls.Account.Login);
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page(Constants.Urls.Account.ExternalLogin, pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
        {
            ExternalLoginCommand.ReturnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage(Constants.Urls.Account.Login, new { ReturnUrl = returnUrl });
            }
            ExternalLoginCommand.ExternalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (ExternalLoginCommand.ExternalLoginInfo == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage(Constants.Urls.Account.Login, new { ReturnUrl = returnUrl });
            }

            Result = await _mediator.Send(ExternalLoginCommand);

            if (Result.Success)
            {
                return LocalRedirect(Result.NavigateToUrl!);
            }

            return Page();
        }
    }
}