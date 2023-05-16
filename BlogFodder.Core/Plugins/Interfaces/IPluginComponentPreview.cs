using BlogFodder.Core.Plugins.Models;

namespace BlogFodder.Core.Plugins.Interfaces;

/// <summary>
/// Interface for IPlugin > PostPluginEditor > PreviewComponent
/// </summary>
public interface IPluginComponentPreview
{
    Plugin? Plugin { get; set; }
}