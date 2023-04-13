namespace BlogFodder.Core.Posts.Models;

public class PostContentItem
{
    public virtual Post Post { get; set; } = default!;
    public int PostId { get; set; }
    public int Id { get; set; }
    public int SortOrder { get; set; }
    public string? PluginAlias { get; set; }
    public string? PluginData { get; set; }
}