using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogFodder.App.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly IMediator _mediator;

        public ConfirmEmailModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public AuthenticationResult Result { get; set; } = new AuthenticationResult();

        public async Task<IActionResult> OnGetAsync(string userId, string code, bool change)
        {
            if (userId == null || code == null)
            {
                Result.AddMessage("Unable to check User Id or Confirmation Code", ResultMessageType.Error);
            }
            else
            {
                var confirmEmailCommand = new ConfirmEmailCommand
                {
                    Code = code,
                    UserId = userId,
                    IsEmailUpdate = change
                };

                Result = await _mediator.Send(confirmEmailCommand).ConfigureAwait(false);
            }

            return Page();
        }
    }
}