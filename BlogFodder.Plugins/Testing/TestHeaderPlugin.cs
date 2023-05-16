using BlogFodder.Core.Backoffice.Models;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Plugins.Models;

namespace BlogFodder.Plugins.Testing;

public class TestHeaderPlugin : IPlugin
{
    public string Alias => nameof(TestHeaderPlugin);
    public string Name => "My Test Plugin";
    public string Description => "Just testing to figure out plugin structure";
    public string Icon => "";
    public PostPluginEditor? Editor { get; set; }

    public ContentPlugin? Content { get; set; } = new()
    {
        Component = typeof(MyTestPlugin),
        PluginDisplayArea = PluginDisplayArea.HeaderBottom
    };

    public SettingsPlugin Settings { get; set; } = new()
    {
        Component = typeof(MyTestPluginSettings),
        BackOfficeLink = new List<Link>
        {
            new ()
            {
                Text = "My Test Plugin",
                Route = "/admin/mytestplugin"
            }
        }
    };
}