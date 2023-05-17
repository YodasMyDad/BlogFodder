using BlogFodder.Core.Extensions;

namespace BlogFodder.Core.Plugins.Models;

public class GlobalPluginSettings
{
    public Guid Id { get; set; } = Guid.NewGuid().NewSequentialGuid();
    
    public string? Alias { get; set; }
    
    public string? Data { get; set; }
}