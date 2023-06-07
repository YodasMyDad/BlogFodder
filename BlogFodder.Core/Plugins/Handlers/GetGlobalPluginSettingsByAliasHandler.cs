using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Plugins.Handlers;

public class GetGlobalPluginSettingsByAliasHandler : IRequestHandler<GetGlobalPluginSettingsByAliasCommand, List<GlobalPluginSettings>>
{
    private readonly ICacheService _cacheService;
    private readonly IServiceProvider _serviceProvider;
    
    public GetGlobalPluginSettingsByAliasHandler(ICacheService cacheService, IServiceProvider serviceProvider)
    {
        _cacheService = cacheService;
        _serviceProvider = serviceProvider;
    }
    
    public async Task<List<GlobalPluginSettings>> Handle(GetGlobalPluginSettingsByAliasCommand request, CancellationToken cancellationToken)
    {
        if (request.Aliases != null)
        {
            return await _cacheService.GetSetCachedItemAsync(typeof(GlobalPluginSettings).ToCacheKey(request.Aliases), async () =>
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
                return await dbContext.PluginSettings.AsNoTracking().Where(x => request.Aliases.Contains(x.Alias)).ToListAsync(cancellationToken: cancellationToken);
            });
        }

        return new List<GlobalPluginSettings>();
    }
}