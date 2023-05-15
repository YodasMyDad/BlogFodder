using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Plugins.Models;
using MudBlazor;

namespace BlogFodder.Plugins.Comments;

public class PostCommentsPlugin : IPlugin
{
    public string Alias => "PostComments";
    public string Name => "Post Comments";
    public string Description => "Plugin that lets users comment on posts";
    public string Icon => Icons.Material.Filled.InsertComment;

    public PostPluginEditor? Editor { get; set; } = new()
    {
        Icon = Icons.Material.Filled.InsertComment,
        Component = typeof(CommentsEditor),
        PreviewComponent = typeof(CommentsEditorPreview)
    };
    
    public ContentPlugin Content { get; set; }
    public SettingsPlugin? Settings { get; set; }
}