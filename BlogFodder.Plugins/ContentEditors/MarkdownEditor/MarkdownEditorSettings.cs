using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Plugins.ContentEditors.MarkdownEditor;

public class MarkdownEditorSettings : IEditorSettings
{
    public int PaddingTop { get; set; }
    public int PaddingBottom { get; set; }
    public int MarginTop { get; set; }
    public int MarginBottom { get; set; }
}