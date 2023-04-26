using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Plugins.Authentication;

public class MicrosoftAuthentication : IExternalAuthenticationProvider
{
    public void Add(IServiceCollection servicesCollection, AuthenticationBuilder authenticationBuilder, IConfiguration configuration)
    {
        var microsoftId = configuration.GetValue<string>("BlogFodder:Identity:ExternalProviders:Microsoft:ClientId");
        if (!microsoftId.IsNullOrWhiteSpace())
        {
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins?view=aspnetcore-5.0
            authenticationBuilder.AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = microsoftId;
                microsoftOptions.ClientSecret = configuration.GetValue<string>("BlogFodder:Identity:ExternalProviders:Microsoft:ClientSecret") ?? "";
            });
        }
    }
}