using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Plugins.Models;

namespace BlogFodder.Plugins.Comments;

public class PostCommentsPlugin : IPlugin
{
    public string Alias => "";
    public string Name => "";
    public string Description => "";
    public string Icon => "";
    public PostPluginEditor? Editor { get; set; }
    public ContentPlugin Content { get; set; }
    public SettingsPlugin? Settings { get; set; }
}