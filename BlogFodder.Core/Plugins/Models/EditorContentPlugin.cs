using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Core.Plugins.Models;

public class EditorContentPlugin
{
    public Type? Component { get; set; }
    public List<string> JsFiles { get; set; } = new();
    public List<string> CssFiles { get; set; } = new();
}