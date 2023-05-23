using BlogFodder.Core.Backoffice.Models;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Plugins.Models;
using MudBlazor;

namespace BlogFodder.Plugins.PostSideBar;

public class PostSideBarPlugin : IPlugin
{
    public string Alias => nameof(PostSideBarPlugin);
    public string Name => "Post Side Bar";
    public string Description => "Default plugin for items in the side bar on the posts";
    public string Icon => Icons.Material.Filled.AddRoad;
    public PostPluginEditor? Editor { get; set; }

    public List<ContentPlugin>? Content { get; set; } = new()
    {
        new ContentPlugin
        {
            PluginDisplayArea = PluginDisplayArea.PostSideBarTop,
            Component = typeof(PostSideBarAboutBox)
        },
        new ContentPlugin
        {
            PluginDisplayArea = PluginDisplayArea.PostSideBarTop,
            Component = typeof(PostSideBarLatestPosts)
        }
    };

    public SettingsPlugin? Settings { get; set; } = new()
    {
        Component = typeof(PostSideBarSettings),
        BackOfficeLink = new List<Link>()
        {
            new()
            {
                Text = "Side Bar (Post)",
                Route = "/admin/plugins/postsidebar"
            }
        },
        Model = new PostSideBarSettingsModel()
    };
}