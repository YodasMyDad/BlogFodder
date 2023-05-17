using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;

namespace BlogFodder.Core.Plugins.Commands;

public class CreateUpdateGlobalPluginSettingsCommand : IRequest<HandlerResult<GlobalPluginSettings>>
{
    public bool IsUpdate { get; set; }
    public GlobalPluginSettings? Settings { get; set; }
}