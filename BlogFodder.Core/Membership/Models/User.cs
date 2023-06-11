using BlogFodder.Core.Media.Models;
using BlogFodder.Core.Posts.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogFodder.Core.Membership.Models;

public class User : IdentityUser<Guid>
{
    public List<UserRole> UserRoles { get; set; } = new();

    public List<Post> Posts { get; set; } = new();
    public BlogFodderFile? ProfileImage { get; set; }
    public Guid? ProfileImageId { get; set; }
    
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    public Dictionary<string, object> ExtendedData { get; set; } = new();
}