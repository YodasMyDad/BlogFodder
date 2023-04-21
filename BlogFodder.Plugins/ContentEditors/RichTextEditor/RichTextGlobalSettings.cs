using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Plugins.ContentEditors.RichTextEditor;

public class RichTextGlobalSettings : IPluginSettings
{
    public Dictionary<string, object> DefaultEditorSettings { get; set; } = new();
}