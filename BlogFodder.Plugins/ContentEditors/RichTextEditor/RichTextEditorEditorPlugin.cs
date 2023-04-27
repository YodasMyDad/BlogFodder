using BlogFodder.Core.Backoffice.Models;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Plugins.Models;
using MudBlazor;

namespace BlogFodder.Plugins.ContentEditors.RichTextEditor;

public class RichTextEditorEditorPlugin : IEditorPlugin
{
    public string Alias => "RichTextEditorPlugin";
    public string Name => "Rich Text Editor";
    public string Description => "Plugin that uses the TinyMCE editor";

    // TODO - Icon?
    // TODO - What to pull through in block list? I.e. Short description etc...
    
    public EditorPlugin? Editor { get; set; } = new()
    {
        CssFiles = new List<string>
        {
            "_content/BlogFodder.Plugins/css/editorstyles.css"
        },
        JsFiles = new List<string>
        {
            "_content/TinyMCE.Blazor/tinymce-blazor.js"
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

    public SettingsPlugin? Settings { get; set; } = new()
    {
        CssFiles = new List<string>(),
        JsFiles = new List<string>(),
        Component = typeof(RichTextSettingsComponent),
        Model = new RichTextGlobalSettings
        {
            DefaultEditorSettings = new Dictionary<string, object>
            {
                // Need to get these from somewhere really? Instead of being hardcoded
                {"height", 600},
                {"toolbar", "undo redo | a11ycheck casechange blocks | bold italic backcolor | alignleft aligncenter alignright alignjustify | bullist numlist checklist outdent indent | removeformat | advcode table help"},
                {"plugins", "advlist autolink lists link image charmap preview anchor searchreplace visualblocks code fullscreen insertdatetime media table code help wordcount"}
            }
        },
        BackOfficeLink = new List<Link>
        {
            new()
            {
                Route = "/admin/richtexteditorsettings",
                Text = "Rich Text Editor"
            }
        }
    };
}