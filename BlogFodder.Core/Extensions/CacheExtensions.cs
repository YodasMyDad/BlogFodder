namespace BlogFodder.Core.Extensions;

/// <summary>
/// Class to hold the cache keys used around the site
/// </summary>
public static class CacheExtensions
{
    public const int MemoryCacheInMinutes = 60;

    public static string ToCacheKey(this Type item, string? identifier)
    {
        return $"{nameof(item)}-{identifier}";
    }
}
