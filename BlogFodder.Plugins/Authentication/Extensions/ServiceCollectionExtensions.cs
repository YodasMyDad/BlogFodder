using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Plugins.Authentication.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddExternalAuthenticationProviders(this IServiceCollection services, IConfiguration configuration)
    {
        var authBuilder = services.AddAuthentication();
        var serviceProvider = services.BuildServiceProvider();
        var extensionManager = serviceProvider.GetService<ExtensionManager>();
        
        foreach (var provider in extensionManager?.GetInstances<IExternalAuthenticationProvider>()!)
        {
            provider.Value.Add(services, authBuilder, configuration);
        }
    }
}