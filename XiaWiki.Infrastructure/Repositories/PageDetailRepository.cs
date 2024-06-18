using Microsoft.Extensions.Options;
using XiaWiki.Core.Models;
using XiaWiki.Core.Repositories;
using XiaWiki.Infrastructure.Options;

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

        var content = await File.ReadAllTextAsync($"{option.Workspace}{page.Path}");

        return new PageDetail(page.Path, page.Title, "xiajingren", content, DateTime.Now) { ParentPage = page.ParentPage };
    }

    public async IAsyncEnumerable<PageDetail> GetLatestUpdatesAsync(int count)
    {
        var pageIds = pageRepository.GetAllWithoutChildren()
                                    .Values
                                    .Where(x => !x.IsFolder)
                                    .OrderByDescending(x => x.UpdatedTime)
                                    .Take(count)
                                    .Select(x => x.Id);

        foreach (var pageId in pageIds)
        {
            var page = await GetAsync(pageId);

            if (page is not null)
                yield return page;
        }
    }

    public async IAsyncEnumerable<PageDetail> GetRandomListAsync(int count)
    {
        var pageIds = pageRepository.GetAllWithoutChildren()
                                    .Values.Where(x => !x.IsFolder)
                                    .OrderByDescending(x => Guid.NewGuid())
                                    .Take(count)
                                    .Select(x => x.Id);

        foreach (var pageId in pageIds)
        {
            var page = await GetAsync(pageId);

            if (page is not null)
                yield return page;
        }
    }

}
