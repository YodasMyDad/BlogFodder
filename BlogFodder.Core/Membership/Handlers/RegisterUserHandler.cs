﻿using BlogFodder.Core.Email.Commands;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Settings;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlogFodder.Core.Membership.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthenticationResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IOptions<BlogFodderSettings> _settings;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _userEmailStore;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<RegisterUserHandler> _logger;
        private readonly IMediator _mediator;
        private readonly BlogFodderSettings _blogFodderSettings;

        public RegisterUserHandler(UserManager<User> userManager,
                                IUserEmailStore<User> userEmailStore,
                                IUserStore<User> userStore,
                                ILogger<RegisterUserHandler> logger,
                                SignInManager<User> signInManager,
                                IOptions<BlogFodderSettings> gabSettings,
                                IMediator mediator, RoleManager<Role> roleManager, IOptions<BlogFodderSettings> settings)
        {
            _userManager = userManager;
            _userEmailStore = userEmailStore;
            _userStore = userStore;
            _logger = logger;
            _signInManager = signInManager;
            _blogFodderSettings = gabSettings.Value;
            _mediator = mediator;
            _roleManager = roleManager;
            _settings = settings;
        }

        public async Task<AuthenticationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new User { Id = Guid.NewGuid().NewSequentialGuid(), Email = request.Email, UserName = request.Username };
            var loginResult = new AuthenticationResult();
            var createResult = await _userManager.CreateAsync(newUser, request.Password).ConfigureAwait(false);
            loginResult.Success = createResult.Succeeded;
            if (loginResult.Success)
            {
                _logger.LogInformation("{RequestUsername} created a new account", request.Username);

                var startingRoleName = _blogFodderSettings.NewUserStartingRole ?? Constants.Roles.StandardRoleName;
                if (_settings.Value.AdminEmailAddresses.Any() && _settings.Value.AdminEmailAddresses.Contains(newUser.Email!))
                {
                    startingRoleName = Constants.Roles.AdminRoleName;
                }
                
                // Check the starting role exists
                var roleExist = await _roleManager.RoleExistsAsync(startingRoleName);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new Role {Name = startingRoleName});
                }
                
                var addToRoleResult = await _userManager.AddToRoleAsync(newUser, startingRoleName).ConfigureAwait(false);
                if (addToRoleResult.Succeeded == false)
                {
                    addToRoleResult.LogErrors();
                    loginResult.AddMessage(addToRoleResult.ToErrorsList(), ResultMessageType.Error);
                    loginResult.Success = false;
                    return loginResult;
                }

                var user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    var sendConfirmationEmailCommand = new SendEmailConfirmationCommand
                    {
                        ReturnUrl = request.ReturnUrl,
                        User = user
                    };

                    await _mediator.Send(sendConfirmationEmailCommand, cancellationToken).ConfigureAwait(false);

                    loginResult.AddMessage("Please check your email and click the link to confirm your account", ResultMessageType.Success);
                }
                else
                {
                    await _signInManager.SignInAsync(user, request.RememberMe).ConfigureAwait(false);

                    loginResult.NavigateToUrl = request.ReturnUrl ?? "~/";
                }
            }
            else
            {
                createResult.LogErrors();
                loginResult.AddMessage(createResult.ToErrorsList(), ResultMessageType.Error);
            }

            return loginResult;
        }
    }
}