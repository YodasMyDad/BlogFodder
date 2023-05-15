using BlogFodder.Core.Backoffice.Models;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Plugins.Comments.Models;
using MudBlazor;

namespace BlogFodder.Plugins.Comments;

public class PostCommentsPlugin : IPlugin
{
    public string Alias => PostCommentsConstants.PluginAlias;
    public string Name => PostCommentsConstants.PluginName;
    public string Description => "Plugin that lets users comment on posts";
    public string Icon => Icons.Material.Filled.InsertComment;

    public PostPluginEditor? Editor { get; set; } = new()
    {
        Icon = Icons.Material.Filled.InsertComment,
        Component = typeof(PostCommentsEditor),
        PreviewComponent = typeof(PostCommentsEditorPreview)
    };

    public ContentPlugin Content { get; set; } = new()
    {
         Component   = typeof(PostCommentsContent),
         PluginDisplayArea = PluginDisplayArea.PostAfterContent
    };

    public SettingsPlugin? Settings { get; set; } = new()
    {
        Component = typeof(PostCommentsGlobalSettings),
        Model = new PostCommentSettings(),
        BackOfficeLink = new List<Link>
        {
            new ()
            {
                Text = "Post Comments",
                Route = "/admin/plugins/postcommentsettings"
            }
        }
    };
}