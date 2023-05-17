using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Plugins.Handlers;

public class GetPluginSettingsByAliasHandler : IRequestHandler<GetPluginSettingsByAliasCommand, List<PluginSettings>>
{
    private readonly BlogFodderDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public GetPluginSettingsByAliasHandler(ICacheService cacheService, BlogFodderDbContext dbContext)
    {
        _cacheService = cacheService;
        _dbContext = dbContext;
    }
    
    public async Task<List<PluginSettings>> Handle(GetPluginSettingsByAliasCommand request, CancellationToken cancellationToken)
    {
        if (request.Aliases != null)
        {
            return await _cacheService.GetSetCachedItemAsync(typeof(PluginSettings).ToCacheKey(request.Aliases), async () =>
            {
                return await _dbContext.PluginSettings.AsNoTracking().Where(x => request.Aliases.Contains(x.Alias)).ToListAsync(cancellationToken: cancellationToken);
            });
        }

        return new List<PluginSettings>();
    }
}