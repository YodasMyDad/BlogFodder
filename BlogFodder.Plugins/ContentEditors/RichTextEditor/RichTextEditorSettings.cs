using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Plugins.ContentEditors.RichTextEditor;

public class RichTextEditorSettings : IEditorSettings
{
    public int PaddingTop { get; set; }
    public int PaddingBottom { get; set; }
    public int MarginTop { get; set; }
    public int MarginBottom { get; set; }
}