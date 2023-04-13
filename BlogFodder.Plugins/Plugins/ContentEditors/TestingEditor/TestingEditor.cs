using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Plugins.Plugins.ContentEditors.RichTextEditor;

namespace BlogFodder.Plugins.Plugins.ContentEditors.TestingEditor;

public class TestingEditor : IPlugin
{
    public string Alias => "TestingPlugin";
    public string Name => "Test Plugin";
    public string Description => "Meh";
    
    public EditorPlugin Editor { get; set; } = new()
    {
        CssFiles = new List<string>(),
        JsFiles = new List<string>(),
        Component = typeof(RichTextEditorComponent),
        Model = new RichTextEditorModel()
    };

    public ContentPlugin Content { get; set; }
    
    public SettingsPlugin Settings { get; set; }
}