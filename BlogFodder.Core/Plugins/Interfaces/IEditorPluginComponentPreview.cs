using BlogFodder.Core.Posts.Models;

namespace BlogFodder.Core.Plugins.Interfaces;

/// <summary>
/// Interface for IEditorPlugin > EditorPlugin > PreviewComponent
/// </summary>
public interface IEditorPluginComponentPreview
{
    PostContentItem? PostContentItem { get; set; }
}