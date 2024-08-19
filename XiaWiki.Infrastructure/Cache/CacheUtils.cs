using System;
using Microsoft.Extensions.Caching.Memory;

namespace XiaWiki.Infrastructure.Cache;

public class CacheUtils(IMemoryCache memoryCache)
{
    public void RemovePagesCache()
    {
        memoryCache.Remove(CacheKeys.PagesGetAllWithoutChildren);
        memoryCache.Remove(CacheKeys.PagesGetAll);
    }
}
