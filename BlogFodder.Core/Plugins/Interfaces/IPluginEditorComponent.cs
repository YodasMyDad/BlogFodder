using BlogFodder.Core.Plugins.Models;
using Microsoft.AspNetCore.Components;

namespace BlogFodder.Core.Plugins.Interfaces;

/// <summary>
/// Interface for IPlugin > PostPluginEditor > Component
/// </summary>

public interface IPluginEditorComponent
{
    Plugin Plugin { get; set; }
    
    Guid PostId { get; set; }
    
    EventCallback<Plugin> SaveAndClose { get; set; }
}