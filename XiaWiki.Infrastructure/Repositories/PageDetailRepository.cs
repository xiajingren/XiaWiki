﻿using Microsoft.Extensions.Options;
using XiaWiki.Core.Models;
using XiaWiki.Core.Repositories;
using XiaWiki.Infrastructure.Options;
using XiaWiki.Infrastructure.Search;

namespace XiaWiki.Infrastructure.Repositories;

internal class PageDetailRepository(IPageRepository pageRepository, IOptionsMonitor<RuntimeOption> runtimeOptionDelegate) : IPageDetailRepository
{
    public async Task<PageDetail?> GetAsync(PageId id)
    {
        var page = pageRepository.GetPageById(id);

        if (page is null)
            return null;

        if (page.IsFolder)
            throw new ApplicationException($"{id} is a folder..."); // todo: domain exception

        var option = runtimeOptionDelegate.CurrentValue;

        var content = await File.ReadAllTextAsync($"{option.PagesDir}{page.Path}");

        return new PageDetail(page.Path, page.Title, "xiajingren", content, DateTime.Now) { Parent = page.Parent };
    }

    public async IAsyncEnumerable<PageDetail> GetAllAsync()
    {
        var pageIds = pageRepository.GetAllWithoutChildren().Values.Where(x => !x.IsFolder).Select(x => x.Id);

        foreach (var pageId in pageIds)
        {
            var page = await GetAsync(pageId);

            if (page is not null)
                yield return page;
        }
    }
}
