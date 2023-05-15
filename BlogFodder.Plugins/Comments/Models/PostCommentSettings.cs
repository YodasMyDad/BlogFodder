using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Plugins.Comments.Models;

public class PostCommentSettings : IPluginSettings
{
    public bool ManuallyApproveComments { get; set; }
    public bool EnableAkismetSpamCheck { get; set; }
    public bool Enabled { get; set; }
}