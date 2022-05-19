namespace ProjectB.Infrastructure;

public interface ICacheFilter<T>
{
    T Get(string cacheKey);

    T Set(string cacheKey, T item);
}