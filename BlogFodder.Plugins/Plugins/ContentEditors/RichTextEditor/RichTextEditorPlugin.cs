using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Plugins.Models;

namespace BlogFodder.Plugins.Plugins.ContentEditors.RichTextEditor;

public class RichTextEditorPlugin : IContentPlugin
{
    public string Alias => "RichTextEditorPlugin";
    public string Name => "Rich Text Editor";
    public string Description => "Plugin that uses the TinyMCE editor";

    public EditorPlugin Editor { get; set; } = new()
    {
        CssFiles = new List<string>
        {
            "_content/BlogFodder.Plugins/css/editorstyles.css"
        },
        JsFiles = new List<string>
        {
            "_content/TinyMCE.Blazor/tinymce-blazor.js",
            "_content/BlogFodder.Plugins/js/scripts.js"
        },
        Component = typeof(RichTextEditorComponent),
        Model = new RichTextEditorModel()
    };

    public ContentPlugin Content { get; set; } = new()
    {
        CssFiles = new List<string>        {
            "_content/BlogFodder.Plugins/css/styles.css"
        },
        JsFiles = new List<string>(),
        Component = typeof(RichTextContentComponent),
        Model = new RichTextContentModel()
    };

    public SettingsPlugin Settings { get; set; } = new()
    {
        CssFiles = new List<string>(),
        JsFiles = new List<string>(),
        Component = typeof(RichTextSettingsComponent),
        Model = new RichTextGlobalSettings()
    };
}