using Gab.Core.Email.Commands;
using Gab.Core.Membership.Models;
using Gab.Core.Membership.Models.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using BlogFodder.Core.Membership.Commands;

namespace Gab.Core.Membership.Handlers
{
    /// <summary>
    /// Responsible for handling a user login
    /// </summary>
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, GabAuthenticationResult>
    {
        private readonly SignInManager<GabUser> _signInManager;
        private readonly UserManager<GabUser> _userManager;
        private readonly ILogger<LoginUserHandler> _logger;
        private readonly IMediator _mediator;

        public LoginUserHandler(SignInManager<GabUser> signInManager, UserManager<GabUser> userManager, ILogger<LoginUserHandler> logger, IMediator mediator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<GabAuthenticationResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var loginResult = new GabAuthenticationResult();
            await _signInManager.SignOutAsync().ConfigureAwait(false);
            var user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, false)
                    .ConfigureAwait(false);
                loginResult.Succeeded = signInResult.Succeeded;

                if (loginResult.Succeeded)
                {
                    var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user).ConfigureAwait(false);
                    loginResult.NavigateToUrl = request.ReturnUrl ?? "~/";
                }
                else
                {
                    if (signInResult.IsNotAllowed)
                    {
                        if (!await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false))
                        {
                            loginResult.FailedReasons.Add("Email isn't confirmed. Check your inbox for a confirmation email");

                            // Resend confirmation email
                            var sendConfirmationEmailCommand = new SendConfirmationEmailCommand
                            {
                                ReturnUrl = request.ReturnUrl,
                                User = user
                            };

                            await _mediator.Send(sendConfirmationEmailCommand, cancellationToken).ConfigureAwait(false);
                        }

                        //if (!await _userManager.IsPhoneNumberConfirmedAsync(user).ConfigureAwait(false))
                        //{
                        //      // Phone Number isn't confirmed.
                        //      // loginResult.FailedReason = "Phone Number isn't confirmed";
                        //}
                    }
                    else if (signInResult.IsLockedOut)
                    {
                        _logger.LogWarning($"User {request.Email} account is locked out.");
                        loginResult.FailedReasons.Add("Account is locked out.");
                    }
                    else if (signInResult.RequiresTwoFactor)
                    {
                        loginResult.NavigateToUrl = $"/account/loginwith2fa?returnUrl={request.ReturnUrl}&rememberMe={request.RememberMe}";
                    }
                    else
                    {
                        if (user != null)
                        {
                            loginResult.FailedReasons.Add("Password is incorrect");
                        }
                    }
                }
            }
            else
            {
                loginResult.FailedReasons.Add("You are do not have an account, please register");
            }

            return loginResult;
        }
    }
}