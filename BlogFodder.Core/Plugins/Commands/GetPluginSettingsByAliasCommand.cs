using BlogFodder.Core.Plugins.Models;
using MediatR;

namespace BlogFodder.Core.Plugins.Commands;

public class GetPluginSettingsByAliasCommand : IRequest<List<PluginSettings>>
{
    public List<string>? Aliases { get; set; }
}