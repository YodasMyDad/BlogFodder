namespace BlogFodder.Plugins.Comments.Models;

public class PostComment
{
    public DateTime DateCreated { get; set; }
    public Guid PostId { get; set; }
    public string? Name { get; set; }
    public string? Comment { get; set; }
}