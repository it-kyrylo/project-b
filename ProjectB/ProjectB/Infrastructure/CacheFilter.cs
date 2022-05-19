using Microsoft.Extensions.Caching.Memory;

namespace ProjectB.Infrastructure
{
    public class CacheFilter<T> : ICacheFilter<T> where T : class
    {
        private readonly IMemoryCache _memoryCache;

        public CacheFilter(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get(string cacheKey)
        {
            return _memoryCache.Get<T>(cacheKey);
        }

        public T Set(string cacheKey, T item)
        {
            return _memoryCache.Set(cacheKey, item, TimeSpan.FromHours(1));
        }
    }
}
