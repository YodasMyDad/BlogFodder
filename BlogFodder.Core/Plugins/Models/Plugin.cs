using BlogFodder.Core.Extensions;

namespace BlogFodder.Core.Plugins.Models;

public class Plugin
{
    public Guid Id { get; set; } = Guid.NewGuid().NewSequentialGuid();
    public string PluginAlias { get; set; } = "";
    public string? PluginData { get; set; }
    public string? PluginSettings { get; set; }
    public bool Enabled { get; set; }
}