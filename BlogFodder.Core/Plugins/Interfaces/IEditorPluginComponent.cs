using BlogFodder.Core.Posts.Models;
using Microsoft.AspNetCore.Components;

namespace BlogFodder.Core.Plugins.Interfaces;

/// <summary>
/// Interface for IEditorPlugin > EditorPlugin > Component
/// </summary>
public interface IEditorPluginComponent
{
    PostContentItem PostContentItem { get; set; }
    EventCallback<PostContentItem> SaveAndClose { get; set; }
}