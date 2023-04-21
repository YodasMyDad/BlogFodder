using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Plugins.Models;
using MudBlazor;

namespace BlogFodder.Plugins.ContentEditors.MarkdownEditor;

public class MarkdownEditorEditorPlugin : IEditorPlugin
{
    public string Alias => "MarkdownEditorPlugin";
    public string Name => "Markdown Editor";
    public string Description => "Plugin that uses a Markdown editor";

    public EditorPlugin? Editor { get; set; } = new ()
    {
        Component = typeof(MarkdownEditorComponent),
        Icon = Icons.Material.Filled.Edit,
        CssFiles = new List<string>
        {
            "_content/MudBlazor.Markdown/MudBlazor.Markdown.min.css"
        },
        JsFiles = new List<string>
        {
            "_content/MudBlazor.Markdown/MudBlazor.Markdown.min.js"
        },
        PreviewComponent = typeof(MarkdownEditorPreview)
    };

    public ContentPlugin Content { get; set; } = new()
    {
        Component = typeof(MarkdownContentComponent),
        CssFiles = new List<string>(),
        JsFiles = new List<string>()
    };

    public SettingsPlugin? Settings { get; set; }
}