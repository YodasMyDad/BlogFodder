using Gab.Core.Email.Commands;
using Gab.Core.Email.Services;
using Gab.Core.ExtensionMethods;
using Gab.Core.Membership.Models;
using Gab.Core.Membership.Models.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using BlogFodder.Core.Membership.Commands;

namespace Gab.Core.Membership.Handlers
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, GabAuthenticationResult>
    {
        private readonly UserManager<GabUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public ForgotPasswordHandler(UserManager<GabUser> userManager, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }

        public async Task<GabAuthenticationResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = new GabAuthenticationResult();

            var user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
            if (user != null)
            {
                if (await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false) == false)
                {
                    result.Succeeded = false;
                    result.FailedReasons.Add("Please check your email to confirm your account");

                    // Resend confirmation email
                    await _mediator.Send(new SendConfirmationEmailCommand { ReturnUrl = "~/", User = user }, cancellationToken).ConfigureAwait(false);
                    return result;
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = _httpContextAccessor.ToAbsoluteUrl(Constants.Urls.Account.ResetPassword, new { code = code });

                var paragraphs = new List<string> { $"Please reset your password by <a class=\"underline\" href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>." };
                await _emailSender.SendEmailAsync(request.Email, "Reset Password", paragraphs).ConfigureAwait(false);
            }

            result.Succeeded = true;
            result.SucceededMessage = "An email has been sent to you to";

            return result;
        }
    }
}