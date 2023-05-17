using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Plugins.Handlers;

public class GetGlobalPluginSettingsHandler : IRequestHandler<GetGlobalPluginSettingsCommand, GlobalPluginSettings?>
{
    private readonly BlogFodderDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public GetGlobalPluginSettingsHandler(BlogFodderDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }

    public async Task<GlobalPluginSettings?> Handle(GetGlobalPluginSettingsCommand request, CancellationToken cancellationToken)
    {
        return await _cacheService.GetSetCachedItemAsync(typeof(GlobalPluginSettings).ToCacheKey(request.Alias!), async () =>
        {
            return await _dbContext.PluginSettings.AsNoTracking().FirstOrDefaultAsync(x => x.Alias == request.Alias, cancellationToken: cancellationToken);
        });
    }
}