using BlogFodder.Core.Plugins.Models;
using MediatR;

namespace BlogFodder.Core.Plugins.Commands;

public class GetGlobalPluginSettingsByAliasCommand : IRequest<List<GlobalPluginSettings>>
{
    public List<string>? Aliases { get; set; }
}