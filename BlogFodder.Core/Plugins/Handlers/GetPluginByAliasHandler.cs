using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Plugins.Handlers;

public class GetPluginByAliasHandler : IRequestHandler<GetPluginByAliasCommand, Plugin?>
{
    private readonly BlogFodderDbContext _dbContext;
    private readonly ICacheService _cacheService;
    public GetPluginByAliasHandler(BlogFodderDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }
    
    public async Task<Plugin?> Handle(GetPluginByAliasCommand request, CancellationToken cancellationToken)
    {
        return await _cacheService.GetSetCachedItemAsync(typeof(Plugin).ToCacheKey(request.Alias!), async () =>
        {
            return await _dbContext.Plugins.AsNoTracking().FirstOrDefaultAsync(x => x.PluginAlias == request.Alias, cancellationToken: cancellationToken);
        });
    }
}