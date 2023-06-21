using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Plugins.ContentEditors.Carousel;

public class CarouselEditorSettings : IEditorSettings
{
    public int ImageHeight { get; set; } = 450;
    public int ImageWidth { get; set; } = 900;
    
    public int PaddingTop { get; set; }
    public int PaddingBottom { get; set; }
    public int MarginTop { get; set; }
    public int MarginBottom { get; set; }
}