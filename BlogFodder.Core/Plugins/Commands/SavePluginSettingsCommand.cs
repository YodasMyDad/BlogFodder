using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;

namespace BlogFodder.Core.Plugins.Commands;

public class SavePluginSettingsCommand : IRequest<HandlerResult<GlobalSettings>>
{
    public bool IsUpdate { get; set; }
    public GlobalSettings? Settings { get; set; }
}