using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Plugins.Interfaces;

public interface IExternalAuthenticationProvider
{
    void Add(IServiceCollection servicesCollection, AuthenticationBuilder authenticationBuilder, IConfiguration configuration);
}