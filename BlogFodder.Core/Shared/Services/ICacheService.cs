namespace BlogFodder.Core.Shared.Services;

public interface ICacheService
{
    Task<T?> GetSetCachedItemAsync<T>(string cacheKey, Func<Task<T>> getCacheItemAsync, int cacheTimeInMinutes = Extensions.CacheExtensions.MemoryCacheInMinutes);
    T? GetSetCachedItem<T>(string cacheKey, Func<T> getCacheItem, int cacheTimeInMinutes = Extensions.CacheExtensions.MemoryCacheInMinutes);
    void ClearCachedItem(string cacheKey);
}