using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Plugins.ContentEditors.RichTextEditor;

public class RichTextGlobalSettings : IPluginSettings
{
    public string? ApiKey { get; set; }

    public Dictionary<string, object> DefaultEditorSettings { get; set; } = new()
    {
        {"height", 600},
        {
            "toolbar",
            "undo redo | a11ycheck casechange blocks | bold italic backcolor | alignleft aligncenter alignright alignjustify | bullist numlist checklist outdent indent | removeformat | advcode table help"
        },
        {
            "plugins",
            "advlist autolink lists link image charmap preview anchor searchreplace visualblocks code fullscreen insertdatetime media table code help wordcount"
        }
    };
}