using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Posts.Models;

namespace BlogFodder.Core.Extensions;

public static class Urls
{
    public static string? Create(string? slug, Type type)
    {
        return type switch
        {
            _ when type == typeof(Post) => $"/p/{slug}",
            _ when type == typeof(Category) => $"/c/{slug}",
            _ => slug
        };
    }
}