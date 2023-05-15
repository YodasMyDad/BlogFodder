using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;

namespace BlogFodder.Core.Plugins.Handlers;

public class CreateUpdatePluginSettingsHandler : IRequestHandler<SavePluginSettingsCommand, HandlerResult<GlobalSettings>>
{
    private readonly BlogFodderDbContext _dbContext;
    private readonly ICacheService _cacheService;
    
    public CreateUpdatePluginSettingsHandler(BlogFodderDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }
    
    public async Task<HandlerResult<GlobalSettings>> Handle(SavePluginSettingsCommand request, CancellationToken cancellationToken)
    {
        var result = new HandlerResult<GlobalSettings>();
        
        result = await _dbContext.CreateOrUpdate(request.Settings, result, !request.IsUpdate, cancellationToken)
            .ConfigureAwait(false);
        
        // Clear the cache
        _cacheService.ClearCachedItem(typeof(GlobalSettings).ToCacheKey(result.Entity.Alias!));
        
        return result;
    }
}