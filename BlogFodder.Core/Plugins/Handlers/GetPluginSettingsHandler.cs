using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Plugins.Handlers;

public class GetPluginSettingsHandler : IRequestHandler<GetPluginSettingsCommand, HandlerResult<GlobalSettings>>
{
    private readonly BlogFodderDbContext _dbContext;

    public GetPluginSettingsHandler(BlogFodderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HandlerResult<GlobalSettings>> Handle(GetPluginSettingsCommand request, CancellationToken cancellationToken)
    {
        var result = new HandlerResult<GlobalSettings>();

        var pluginSettings = await _dbContext.PluginSettings.AsNoTracking().FirstOrDefaultAsync(x => x.Alias == request.Alias, cancellationToken: cancellationToken);
        if (pluginSettings != null)
        {
            result.Entity = pluginSettings;
        }
        else
        {
            result.Success = false;
            result.AddMessage("Unable to find any settings with that alias", HandlerResultMessageType.Error);
        }
        
        return result;
    }
}