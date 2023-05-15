namespace BlogFodder.Core.Plugins.Models;

public class ContentPlugin
{
    public Type? Component { get; set; }
    public List<string> JsFiles { get; set; } = new();
    public List<string> CssFiles { get; set; } = new();
    public PluginDisplayArea? PluginDisplayArea { get; set; }
}