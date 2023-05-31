using BlogFodder.Core;
using BlogFodder.Core.Backoffice.Models;
using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.App.Pages.Admin.Posts;

/// <summary>
/// The navigation items for the post items
/// </summary>
public class PostNavigation : IBackOfficeNavigationItem
{
    public Link Link { get; set; } = new()
    {
        Text = "Posts",
        Open = true,
        Section = Constants.BackOffice.NavigationSectionCore,
        SubLinks = new List<Link>
        {
            new ()
            {
                Route = "/admin/posts",
                Text = "View Posts"
            },
            new ()
            {
                Route = "/admin/createpost",
                Text = "Create Post"
            }
        }
    };
}