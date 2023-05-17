using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;

namespace BlogFodder.Core.Plugins.Handlers;

public class CreateUpdatePluginSettingsHandler : IRequestHandler<SavePluginSettingsCommand, HandlerResult<PluginSettings>>
{
    private readonly BlogFodderDbContext _dbContext;
    private readonly ICacheService _cacheService;
    
    public CreateUpdatePluginSettingsHandler(BlogFodderDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }
    
    public async Task<HandlerResult<PluginSettings>> Handle(SavePluginSettingsCommand request, CancellationToken cancellationToken)
    {
        var result = new HandlerResult<PluginSettings>();
        
        result = await _dbContext.CreateOrUpdate(request.Settings, result, !request.IsUpdate, cancellationToken)
            .ConfigureAwait(false);
        
        // Clear the cache
        _cacheService.ClearCachedItemsWithPrefix(nameof(PluginSettings));
        
        return result;
    }
}