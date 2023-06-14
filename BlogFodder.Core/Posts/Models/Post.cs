using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Media.Models;
using BlogFodder.Core.Membership.Models;

namespace BlogFodder.Core.Posts.Models;

public class Post
{
    public Guid Id { get; set; } = Guid.NewGuid().NewSequentialGuid();
    public string? Name { get; set; }
    public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateUpdated { get; set; } = DateTime.UtcNow;
    
    public string? Excerpt { get; set; } 
    
    public Guid? FeaturedImageId { get; set; }
    public BlogFodderFile? FeaturedImage { get; set; }
    
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    
    // SEO
    public string? PageTitle { get; set; }
    public string? MetaDescription { get; set; }
    public bool NoIndex { get; set; }
    
    public Guid? SocialImageId { get; set; }
    public BlogFodderFile? SocialImage { get; set; }
    public string? Url { get; set; }
    
    public Dictionary<string, string> ExtendedData { get; set; } = new();

    public List<PostContentItem> ContentItems { get; set; } = new();

    public List<Category> Categories { get; set; } = new();
}