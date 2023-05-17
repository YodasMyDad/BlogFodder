using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;

namespace BlogFodder.Core.Plugins.Handlers;

public class CreateUpdatePluginHandler : IRequestHandler<CreateUpdatePluginCommand, HandlerResult<Plugin>>
{
    private readonly BlogFodderDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public CreateUpdatePluginHandler(BlogFodderDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }
    
    public async Task<HandlerResult<Plugin>> Handle(CreateUpdatePluginCommand request, CancellationToken cancellationToken)
    {
        var result = new HandlerResult<Plugin>();
        
        result = await _dbContext.CreateOrUpdate(request.Plugin, result, !request.IsUpdate, cancellationToken)
            .ConfigureAwait(false);
        
        // Clear the cache
        _cacheService.ClearCachedItemsWithPrefix(nameof(Plugin));
        
        return result;
    }
}