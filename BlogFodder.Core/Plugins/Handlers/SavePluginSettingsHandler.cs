using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;

namespace BlogFodder.Core.Plugins.Handlers;

public class SavePluginSettingsHandler : IRequestHandler<SavePluginSettingsCommand, HandlerResult<GlobalSettings>>
{
    private readonly BlogFodderDbContext _dbContext;

    public SavePluginSettingsHandler(BlogFodderDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<HandlerResult<GlobalSettings>> Handle(SavePluginSettingsCommand request, CancellationToken cancellationToken)
    {
        var result = new HandlerResult<GlobalSettings>();
        return await _dbContext.CreateOrUpdate(request.Settings, result, !request.IsUpdate, cancellationToken)
            .ConfigureAwait(false);
    }
}