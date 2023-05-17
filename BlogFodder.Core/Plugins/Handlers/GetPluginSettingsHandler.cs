using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Plugins.Handlers;

public class GetPluginSettingsHandler : IRequestHandler<GetPluginSettingsCommand, PluginSettings?>
{
    private readonly BlogFodderDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public GetPluginSettingsHandler(BlogFodderDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }

    public async Task<PluginSettings?> Handle(GetPluginSettingsCommand request, CancellationToken cancellationToken)
    {
        return await _cacheService.GetSetCachedItemAsync(typeof(PluginSettings).ToCacheKey(request.Alias!), async () =>
        {
            return await _dbContext.PluginSettings.AsNoTracking().FirstOrDefaultAsync(x => x.Alias == request.Alias, cancellationToken: cancellationToken);
        });
    }
}