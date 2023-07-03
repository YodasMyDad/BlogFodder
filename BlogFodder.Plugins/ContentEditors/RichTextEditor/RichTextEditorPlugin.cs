using BlogFodder.Core.Backoffice.Models;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Plugins.Models;
using MudBlazor;

namespace BlogFodder.Plugins.ContentEditors.RichTextEditor;

public class RichTextEditorPlugin : IEditorPlugin
{
    public string Alias => RichTextEditorConstants.PluginAlias;
    public string Name => RichTextEditorConstants.PluginName;
    public string Description => "Plugin that uses the TinyMCE editor";
    public string Icon => Icons.Material.Filled.EditNote;

    public EditorPlugin? Editor { get; set; } = new()
    {
        CssFiles = new List<string>
        {
            "_content/BlogFodder.Plugins/css/editorstyles.css",
            "https://cdn.quilljs.com/1.3.6/quill.snow.css"
        },
        JsFiles = new List<string>
        {
            //"_content/TinyMCE.Blazor/tinymce-blazor.js"
            "https://cdn.quilljs.com/1.3.6/quill.js",
            "_content/BlogFodder.Plugins/js/quill-blot-formatter.js",
            "_content/BlogFodder.Plugins/js/quill.js"
        },
        Component = typeof(RichTextEditorComponent),
        PreviewComponent = typeof(RichTextEditorPreview),
        Icon = Icons.Material.Filled.AlignHorizontalLeft
    };

    public EditorContentPlugin Content { get; set; } = new()
    {
        CssFiles = new List<string>        {
            "_content/BlogFodder.Plugins/css/styles.css"
        },
        JsFiles = new List<string>(),
        Component = typeof(RichTextContentComponent)
    };

    public SettingsPlugin? Settings { get; set; }
        /*= new()
    {
        CssFiles = new List<string>(),
        JsFiles = new List<string>(),
        Component = typeof(RichTextSettingsComponent),
        Model = new RichTextGlobalSettings(),
        BackOfficeLink = new List<Link>
        {
            new()
            {
                Route = "/admin/richtexteditorsettings",
                Text = "Rich Text Editor"
            }
        }
    };*/
}