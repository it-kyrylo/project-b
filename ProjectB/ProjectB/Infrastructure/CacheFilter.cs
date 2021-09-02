using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.Infrastructure
{
    public class CacheFilter
    {
        private IMemoryCache memoryCache;
        public CacheFilter(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }
        public object GetCache(string cacheKey)
        {
            object value;
            memoryCache.TryGetValue(cacheKey, out value);
            return value;
        }
        public Task SetCache(string cacheKey, object value)
        {
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(2),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromHours(1),
                Size = 64
            };
            memoryCache.Set(cacheKey, value, cacheExpiryOptions);
            return Task.CompletedTask;
        }
        public Task SetCache(string cacheKey, object value, int absoluteExpiration, int slidingExpiration)
        {
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(absoluteExpiration),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromHours(slidingExpiration),
                Size = 32
            };
            memoryCache.Set(cacheKey, value, cacheExpiryOptions);
            return Task.CompletedTask;
        }
    }
}
