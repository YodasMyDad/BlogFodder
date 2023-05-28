using BlogFodder.Core.Membership.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogFodder.App.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;
        private readonly SignInManager<User> _signInManager;

        public LogoutModel(ILogger<LogoutModel> logger, SignInManager<User> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _signInManager.SignOutAsync().ConfigureAwait(false);

            return Redirect("~/");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync().ConfigureAwait(false);

            //if (returnUrl != null)
            //{
            //    return LocalRedirect(returnUrl);
            //}
            //else
            //{
            //    // This needs to be a redirect so that the browser performs a new
            //    // request and the identity for the user gets updated.
            //    return RedirectToPage();
            //}

            return Redirect("~/");
        }
    }
}