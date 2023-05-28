using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogFodder.App.Pages.Account.Manage
{
    public class ShowRecoveryCodesModel : PageModel
    {
        [TempData] public string[] RecoveryCodes { get; set; } = Array.Empty<string>();

        [TempData]
        public string? StatusMessage { get; set; }

        public IActionResult OnGet()
        {
            if (RecoveryCodes.Length == 0)
            {
                return RedirectToPage("./TwoFactorAuthentication");
            }

            return Page();
        }
    }
}