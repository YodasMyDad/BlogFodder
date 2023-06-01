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

    public override async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor)
    {
        var signInResult = await base.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);

        // If the user successfully signed in, add them to a role.
        if (signInResult.Succeeded)
        {
            var user = await _userManager.FindByLoginAsync(loginProvider, providerKey);

            if (user != null)
            {
                // Check if the role exists, create if not.
                var roleName = _settings.Value.NewUserStartingRole;
                if (roleName != null)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(roleName);
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new Role { Name = roleName});
                    }

                    // Add user to the role.
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    if (!result.Succeeded)
                    {
                        // Handle failure to add user to role here.
                        _logger.LogError("Unable to add {UserUserName} to the role {RoleName}", user.UserName, roleName);
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