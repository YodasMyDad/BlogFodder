using BlogFodder.Core.Extensions;
using BlogFodder.Core.Media.Models;

namespace BlogFodder.Core.Settings.Models;

public class SiteSettings
{
    public Guid Id { get; set; } = Guid.NewGuid().NewSequentialGuid();
    
    public DateTime? DateUpdated { get; set; } = DateTime.UtcNow;
    
    // Site Settings
    public Guid? LogoId { get; set; }
    public virtual BlogFodderFile? Logo { get; set; }
    public string SiteName { get; set; } = "BlogFodder";
    public string DefaultPageTitle { get; set; } = "BlogFodder - Blazor Blog Engine";
    public string DefaultMetaDescription { get; set; } = "A plugin based blog engine built in .Net C# Blazor";
    public int HomeAmountPerPage { get; set; } = 10;
    
    // Social
    public string? Instagram { get; set; }
    public string? Twitter { get; set; }
    public string? Facebook { get; set; }
    public string? Pinterest { get; set; }
    public string? LinkedIn { get; set; }
    
    public Dictionary<string, string> ExtendedData { get; set; } = new();
}