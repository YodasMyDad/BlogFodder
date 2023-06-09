using System.Reflection;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Plugins;

public static class ServiceCollectionExtensions
{
    public static void EnableBlogFodderPlugins(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Authentication
        var authBuilder = services.AddAuthentication();
        
        // Build the service provider and get the extension manager
        var serviceProvider = services.BuildServiceProvider();
        var extensionManager = serviceProvider.GetRequiredService<ExtensionManager>();
        
        // Plugins
        var assemblyProvider = new DefaultAssemblyProvider(serviceProvider);
        var assemblies = assemblyProvider.GetAssemblies();
        Assembly[] discoverAssemblies = (assemblies as Assembly[] ?? assemblies.ToArray())!;
        AssemblyManager.SetAssemblies(discoverAssemblies);

        // Mediatr
        services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(discoverAssemblies));

        // Automapper
        services.AddAutoMapper(discoverAssemblies);
        
        // Start up items
        var startUpItems = extensionManager?.GetInstances<IStartupPlugin>();
        if (startUpItems != null)
        {
            foreach (var startUpItem in startUpItems)
            {
                startUpItem.Value.Register(services, configuration);
            }   
        }

        // Add external authentication providers
        foreach (var provider in extensionManager?.GetInstances<IExternalAuthenticationProvider>()!)
        {
            provider.Value.Add(services, authBuilder, configuration);
        }
    }
}