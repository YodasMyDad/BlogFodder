using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Plugins.Handlers;

public class GetGlobalPluginSettingsHandler : IRequestHandler<GetGlobalPluginSettingsCommand, GlobalPluginSettings?>
{
    private readonly ICacheService _cacheService;
    private readonly IServiceProvider _serviceProvider;
    
    public GetGlobalPluginSettingsHandler(ICacheService cacheService, IServiceProvider serviceProvider)
    {
        _cacheService = cacheService;
        _serviceProvider = serviceProvider;
    }

    public async Task<GlobalPluginSettings?> Handle(GetGlobalPluginSettingsCommand request, CancellationToken cancellationToken)
    {
        return await _cacheService.GetSetCachedItemAsync(typeof(GlobalPluginSettings).ToCacheKey(request.Alias!), async () =>
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
            return await dbContext.PluginSettings.AsNoTracking().FirstOrDefaultAsync(x => x.Alias == request.Alias, cancellationToken: cancellationToken);
        });
    }
}