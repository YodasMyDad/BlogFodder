using BlogFodder.Core.Plugins.Models;

namespace BlogFodder.Core.Plugins.Interfaces;

public interface IPlugin
{
    /// <summary>
    /// Alias is important, do not change the alias of your plugin when updating it
    /// </summary>
    string Alias { get; }
    
    /// <summary>
    /// Name of the plugin, appears everywhere in website, including navigation
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Short description of what your plugin is and does
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Icon which displays when they are selecting the editor and also in preview list
    /// </summary>
    string Icon { get; }

    /// <summary>
    /// The optional plugin for the editor that the user will use to edit/view/save data on a post
    /// </summary>
    PostPluginEditor? Editor { get; set; }

    /// <summary>
    /// The plugin to display content of the plugin on the front end
    /// </summary>
    ContentPlugin Content { get; set; }
    
    /// <summary>
    /// The optional plugin for the global settings, so use this if you have configurable settings needed for each instance of the editor
    /// </summary>
    SettingsPlugin? Settings { get; set; }
}