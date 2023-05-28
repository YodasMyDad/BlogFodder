using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogFodder.App.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly IMediator _mediator;

        public ForgotPasswordModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty] 
        public ForgotPasswordCommand ForgotPasswordCommand { get; set; } = new();

        public AuthenticationResult Result { get; set; } = new();

        public string ValidationSummaryStyles { get; set; } = "font-medium text-red-400 text-sm text-center";

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Result = await _mediator.Send(ForgotPasswordCommand).ConfigureAwait(false);

                if (Result.Success == false)
                {
                    foreach (var identityError in Result.Messages.ErrorMessages())
                    {
                        if (identityError.Message != null)
                        {
                            ModelState.AddModelError(string.Empty, identityError.Message);
                        }
                    }
                }
            }

            return Page();
        }
    }
}