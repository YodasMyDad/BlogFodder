using BlogFodder.Core.Extensions;

namespace BlogFodder.Core.Plugins.Models;

public class PluginSettings
{
    public Guid Id { get; set; } = Guid.NewGuid().NewSequentialGuid();
    
    public string? Alias { get; set; }
    
    public string? Data { get; set; }
}