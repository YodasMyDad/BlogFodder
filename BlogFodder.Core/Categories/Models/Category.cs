using BlogFodder.Core.Extensions;
using BlogFodder.Core.Media.Models;
using BlogFodder.Core.Posts.Models;

namespace BlogFodder.Core.Categories.Models;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid().NewSequentialGuid();
    public string? Name { get; set; }
    public int SortOrder { get; set; }
    public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateUpdated { get; set; } = DateTime.UtcNow;

    public int PostsPerPage { get; set; } = 10;
    
    // SEO
    public string? PageTitle { get; set; }
    public string? MetaDescription { get; set; }
    public bool NoIndex { get; set; }
    public Guid? SocialImageId { get; set; }
    public virtual BlogFodderFile? SocialImage { get; set; }
    public string? Url { get; set; }
    public Dictionary<string, string> ExtendedData { get; set; } = new();
    public List<Post> Posts { get; set; } = new();
}