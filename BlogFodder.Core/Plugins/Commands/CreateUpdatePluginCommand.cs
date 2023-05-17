using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;

namespace BlogFodder.Core.Plugins.Commands;

public class CreateUpdatePluginCommand : IRequest<HandlerResult<Plugin>>
{
    public bool IsUpdate { get; set; }
    public Plugin? Plugin { get; set; }
}