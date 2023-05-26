using Gab.Core.ExtensionMethods;
using Gab.Core.Membership.Models;
using Gab.Core.Membership.Models.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using BlogFodder.Core.Membership.Commands;

namespace Gab.Core.Membership.Handlers
{
    public class ExternalLoginHandler : IRequestHandler<ExternalLoginCommand, GabAuthenticationResult>
    {
        private readonly SignInManager<GabUser> _signInManager;
        private readonly UserManager<GabUser> _userManager;
        private readonly IUserStore<GabUser> _userStore;
        private readonly IUserEmailStore<GabUser> _emailStore;
        private readonly ILogger<ExternalLoginHandler> _logger;

        public ExternalLoginHandler(SignInManager<GabUser> signInManager, IUserStore<GabUser> userStore, IUserEmailStore<GabUser> emailStore, UserManager<GabUser> userManager, ILogger<ExternalLoginHandler> logger)
        {
            _signInManager = signInManager;
            _userStore = userStore;
            _emailStore = emailStore;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<GabAuthenticationResult> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
        {
            var authenticationResult = new GabAuthenticationResult();

            // Sign in the user with this external login provider if the user already has a login.
            var externalLoginResult = await _signInManager.ExternalLoginSignInAsync(request.ExternalLoginInfo.LoginProvider, request.ExternalLoginInfo.ProviderKey, isPersistent: true, bypassTwoFactor: true).ConfigureAwait(false);
            if (externalLoginResult.Succeeded)
            {
                authenticationResult.Succeeded = true;
                authenticationResult.NavigateToUrl = request.ReturnUrl;
                return authenticationResult;
            }
            if (externalLoginResult.IsLockedOut)
            {
                authenticationResult.Succeeded = false;
                authenticationResult.FailedReasons.Add("Unable to login, your account is locked out");
                return authenticationResult;
            }

            // If the user does not have an account, so we need to create one, however
            // the external provider must pass an email address or they are not allowed to register
            if (!request.ExternalLoginInfo.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                authenticationResult.Succeeded = false;
                authenticationResult.FailedReasons.Add("Unable to login using this provider, it did not provide a valid email address");
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
            var user = new GabUser(GuidFactory.NewSequentialGuid);

            // Set a flag so we know this user has logged in with an external account
            user.ExtendedData.Add(Constants.ExtendedDataKeys.IsExternalLogin, true);

            await _userStore.SetUserNameAsync(user, emailAddress, cancellationToken).ConfigureAwait(false);
            await _emailStore.SetEmailAsync(user, emailAddress, cancellationToken).ConfigureAwait(false);

            var result = await _userManager.CreateAsync(user).ConfigureAwait(false);
            if (result.Succeeded)
            {
                authenticationResult.Succeeded = true;
                result = await _userManager.AddLoginAsync(user, request.ExternalLoginInfo).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    authenticationResult.Succeeded = true;
                    var userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);

                    // Because the email is coming from an external provider, we are going to assume they have
                    // confirmed the email and done all that stuff. So we are just going to confirm it.
                    var confirmResult = await _userManager.ConfirmEmailAsync(user, code).ConfigureAwait(false);

                    if (confirmResult.Succeeded == false)
                    {
                        authenticationResult.Succeeded = false;
                        foreach (var error in confirmResult.Errors)
                        {
                            _logger.LogError($"Failure to confirm email address - {error}");
                            authenticationResult.FailedReasons.Add(error.Description);
                        }
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false, request.ExternalLoginInfo.LoginProvider).ConfigureAwait(false);
                        authenticationResult.NavigateToUrl = request.ReturnUrl;
                    }
                }
                else
                {
                    authenticationResult.Succeeded = false;
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"Failure to login using external provider - {error}");
                        authenticationResult.FailedReasons.Add(error.Description);
                    }
                }
            }
            else
            {
                authenticationResult.Succeeded = false;
                foreach (var error in result.Errors)
                {
                    _logger.LogError($"Failure to login using external provider - {error}");
                    authenticationResult.FailedReasons.Add(error.Description);
                }
            }
            return authenticationResult;
        }
    }
}