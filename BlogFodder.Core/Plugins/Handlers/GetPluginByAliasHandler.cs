using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Plugins.Handlers;

public class GetPluginByAliasHandler : IRequestHandler<GetPluginByAliasCommand, Plugin?>
{
    private readonly ICacheService _cacheService;
    private readonly IServiceProvider _serviceProvider;
    public GetPluginByAliasHandler(ICacheService cacheService, IServiceProvider serviceProvider)
    {
        _cacheService = cacheService;
        _serviceProvider = serviceProvider;
    }
    
    public async Task<Plugin?> Handle(GetPluginByAliasCommand request, CancellationToken cancellationToken)
    {
        return await _cacheService.GetSetCachedItemAsync(typeof(Plugin).ToCacheKey(request.Alias!), async () =>
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
            return await dbContext.Plugins.AsNoTracking().FirstOrDefaultAsync(x => x.PluginAlias == request.Alias, cancellationToken: cancellationToken);
        });
    }
}