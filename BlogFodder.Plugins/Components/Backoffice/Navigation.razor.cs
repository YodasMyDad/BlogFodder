using System.Reflection.Metadata;
using BlogFodder.Core;
using BlogFodder.Core.Backoffice.Models;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Settings;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;

namespace BlogFodder.Plugins.Components.Backoffice;

public partial class Navigation : ComponentBase
{
    [Inject] public IOptions<BlogFodderSettings> Settings { get; set; } = default!;
    [Inject] public ExtensionManager ExtensionManager { get; set; } = default!;

    public Dictionary<string, NavigationSection> NavigationSections { get; set; } = new();

    protected override void OnInitialized()
    {
        NavigationSections.Add(Constants.BackOffice.NavigationSectionCore, new NavigationSection
        {
            Name = ""
        });

        NavigationSections.Add(Constants.BackOffice.NavigationSectionPlugins, new NavigationSection
        {
            Name = Constants.BackOffice.NavigationSectionPlugins
        });

        var settings = Settings.Value;
        if (settings.BackOffice.NavigationSections.Any())
        {
            foreach (var section in settings.BackOffice.NavigationSections)
            {
                NavigationSections.Add(section, new NavigationSection
                {
                    Name = section
                });
            }
        }

        if (NavigationSections.TryGetValue(Constants.BackOffice.NavigationSectionCore, out var coreSection))
        {
            // TODO - Could do this with the IBackOfficeNavigationItem, but we need to add a sort order to links!
            // So we'll add it like this for now, so it's always first
            coreSection.Links.Add(new Link
            {
                Route = "/admin", // TODO - Again, need a better way to store routes rather than magic strings
                Text = "Dashboard",
                Icon = Icons.Material.Outlined.Dashboard
            });
        }
        
        var settingsPlugins = ExtensionManager.GetInstances<IPlugin>(true).Where(x => x?.Settings != null).ToList();
        if (settingsPlugins.Any())
        {
            var foundSettingsLinks = settingsPlugins.Select(x => x?.Settings?.BackOfficeLink).Where(x => x != null);
            foreach (var settingsLink in foundSettingsLinks)
            {
                if (settingsLink != null)
                {
                    foreach (var link in settingsLink)
                    {
                        if (NavigationSections.TryGetValue(link.Section, out var section))
                        {
                            section.Links.Add(link);
                        }
                    }
                }
            }
        }

        var backOfficeNavigationItems = ExtensionManager.GetInstances<IBackOfficeNavigationItem>(true).ToList();
        if (backOfficeNavigationItems.Any())
        {
            foreach (var navigationItem in backOfficeNavigationItems)
            {
                if (navigationItem != null)
                {
                    if (NavigationSections.TryGetValue(navigationItem.Link.Section, out var section))
                    {
                        section.Links.Add(navigationItem.Link);
                    }
                }
            }
        }
    }
}