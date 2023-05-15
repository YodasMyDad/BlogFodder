using BlogFodder.Core.Posts.Models;

namespace BlogFodder.Core.Plugins.Interfaces;

/// <summary>
/// Interface for IPlugin > PostPluginEditor > PreviewComponent
/// </summary>
public interface IPluginComponentPreview
{
    PostPlugin? PostPlugin { get; set; }
}