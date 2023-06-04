using System.Reflection;
using BlogFodder.Core;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Morris.Blazor.Validation;

namespace BlogFodder.Plugins;

public static class ServiceCollectionExtensions
{
    public static void EnableBlogFodderPlugins(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Authentication
        var authBuilder = services.AddAuthentication();
        
        // Build the service provider and get the extension manager
        var serviceProvider = services.BuildServiceProvider();
        var extensionManager = serviceProvider.GetService<ExtensionManager>();
        
        // Plugins
        var assemblyProvider = new DefaultAssemblyProvider(serviceProvider);
        var assemblies = assemblyProvider.GetAssemblies();
        Assembly[] discoverAssemblies = (assemblies as Assembly[] ?? assemblies.ToArray())!;
        AssemblyManager.SetAssemblies(discoverAssemblies);

        // Mediatr
        services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(discoverAssemblies));

        // Start up items
        var startUpItems = extensionManager?.GetInstances<IStartupPlugin>();
        if (startUpItems != null)
        {
            foreach (var startUpItem in startUpItems)
            {
                startUpItem.Value.Register(services, configuration);
            }   
        }

        // Hosted services (IHostedService)
        var hostedServices = extensionManager?.GetImplementations<IHostedService>();
        if (hostedServices != null)
        {
            foreach (var hs in hostedServices)
            {
                var method = typeof(ServiceCollectionHostedServiceExtensions)
                    .GetMethod("AddHostedService", new[] { typeof(IServiceCollection) });
                if (method != null)
                {
                    var generic = method.MakeGenericMethod(hs);
                    generic.Invoke(null, new object[] { services });
                }
            }   
        }

        // Validation
        services.AddFormValidation(config =>
            config
                .AddDataAnnotationsValidation()
                //.AddFluentValidation(typeof(Constants).Assembly, discoverAssemblies)
        );
        
        // Add external authentication providers
        foreach (var provider in extensionManager?.GetInstances<IExternalAuthenticationProvider>()!)
        {
            provider.Value.Add(services, authBuilder, configuration);
        }
    }
}