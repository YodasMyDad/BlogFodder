using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Plugins.Comments.Models;

public class PostCommentSettings : IPluginSettings
{
    public bool ManuallyApproveComments { get; set; }
    public bool EnableAkismetSpamCheck { get; set; }
    public bool Enabled { get; set; }

    public List<PostComment> FlaggedComments { get; set; } = new();
    public List<PostComment> CommentsToApprove { get; set; } = new();
}