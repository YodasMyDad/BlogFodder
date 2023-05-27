using BlogFodder.Core.Email.Commands;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BlogFodder.Core.Membership.Handlers
{
    /// <summary>
    /// Responsible for handling a user login
    /// </summary>
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthenticationResult>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginUserHandler> _logger;
        private readonly IMediator _mediator;

        public LoginUserHandler(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<LoginUserHandler> logger, IMediator mediator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<AuthenticationResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var loginResult = new AuthenticationResult();
            await _signInManager.SignOutAsync().ConfigureAwait(false);
            var user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, false)
                    .ConfigureAwait(false);
                loginResult.Success = signInResult.Succeeded;

                if (loginResult.Success)
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
                            loginResult.AddMessage("Email isn't confirmed. Check your inbox for a confirmation email", ResultMessageType.Warning);

                            // Resend confirmation email
                            var sendConfirmationEmailCommand = new SendEmailConfirmationCommand
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
                        _logger.LogWarning("User {RequestEmail} account is locked out", request.Email);
                        loginResult.AddMessage("Account is locked out.", ResultMessageType.Error);
                    }
                    else if (signInResult.RequiresTwoFactor)
                    {
                        loginResult.NavigateToUrl = $"/account/loginwith2fa?returnUrl={request.ReturnUrl}&rememberMe={request.RememberMe}";
                    }
                    else
                    {
                        loginResult.AddMessage("Password is incorrect", ResultMessageType.Error);
                    }
                }
            }
            else
            {
                loginResult.AddMessage("You are do not have an account, please register", ResultMessageType.Error);
            }

            return loginResult;
        }
    }
}