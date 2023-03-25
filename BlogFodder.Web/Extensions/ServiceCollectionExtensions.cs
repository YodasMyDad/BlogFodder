using BlogFodder.Core.Plugins;

namespace BlogFodder.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceProvider DiscoverAssemblies(this IServiceProvider serviceProvider)
    {
        var assemblyProvider = new DefaultAssemblyProvider(serviceProvider);
        AssemblyManager.SetAssemblies(assemblyProvider.GetAssemblies());
        return serviceProvider;
    }
}