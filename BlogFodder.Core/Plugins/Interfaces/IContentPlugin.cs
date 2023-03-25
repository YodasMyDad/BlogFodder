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
    /// The editor view the user will interact with to add/save content to the DB
    /// </summary>
    Type  PluginEditorComponent { set; }
    
    /// <summary>
    /// The data/settings for the editor that are serialised and saved to the DB
    /// This is settings per editor block, for things like API keys use the PluginGlobalSettings
    /// </summary>
    IPluginSettings PluginEditorModel { get; set; }
    
    /// <summary>
    /// This is the content view that is rendered on the public facing site
    /// </summary>
    Type  PluginContentComponent { get; set; }
    
    /// <summary>
    /// This is the model that is serialised and saved to the DB and passed into the PluginContentComponent
    /// </summary>
    IPluginSettings PluginContentModel { get; set; }
    
    /// <summary>
    /// Optional: If your plugin requires globals settings that are used for all instances of your editor
    /// then use this view to display the settings editor, appears in the admin/plugins navigation
    /// </summary>
    Type?  PluginGlobalSettingsEditorComponent { get; set; }
    
    /// <summary>
    /// Optional: This is the model that is used to saved the global settings of your plugin  
    /// </summary>
    IPluginSettings? PluginGlobalSettingsModel { get; set; }
}