using BlogFodder.Core;
using BlogFodder.Core.Backoffice.Models;
using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.App.Pages.Admin.Settings;

public class SettingsNavigation : IBackOfficeNavigationItem
{
    public Link Link { get; set; } = new()
    {
        Text = "Settings",
        SortOrder = 0,
        Section = Constants.BackOffice.NavigationSectionCore,
        Route = "/admin/settings"
    };
}