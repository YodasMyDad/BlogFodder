using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Plugins.ContentEditors.RichTextEditor;

public class RichTextGlobalSettings : IPluginSettings
{
    //public string? ApiKey { get; set; }

    // height
    public int Height { get; set; } = 600;

    // toolbar
    public string? ToolBar { get; set; } =
        "undo redo | a11ycheck casechange blocks | bold italic backcolor | alignleft aligncenter alignright alignjustify | bullist numlist checklist outdent indent | removeformat | advcode table help";

    // plugins
    public string? Plugins { get; set; } =
        "advlist autolink lists link image charmap preview anchor searchreplace visualblocks code fullscreen insertdatetime media table code help wordcount";

    public bool Enabled { get; set; } = true;
}