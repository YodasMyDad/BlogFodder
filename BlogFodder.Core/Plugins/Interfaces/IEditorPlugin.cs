using BlogFodder.Core.Plugins.Models;

namespace BlogFodder.Core.Plugins.Interfaces;

public interface IEditorPlugin
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
    /// The plugin for the editor that the user will use to edit and save data
    /// </summary>
    EditorPlugin? Editor { get; set; }

    /// <summary>
    /// The plugin to display content of the editor on the front end
    /// </summary>
    EditorContentPlugin Content { get; set; }
    
    /// <summary>
    /// The plugin for the global settings, so use this if you have configurable settings needed for each instance of the editor
    /// </summary>
    SettingsPlugin? Settings { get; set; }
}