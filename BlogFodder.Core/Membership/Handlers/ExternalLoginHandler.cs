﻿using System.Security.Claims;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlogFodder.Core.Membership.Handlers
{
    public class ExternalLoginHandler : IRequestHandler<ExternalLoginCommand, AuthenticationResult>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExternalLoginHandler> _logger;

        public ExternalLoginHandler(ILogger<ExternalLoginHandler> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task<AuthenticationResult> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<User>>();
            var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<User>>();
            var emailStore = scope.ServiceProvider.GetRequiredService<IUserEmailStore<User>>();

            var authenticationResult = new AuthenticationResult();

            // Sign in the user with this external login provider if the user already has a login.
            var externalLoginResult = await signInManager.ExternalLoginSignInAsync(request.ExternalLoginInfo.LoginProvider, request.ExternalLoginInfo.ProviderKey, isPersistent: true, bypassTwoFactor: true).ConfigureAwait(false);
            if (externalLoginResult.Succeeded)
            {
                authenticationResult.Success = true;
                authenticationResult.NavigateToUrl = request.ReturnUrl;
                return authenticationResult;
            }
            if (externalLoginResult.IsLockedOut)
            {
                authenticationResult.Success = false;
                authenticationResult.AddMessage("Unable to login, your account is locked out", ResultMessageType.Error);
                return authenticationResult;
            }

            // If the user does not have an account, so we need to create one, however
            // the external provider must pass an email address or they are not allowed to register
            if (!request.ExternalLoginInfo.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                authenticationResult.Success = false;
                authenticationResult.AddMessage("Unable to login using this provider, it did not provide a valid email address", ResultMessageType.Error);
                return authenticationResult;
            }

            // Get the email address out ready
            var emailAddress = request.ExternalLoginInfo.Principal.GetUserEmail();
            var userName = request.ExternalLoginInfo.Principal.GetUserName();
            if (userName.IsNullOrWhiteSpace())
            {
                userName = "user";
            }

            // Now we create a new user
            var user = new User{ Id = Guid.NewGuid().NewSequentialGuid()};

            // Set a flag so we know this user has logged in with an external account
            user.ExtendedData.Add(Constants.ExtendedDataKeys.IsExternalLogin, true);

            await userStore.SetUserNameAsync(user, emailAddress, cancellationToken).ConfigureAwait(false);
            await emailStore.SetEmailAsync(user, emailAddress, cancellationToken).ConfigureAwait(false);

            var result = await userManager.CreateAsync(user).ConfigureAwait(false);
            if (result.Succeeded)
            {
                authenticationResult.Success = true;
                result = await userManager.AddLoginAsync(user, request.ExternalLoginInfo).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    authenticationResult.Success = true;
                    var userId = await userManager.GetUserIdAsync(user).ConfigureAwait(false);
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);

                    // Because the email is coming from an external provider, we are going to assume they have
                    // confirmed the email and done all that stuff. So we are just going to confirm it.
                    var confirmResult = await userManager.ConfirmEmailAsync(user, code).ConfigureAwait(false);

                    if (confirmResult.Succeeded == false)
                    {
                        authenticationResult.Success = false;
                        foreach (var error in confirmResult.Errors)
                        {
                            _logger.LogError("Failure to confirm email address - {Error}", error.Description);
                            authenticationResult.AddMessage(error.Description, ResultMessageType.Error);
                        }
                    }
                    else
                    {
                        await signInManager.SignInAsync(user, isPersistent: false, request.ExternalLoginInfo.LoginProvider).ConfigureAwait(false);
                        authenticationResult.NavigateToUrl = request.ReturnUrl;
                    }
                }
                else
                {
                    authenticationResult.Success = false;
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError("Failure to login using external provider - {Error}", error.Description);
                        authenticationResult.AddMessage(error.Description, ResultMessageType.Error);
                    }
                }
            }
            else
            {
                authenticationResult.Success = false;
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Failure to login using external provider - {Error}", error.Description);
                    authenticationResult.AddMessage(error.Description, ResultMessageType.Error);
                }
            }
            return authenticationResult;
        }
    }
}