namespace BlogFodder.Core.Plugins.Interfaces;

public interface IEditorPluginContentComponent
{
    string? Model { get; set; }
    string? Settings { get; set; }
}