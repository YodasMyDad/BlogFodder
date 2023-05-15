namespace BlogFodder.Plugins.Comments.Models;

public class PostComment
{
    public DateTime DateCreated { get; set; }
    public string? Name { get; set; }
    public string? Comment { get; set; }
    public bool Flagged { get; set; }
}