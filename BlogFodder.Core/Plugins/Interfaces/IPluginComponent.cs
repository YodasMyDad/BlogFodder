using BlogFodder.Core.Posts.Models;
using Microsoft.AspNetCore.Components;

namespace BlogFodder.Core.Plugins.Interfaces;

/// <summary>
/// Interface for IPlugin > PostPluginEditor > Component
/// </summary>

public interface IPluginComponent
{
    PostPlugin PostPlugin { get; set; }
    EventCallback<PostPlugin> SaveAndClose { get; set; }
}