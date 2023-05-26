using Gab.Core.Email.Commands;
using Gab.Core.ExtensionMethods;
using Gab.Core.Membership.Models;
using Gab.Core.Membership.Models.Identity;
using Gab.Core.Settings.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;

namespace Gab.Core.Membership.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthenticationResult>
    {
        private readonly UserManager<GabUser> _userManager;
        private readonly SignInManager<GabUser> _signInManager;
        private readonly IUserStore<GabUser> _userStore;
        private readonly IUserEmailStore<GabUser> _userEmailStore;
        private readonly ILogger<RegisterUserHandler> _logger;
        private readonly IMediator _mediator;
        private readonly GabSettings _gabSettings;

        public RegisterUserHandler(UserManager<GabUser> userManager,
                                IUserEmailStore<GabUser> userEmailStore,
                                IUserStore<GabUser> userStore,
                                ILogger<RegisterUserHandler> logger,
                                SignInManager<GabUser> signInManager,
                                IOptionsSnapshot<GabSettings> gabSettings,
                                IMediator mediator)
        {
            _userManager = userManager;
            _userEmailStore = userEmailStore;
            _userStore = userStore;
            _logger = logger;
            _signInManager = signInManager;
            _gabSettings = gabSettings?.Value;
            _mediator = mediator;
        }

        public async Task<GabAuthenticationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new GabUser { Id = GuidFactory.NewSequentialGuid, Email = request.Email, UserName = request.Username };
            var loginResult = new GabAuthenticationResult();
            await _userStore.SetUserNameAsync(newUser, request.Email, CancellationToken.None).ConfigureAwait(false);
            await _userEmailStore.SetEmailAsync(newUser, request.Email, CancellationToken.None).ConfigureAwait(false);
            var createResult = await _userManager.CreateAsync(newUser, request.Password).ConfigureAwait(false);
            loginResult.Succeeded = createResult.Succeeded;
            if (loginResult.Succeeded)
            {
                _logger.LogInformation($"{request.Username} created a new account.");

                var addToRoleResult = await _userManager.AddToRoleAsync(newUser, _gabSettings.NewUserStartingRole).ConfigureAwait(false);
                if (addToRoleResult.Succeeded == false)
                {
                    addToRoleResult.LogErrors();
                    loginResult.FailedReasons.AddRange(addToRoleResult.ToErrorsList());
                    loginResult.Succeeded = false;
                    return loginResult;
                }
                else
                {
                    var user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        var sendConfirmationEmailCommand = new SendConfirmationEmailCommand
                        {
                            ReturnUrl = request.ReturnUrl,
                            User = user
                        };

                        await _mediator.Send(sendConfirmationEmailCommand, cancellationToken).ConfigureAwait(false);

                        loginResult.SucceededMessage = "Please check your email and click the link to confirm your account";
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, request.RememberMe).ConfigureAwait(false);

                        loginResult.NavigateToUrl = request.ReturnUrl ?? "~/";
                    }
                }
            }
            else
            {
                createResult.LogErrors();
                loginResult.FailedReasons.AddRange(createResult.ToErrorsList());
            }

            return loginResult;
        }
    }
}