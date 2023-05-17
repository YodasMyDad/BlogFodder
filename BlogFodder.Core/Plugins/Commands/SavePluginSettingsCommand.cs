using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;

namespace BlogFodder.Core.Plugins.Commands;

public class SavePluginSettingsCommand : IRequest<HandlerResult<PluginSettings>>
{
    public bool IsUpdate { get; set; }
    public PluginSettings? Settings { get; set; }
}