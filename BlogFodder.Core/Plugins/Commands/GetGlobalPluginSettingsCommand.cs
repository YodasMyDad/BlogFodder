using BlogFodder.Core.Plugins.Models;
using MediatR;

namespace BlogFodder.Core.Plugins.Commands;

public class GetGlobalPluginSettingsCommand : IRequest<GlobalPluginSettings?>
{
    public string? Alias { get; set; }

}