using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Plugins.Authentication;

public class FacebookAuthentication : IExternalAuthenticationProvider
{
    public void Add(IServiceCollection servicesCollection, AuthenticationBuilder authenticationBuilder,
        IConfiguration configuration)
    {
        var facebookId = configuration.GetValue<string>("BlogFodder:Identity:ExternalProviders:Facebook:AppId");
        if (!facebookId.IsNullOrWhiteSpace())
        {
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/facebook-logins?view=aspnetcore-5.0
            authenticationBuilder.AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = facebookId;
                facebookOptions.AppSecret =
                    configuration.GetValue<string>("BlogFodder:Identity:ExternalProviders:Facebook:AppSecret") ?? "";
            });
        }
    }
}