using System.Reflection;
using BlogFodder.Core.Plugins;

namespace BlogFodder.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static Assembly?[] DiscoverAssemblies(this IServiceProvider serviceProvider)
    {
        var assemblyProvider = new DefaultAssemblyProvider(serviceProvider);
        var assemblies = assemblyProvider.GetAssemblies();
        var discoverAssemblies = assemblies as Assembly?[] ?? assemblies.ToArray();
        AssemblyManager.SetAssemblies(discoverAssemblies);
        return discoverAssemblies;
    }
}