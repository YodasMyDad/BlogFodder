using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Posts.Models;
using MailKit.Search;

namespace BlogFodder.Core.Extensions;

public static class Urls
{
    public static string? Create(string? slug, Type type)
    {
        return type switch
        {
            _ when type == typeof(Post) => $"/p/{slug?.ToLower()}",
            _ when type == typeof(Category) => $"/c/{slug?.ToLower()}",
            _ when type == typeof(SearchResults) => $"/s/{slug?.ToLower().Replace(" ", "+")}",
            _ => slug
        };
    }
}