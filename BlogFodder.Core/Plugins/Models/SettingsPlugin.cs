using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Core.Plugins.Models;

public class SettingsPlugin
{
    public Type? Component { get; set; }
    public IPluginSettings? Model { get; set; }
    public List<string> JsFiles { get; set; } = new();
    public List<string> CssFiles { get; set; } = new();
}