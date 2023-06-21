using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Plugins.PostSideBar;

public class PostSideBarSettingsModel : IPluginSettings
{
    public bool Enabled { get; set; }

    public bool AboutBoxEnabled { get; set; } = true;
    public string? AboutBoxTitle { get; set; }
    public Guid? AboutBoxImageId { get; set; }
    public string? AboutBoxImageUrl { get; set; }
    public string? AboutBoxText { get; set; }
    public int AboutBoxImageHeight { get; set; } = 160;
    public int AboutBoxImageWidth { get; set; } = 400;
    
    public int LatestPostsAmount { get; set; } = 3;
}