using AutoMapper;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Plugins.Handlers;

public class CreateUpdatePluginHandler : IRequestHandler<CreateUpdatePluginCommand, HandlerResult<Plugin>>
{
    private readonly ICacheService _cacheService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;
    
    public CreateUpdatePluginHandler(ICacheService cacheService, IServiceProvider serviceProvider, IMapper mapper)
    {
        _cacheService = cacheService;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }
    
    public async Task<HandlerResult<Plugin>> Handle(CreateUpdatePluginCommand request, CancellationToken cancellationToken)
    {
        var result = new HandlerResult<Plugin>();
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
        
        var plugin = dbContext.Plugins
            .FirstOrDefault(x => request.Plugin != null && x.Id == request.Plugin.Id);
        
        plugin ??= new Plugin();
        
        _mapper.Map(request.Plugin, plugin);
        
        if (!request.IsUpdate)
        {
            dbContext.Plugins.Add(plugin);
        }
        
        result = await dbContext.SaveChangesAndLog(plugin, result, cancellationToken);
        
        // Clear the cache
        _cacheService.ClearCachedItemsWithPrefix(nameof(Plugin));
        
        return result;
    }
}