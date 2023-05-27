using System.Text;
using System.Text.Encodings.Web;
using BlogFodder.Core.Email.Commands;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace BlogFodder.Core.Membership.Handlers
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, AuthenticationResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly ProviderService _providerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public ForgotPasswordHandler(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IMediator mediator, ProviderService providerService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
            _providerService = providerService;
        }

        public async Task<AuthenticationResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = new AuthenticationResult();

            var user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
            if (user != null)
            {
                if (await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false) == false)
                {
                    result.Success = false;
                    result.AddMessage("Please check your email to confirm your account", ResultMessageType.Error);

                    // Resend confirmation email
                    await _mediator.Send(new SendEmailConfirmationCommand { ReturnUrl = "~/", User = user }, cancellationToken).ConfigureAwait(false);
                    return result;
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = _httpContextAccessor.ToAbsoluteUrl(Constants.Urls.Account.ResetPassword, new { code = code });

                var paragraphs = new List<string> { $"Please reset your password by <a class=\"underline\" href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>." };
                await _providerService.EmailProvider!.SendEmailWithTemplateAsync(request.Email, "Reset Password", paragraphs).ConfigureAwait(false);
            }

            result.Success = true;
            result.AddMessage("An email has been sent to you to", ResultMessageType.Success);

            return result;
        }
    }
}