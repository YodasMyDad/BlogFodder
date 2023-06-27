using BlogFodder.Core.Email.Commands;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Settings;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlogFodder.Core.Membership.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthenticationResult>
    {
        private readonly IOptions<BlogFodderSettings> _settings;
        private readonly ILogger<RegisterUserHandler> _logger;
        private readonly BlogFodderSettings _blogFodderSettings;
        private readonly IServiceProvider _serviceProvider;

        public RegisterUserHandler(
            ILogger<RegisterUserHandler> logger,
            IOptions<BlogFodderSettings> gabSettings,
            IOptions<BlogFodderSettings> settings, 
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _blogFodderSettings = gabSettings.Value;
            _settings = settings;
            _serviceProvider = serviceProvider;
        }

        public async Task<AuthenticationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        
            
            var newUser = new User { Id = Guid.NewGuid().NewSequentialGuid(), Email = request.Email, UserName = request.Username };
            var loginResult = new AuthenticationResult();
            var createResult = await userManager.CreateAsync(newUser, request.Password);
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
                var roleExist = await roleManager.RoleExistsAsync(startingRoleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new Role {Name = startingRoleName});
                }
                
                var addToRoleResult = await userManager.AddToRoleAsync(newUser, startingRoleName);
                if (addToRoleResult.Succeeded == false)
                {
                    addToRoleResult.LogErrors();
                    loginResult.AddMessage(addToRoleResult.ToErrorsList(), ResultMessageType.Error);
                    loginResult.Success = false;
                    return loginResult;
                }

                var user = await userManager.FindByEmailAsync(request.Email);

                if (userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    var sendConfirmationEmailCommand = new SendEmailConfirmationCommand
                    {
                        ReturnUrl = request.ReturnUrl,
                        User = user
                    };

                    await mediatr.Send(sendConfirmationEmailCommand, cancellationToken);

                    loginResult.AddMessage("Please check your email and click the link to confirm your account", ResultMessageType.Success);
                }
                else
                {
                    await signInManager.SignInAsync(user, request.RememberMe);

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