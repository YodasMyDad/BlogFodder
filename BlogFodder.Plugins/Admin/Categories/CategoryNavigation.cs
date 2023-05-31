using BlogFodder.Core;
using BlogFodder.Core.Backoffice.Models;
using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Plugins.Admin.Categories;

public class CategoryNavigation : IBackOfficeNavigationItem
{
    public Link Link { get; set; } = new()
    {
        Text = "Categories",
        SortOrder = 2,
        Section = Constants.BackOffice.NavigationSectionCore,
        SubLinks = new List<Link>
        {
            new ()
            {
                Route = "/admin/categories",
                Text = "View Categories"
            },
            new ()
            {
                Route = "/admin/createcategory",
                Text = "Create Category"
            }
        }
    };
}