using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Plugins.Models;
using MudBlazor;

namespace BlogFodder.Plugins.ContentEditors.MarkdownEditor;

public class MarkdownEditorPlugin : IEditorPlugin
{
    public string Alias => "MarkdownEditorPlugin";
    public string Name => "Markdown Editor";
    public string Description => "Plugin that uses a Markdown editor";
    public string Icon => Icons.Material.Filled.ModeEdit;

    public EditorPlugin? Editor { get; set; } = new ()
    {
        Component = typeof(MarkdownEditorComponent),
        Icon = Icons.Material.Filled.Edit,
        CssFiles = new List<string>
        {
            "https://unpkg.com/easymde/dist/easymde.min.css"
        },
        JsFiles = new List<string>
        {
            "https://unpkg.com/easymde/dist/easymde.min.js",
            "_content/BlogFodder.Plugins/js/markdowneditor.js"
        },
        PreviewComponent = typeof(MarkdownEditorPreview)
    };

    public EditorContentPlugin Content { get; set; } = new()
    {
        Component = typeof(MarkdownContentComponent)
    };

    public SettingsPlugin? Settings { get; set; }
}