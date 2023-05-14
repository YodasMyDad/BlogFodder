namespace BlogFodder.Core.Plugins.Interfaces;

/// <summary>
/// Interface for IEditorPlugin > EditorContentPlugin > Component
/// </summary>
public interface IEditorPluginContentComponent
{
    string? Model { get; set; }
    string? Settings { get; set; }
}