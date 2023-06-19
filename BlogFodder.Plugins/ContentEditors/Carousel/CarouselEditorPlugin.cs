using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Plugins.Models;
using MudBlazor;

namespace BlogFodder.Plugins.ContentEditors.Carousel;

public class CarouselEditorPlugin : IEditorPlugin
{
    public string Alias => CarouselEditorConstants.PluginAlias;
    public string Name => CarouselEditorConstants.PluginName;
    public string Description => "Carousel plugin, allows you to upload images with optional text and are displayed in a sliding/swipable carousel";
    public string Icon => Icons.Material.Filled.ViewCarousel;
    public EditorPlugin? Editor { get; set; } = new ()
    {
        Component = typeof(CarouselEditor),
        Icon = Icons.Material.Filled.Edit,
        PreviewComponent = typeof(CarouselPreview)
    };
    public EditorContentPlugin Content { get; set; } = new()
    {
        Component = typeof(CarouselContent)
    };
    public SettingsPlugin? Settings { get; set; }
}