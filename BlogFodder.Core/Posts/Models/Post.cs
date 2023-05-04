using BlogFodder.Core.Extensions;
using BlogFodder.Core.Media;
using BlogFodder.Core.Media.Models;

namespace BlogFodder.Core.Posts.Models;

public class Post
{
    public Guid Id { get; set; } = Guid.NewGuid().NewSequentialGuid();
    public string? Name { get; set; }
    public DateTime? DateCreated { get; set; } = DateTime.Now;
    public DateTime? DateUpdated { get; set; } = DateTime.Now;
    
    public string? Excerpt { get; set; } 
    
    public int AuthorId { get; set; }

    public Guid? FeaturedImageId { get; set; }
    public virtual BlogFodderFile? FeaturedImage { get; set; }
    
    // TODO - Tags, Categories?
    
    // SEO
    public string? PageTitle { get; set; }
    public string? MetaDescription { get; set; }
    public bool NoIndex { get; set; }
    
    public Guid? SocialImageId { get; set; }
    public virtual BlogFodderFile? SocialImage { get; set; }
    public string? Url { get; set; }
    
    public Dictionary<string, string> ExtendedData { get; set; } = new();

    public List<PostContentItem> ContentItems { get; set; } = new();
}