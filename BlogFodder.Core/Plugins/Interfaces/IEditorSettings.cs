namespace BlogFodder.Core.Plugins.Interfaces;

public interface IEditorSettings
{
    int PaddingTop { get; set; }
    int PaddingBottom { get; set; }
    
    int MarginTop { get; set; }
    int MarginBottom { get; set; }
}