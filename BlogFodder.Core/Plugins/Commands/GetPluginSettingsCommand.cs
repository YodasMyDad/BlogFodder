using BlogFodder.Core.Plugins.Models;
using MediatR;

namespace BlogFodder.Core.Plugins.Commands;

public class GetPluginSettingsCommand : IRequest<PluginSettings?>
{
    public string? Alias { get; set; }

}