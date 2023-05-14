using MudBlazor;

namespace BlogFodder.Core.Plugins.Models;

public class PostPlugin
{
    public Type? Component { get; set; }
    public string Icon { get; set; } = Icons.Material.Filled.ArrowRight;
    public List<string> JsFiles { get; set; } = new();
    public List<string> CssFiles { get; set; } = new();
}