﻿using BlogFodder.Core.Plugins.Models;

namespace BlogFodder.Core.Plugins.Interfaces;

/// <summary>
/// The interface which allows you to create an editor for a blog post, also a preview in the post editor
/// and a component to display the content on the front end of the site
/// </summary>
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
    /// Icon which displays when they are selecting the editor and also in preview list
    /// </summary>
    string Icon { get; }

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