﻿using XiaWiki.Core.Models;

namespace XiaWiki.Core.Repositories;

public interface IPageDetailRepository
{
    Task<PageDetail?> GetAsync(PageId id);

    IAsyncEnumerable<PageDetail> GetAllAsync();
}
