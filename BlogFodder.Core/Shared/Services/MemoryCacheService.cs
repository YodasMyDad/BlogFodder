using Microsoft.Extensions.Caching.Memory;

namespace BlogFodder.Core.Shared.Services;


public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }
    
    public async Task<T?> GetSetCachedItemAsync<T>(string cacheKey, Func<Task<T>> getCacheItemAsync, int cacheTimeInMinutes = Extensions.CacheExtensions.MemoryCacheInMinutes)
    {
        // Look for cache key.
        if (!_cache.TryGetValue(cacheKey, out T? cacheEntry))
        {
            // Key not in cache, so get data.
            cacheEntry = await getCacheItemAsync();

            // Set cache options.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromMinutes(cacheTimeInMinutes));

            // Save data in cache.
            _cache.Set(cacheKey, cacheEntry, cacheEntryOptions);
        }

        return cacheEntry;
    }
    
    public T? GetSetCachedItem<T>(string cacheKey, Func<T> getCacheItem, int cacheTimeInMinutes = Extensions.CacheExtensions.MemoryCacheInMinutes)
    {
        // Look for cache key.
        if (!_cache.TryGetValue(cacheKey, out T? cacheEntry))
        {
            // Key not in cache, so get data.
            cacheEntry = getCacheItem();

            // Set cache options.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromMinutes(cacheTimeInMinutes));

            // Save data in cache.
            _cache.Set(cacheKey, cacheEntry, cacheEntryOptions);
        }

        return cacheEntry;
    }
    
    public void ClearCachedItem(string cacheKey)
    {
        _cache.Remove(cacheKey);
    }
}