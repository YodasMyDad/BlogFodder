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

public class CreateUpdateGlobalPluginSettingsHandler : IRequestHandler<CreateUpdateGlobalPluginSettingsCommand, HandlerResult<GlobalPluginSettings>>
{
    private readonly ICacheService _cacheService;
    private readonly IServiceProvider _serviceProvider;
    public CreateUpdateGlobalPluginSettingsHandler(ICacheService cacheService, IServiceProvider serviceProvider)
    {
        _cacheService = cacheService;
        _serviceProvider = serviceProvider;
    }
    
    public async Task<HandlerResult<GlobalPluginSettings>> Handle(CreateUpdateGlobalPluginSettingsCommand request, CancellationToken cancellationToken)
    {
        var result = new HandlerResult<GlobalPluginSettings>();
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
        
        var settings = dbContext.PluginSettings
            .FirstOrDefault(x => request.Settings != null && x.Id == request.Settings.Id);
        
        settings ??= new GlobalPluginSettings();

        settings.Data = request.Settings?.Data;
        settings.Alias = request.Settings?.Alias;
        
        if (!request.IsUpdate)
        {
            dbContext.PluginSettings.Add(settings);
        }
        
        result = await dbContext.SaveChangesAndLog(settings, result, cancellationToken);
        
        // Clear the cache
        _cacheService.ClearCachedItemsWithPrefix(nameof(GlobalPluginSettings));
        
        return result;
    }
}