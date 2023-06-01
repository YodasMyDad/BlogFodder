using System.Security.Claims;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlogFodder.Core.Membership;

public class BlogFodderSignInManager : SignInManager<User>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IOptions<BlogFodderSettings> _settings;
    private readonly ILogger<BlogFodderSignInManager> _logger;

    public BlogFodderSignInManager(
        UserManager<User> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<User> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<BlogFodderSignInManager> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<User> confirmation, IOptions<BlogFodderSettings> options, RoleManager<Role> roleManager)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
        _userManager = userManager;
        _logger = logger;
        _settings = options;
        _roleManager = roleManager;
    }


    public override async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey,
        bool isPersistent, bool bypassTwoFactor)
    {
        var signInResult =
            await base.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);

        var user = await _userManager.FindByLoginAsync(loginProvider, providerKey);

        // If the user doesn't exist, create them.
        if (user == null)
        {
            var info = await GetExternalLoginInfoAsync();
            if (info == null)
            {
                _logger.LogError("Error loading external login information");
                return SignInResult.Failed;
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var username = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email; // Use email as username if name is not available.

            user = new User
            {
                UserName = username,
                Email = email
            };

            var createUserResult = await _userManager.CreateAsync(user);

            if (!createUserResult.Succeeded)
            {
                // Handle failure to create user here.
                _logger.LogError("Unable to create user for external login {LoginProvider}", loginProvider);
                return SignInResult.Failed;
            }

            var addLoginResult =
                await _userManager.AddLoginAsync(user, new UserLoginInfo(loginProvider, providerKey, loginProvider));

            if (!addLoginResult.Succeeded)
            {
                // Handle failure to associate login with user here.
                _logger.LogError("Unable to add external login for user {UserUserName}", user.UserName);
                return SignInResult.Failed;
            }

            signInResult =
                await base.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
            
            // If the user successfully signed in, add them to a role.
            if (signInResult.Succeeded)
            {
                // Firstly, check to see if this email address is meant to be a default admin
                var roleName = _settings.Value.NewUserStartingRole;
                if (_settings.Value.AdminEmailAddresses.Any() && _settings.Value.AdminEmailAddresses.Contains(user.Email!))
                {
                    roleName = Constants.Roles.AdminRoleName;
                }

                // Check if the role exists, create if not.
                if (roleName != null)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(roleName);
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new Role {Name = roleName});
                    }

                    // Add user to the role.
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    if (!result.Succeeded)
                    {
                        // Handle failure to add user to role here.
                        _logger.LogError("Unable to add {UserUserName} to the role {RoleName}", user.UserName, roleName);
                    }
                    else
                    {
                        // Sign in the user again to update their claims.
                        await base.SignOutAsync();
                        signInResult = await base.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
                    }
                }
                else
                {
                    _logger.LogError("Unable to add {UserUserName} to a role as the roleName was null", user.UserName);
                }
            }
        }

        return signInResult;
    }
}