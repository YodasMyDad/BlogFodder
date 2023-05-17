namespace BlogFodder.Core.Extensions;

/// <summary>
/// Class to hold the cache keys used around the site
/// </summary>
public static class CacheExtensions
{
    public const int MemoryCacheInMinutes = 240;

    public static string ToCacheKey(this Type item, string identifier)
    {
        return $"{nameof(item)}-{identifier}";
    }
    
    public static string ToCacheKey(this Type item, List<string> identifier)
    {
        // need to order the items and CSV them to keep them consistent
        var cacheKey = string.Join("-", identifier.OrderBy(x => x));
        return $"{nameof(item)}-{cacheKey}";
    }
}
