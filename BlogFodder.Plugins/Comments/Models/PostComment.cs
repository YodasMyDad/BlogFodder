using BlogFodder.Core.Membership.Models;

namespace BlogFodder.Plugins.Comments.Models;

public class PostComment
{
    public DateTime DateCreated { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string? Comment { get; set; }
}