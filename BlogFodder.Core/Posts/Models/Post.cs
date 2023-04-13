namespace BlogFodder.Core.Posts.Models;

public class Post
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    
    public virtual ICollection<PostContentItem> ContentItems { get; set; }
}