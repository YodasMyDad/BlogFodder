using BlogFodder.Core.Posts.Models;
using Microsoft.AspNetCore.Components;

namespace BlogFodder.Core.Plugins.Interfaces;

public interface IEditorPluginComponent
{
    PostContentItem PostContentItem { get; set; }
    EventCallback<PostContentItem> SaveAndClose { get; set; }
    Task Submit();
}