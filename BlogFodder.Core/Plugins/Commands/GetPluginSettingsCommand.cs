using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;

namespace BlogFodder.Core.Plugins.Commands;

public class GetPluginSettingsCommand : IRequest<HandlerResult<GlobalSettings>>
{
    public string? Alias { get; set; }
}