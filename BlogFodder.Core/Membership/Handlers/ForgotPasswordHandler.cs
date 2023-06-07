﻿using System.Text;
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
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Membership.Handlers
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, AuthenticationResult>
    {
        private readonly ProviderService _providerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;
        
        public ForgotPasswordHandler(IHttpContextAccessor httpContextAccessor, ProviderService providerService, IServiceProvider serviceProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _providerService = providerService;
            _serviceProvider = serviceProvider;
        }

        public async Task<AuthenticationResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = new AuthenticationResult();

            using var scope = _serviceProvider.CreateScope();
            var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var user = await userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
            if (user != null)
            {
                if (await userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false) == false)
                {
                    result.Success = false;
                    result.AddMessage("Please check your email to confirm your account", ResultMessageType.Success);

                    // Resend confirmation email
                    await mediatr.Send(new SendEmailConfirmationCommand { ReturnUrl = "~/", User = user }, cancellationToken).ConfigureAwait(false);
                    return result;
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = _httpContextAccessor.ToAbsoluteUrl(Constants.Urls.Account.ResetPassword, new { code = code, email = request.Email });

                var paragraphs = new List<string> { $"Please reset your password by <a class=\"underline\" href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>." };
                await _providerService.EmailProvider!.SendEmailWithTemplateAsync(request.Email, "Reset Password", paragraphs).ConfigureAwait(false);
            }

            result.Success = true;
            result.AddMessage("An email has been sent to you to", ResultMessageType.Success);

            return result;
        }
    }
}