namespace BlogFodder.Core.Plugins.Interfaces;

/// <summary>
/// Interface for the any settings saved on the editor
/// because padding & margin are expected as values
/// </summary>
public interface IEditorSettings
{
    int PaddingTop { get; set; }
    int PaddingBottom { get; set; }
    
    int MarginTop { get; set; }
    int MarginBottom { get; set; }
}