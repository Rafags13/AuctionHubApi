using AuctionHub.Domain.Interfaces.Services.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace AuctionHub.Infrastructure.Services.Caching
{
    public sealed class CachingService(
        IMemoryCache memoryCache
    ) : ICachingService
    {
        public T? Get<T>(string key)
        {
            if (memoryCache.TryGetValue(key, out T value))
                return value;

            return default;
        }

        public void Remove(string key)
        {
            memoryCache.Remove(key);
        }

        public void Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new MemoryCacheEntryOptions();
            if (expiration.HasValue)
                options.SetAbsoluteExpiration(expiration.Value);

            memoryCache.Set(key, value, options);
        }
    }
}
