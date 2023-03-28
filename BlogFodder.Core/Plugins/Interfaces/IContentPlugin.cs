using BlogFodder.Core.Plugins.Models;

namespace BlogFodder.Core.Plugins.Interfaces;

public interface IContentPlugin
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
    /// The plugin for the editor
    /// </summary>
    EditorPlugin Editor { get; set; }

    /// <summary>
    /// The plugin for the content
    /// </summary>
    ContentPlugin Content { get; set; }
    
    /// <summary>
    /// The plugin for the settings
    /// </summary>
    SettingsPlugin Settings { get; set; }
}