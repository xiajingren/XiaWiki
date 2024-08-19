using System;

namespace XiaWiki.Infrastructure.Cache;

public static class CacheKeys
{
    public static readonly string PagesGetAll = "Pages-GetAll";

    public static readonly string PagesGetAllWithoutChildren = "Pages-GetAllWithoutChildren";
}
