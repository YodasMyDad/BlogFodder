using BlogFodder.Core;
using BlogFodder.Core.Backoffice.Models;
using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Plugins.Admin.Membership;

public class MembershipNavigation : IBackOfficeNavigationItem
{
    public Link Link { get; set; } = new()
    {
        Text = "Membership",
        SortOrder = 6,
        Section = Constants.BackOffice.NavigationSectionCore,
        SubLinks = new List<Link>
        {
            new ()
            {
                Route = "/admin/membership/users",
                Text = "Users"
            },
            new ()
            {
                Route = "/admin/membership/roles",
                Text = "Roles"
            }
        }
    };
}