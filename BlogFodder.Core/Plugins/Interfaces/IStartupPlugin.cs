using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Plugins.Interfaces;

public interface IStartupPlugin
{
    void Register(IServiceCollection services, IConfiguration configuration);
}