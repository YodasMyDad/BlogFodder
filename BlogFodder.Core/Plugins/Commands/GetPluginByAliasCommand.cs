using BlogFodder.Core.Plugins.Models;
using MediatR;

namespace BlogFodder.Core.Plugins.Commands;

public class GetPluginByAliasCommand : IRequest<Plugin?>
{
    public string? Alias { get; set; }
}