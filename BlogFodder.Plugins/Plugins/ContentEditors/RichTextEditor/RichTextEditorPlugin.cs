using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Plugins.Plugins.ContentEditors.RichTextEditor;

public class RichTextEditorPlugin : IContentPlugin
{
    public string Alias => "RichTextEditorPlugin";
    public string Name => "Rich Text Editor";
    public string Description => "Plugin that uses the TinyMCE editor";
    public Type PluginEditorComponent { get; set; } = typeof(RichTextEditorComponent);
    public IPluginSettings PluginEditorModel { get; set; } = new RichTextEditorModel(); // Try these are dictionaries
    public Type PluginContentComponent { get; set; } = typeof(RichTextContentComponent);
    public IPluginSettings PluginContentModel { get; set; } = new RichTextContentModel();
    public Type? PluginGlobalSettingsEditorComponent { get; set; } = typeof(RichTextSettingsComponent);
    public IPluginSettings? PluginGlobalSettingsModel { get; set; } = new RichTextGlobalSettings();
}