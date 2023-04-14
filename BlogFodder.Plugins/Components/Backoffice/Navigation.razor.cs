using BlogFodder.Core.Backoffice.Models;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BlogFodder.Plugins.Components.Backoffice;

public partial class Navigation : ComponentBase
{
    [Inject] public ExtensionManager ExtensionManager { get; set; } = default!;

    public List<Link> PluginSettings { get; set; } = new();
    protected override void OnInitialized()
    {
        var settingsPlugins = ExtensionManager.GetInstances<IPlugin>(true).Where(x => x?.Settings != null).ToList();
        if (settingsPlugins.Any())
        {
            var foundSettingsLinks = settingsPlugins.Select(x => x?.Settings?.BackOfficeLink).Where(x => x != null);
            PluginSettings.AddRange(foundSettingsLinks.Where(x => !x.Route.IsNullOrWhiteSpace()));
        }
    }
}