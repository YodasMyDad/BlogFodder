using BlogFodder.Core.Extensions;

namespace BlogFodder.Core.Posts.Models;

public class PostPlugin
{
    public Post Post { get; set; } = default!;
    public Guid PostId { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid().NewSequentialGuid();
    public string? PluginAlias { get; set; }
    public string? PluginData { get; set; }
    public string? PluginSettings { get; set; }
    
    // EF Ignore
    public string? GlobalSettings { get; set; }
    public bool IsNew { get; set; }
}