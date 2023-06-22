using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Plugins.Models;
using MudBlazor;

namespace BlogFodder.Plugins.ContentEditors.YouTube;

public class YouTubeEditorPlugin : IEditorPlugin
{
    public string Alias => YouTubeConstants.PluginAlias;
    public string Name => YouTubeConstants.PluginName;
    public string Description => "Embed YouTube posts within your blog post";
    public string Icon => Icons.Material.Filled.VideoLibrary;

    public EditorPlugin? Editor { get; set; } = new()
    {
        Component = typeof(YouTubeEditor),
        Icon = Icons.Material.Filled.Edit,
        PreviewComponent = typeof(YouTubePreview)
    };

    public EditorContentPlugin Content { get; set; } = new()
    {
        Component = typeof(YouTubeContent)
    };
    public SettingsPlugin? Settings { get; set; }
}