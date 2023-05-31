using System.Text;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Settings.Commands;
using BlogFodder.Core.Settings.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace BlogFodder.App.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IMediator _mediator;

        public ResetPasswordModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public ResetPasswordCommand ResetPasswordCommand { get; set; } = new();

        public AuthenticationResult Result { get; set; } = new();
        
        public SiteSettings? Settings { get; set; }

        public async Task<IActionResult> OnGetAsync(string? code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            Settings = await _mediator.Send(new GetSiteSettingsCommand()).ConfigureAwait(false);
            ResetPasswordCommand.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Settings = await _mediator.Send(new GetSiteSettingsCommand()).ConfigureAwait(false);
            if (ModelState.IsValid)
            {
                Result = await _mediator.Send(ResetPasswordCommand).ConfigureAwait(false);

                if (Result.Success == false)
                {
                    foreach (var identityError in Result.Messages.ErrorMessages())
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
    }
}