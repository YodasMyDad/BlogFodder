using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Plugins.Handlers;

public class CreateUpdatePluginHandler : IRequestHandler<CreateUpdatePluginCommand, HandlerResult<Plugin>>
{
    private readonly ICacheService _cacheService;
    private readonly IServiceProvider _serviceProvider;
    
    public CreateUpdatePluginHandler(ICacheService cacheService, IServiceProvider serviceProvider)
    {
        _cacheService = cacheService;
        _serviceProvider = serviceProvider;
    }
    
    public async Task<HandlerResult<Plugin>> Handle(CreateUpdatePluginCommand request, CancellationToken cancellationToken)
    {
        var result = new HandlerResult<Plugin>();
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
        result = await dbContext.CreateOrUpdate(request.Plugin, result, !request.IsUpdate, cancellationToken)
            .ConfigureAwait(false);
        
        // Clear the cache
        _cacheService.ClearCachedItemsWithPrefix(nameof(Plugin));
        
        return result;
    }
}