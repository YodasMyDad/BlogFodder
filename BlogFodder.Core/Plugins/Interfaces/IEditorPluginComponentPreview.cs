using BlogFodder.Core.Posts.Models;

namespace BlogFodder.Core.Plugins.Interfaces;

public interface IEditorPluginComponentPreview
{
    PostContentItem? PostContentItem { get; set; }
}