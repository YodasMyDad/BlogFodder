using BlogFodder.Core;
using BlogFodder.Core.Backoffice.Models;
using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.App.Pages.Admin;

/// <summary>
/// The navigation items for the post items
/// </summary>
public class PostNavigation : IBackOfficeNavigationItem
{
    public Link Link { get; set; } = new()
    {
        Text = "Posts",
        Section = Constants.BackOffice.NavigationSectionCore,
        SubLinks = new List<Link>
        {
            new ()
            {
                Route = "/admin/viewposts",
                Text = "View Posts"
            },
            new ()
            {
                Route = "/admin/post",
                Text = "Create Post"
            }
        }
    };
}