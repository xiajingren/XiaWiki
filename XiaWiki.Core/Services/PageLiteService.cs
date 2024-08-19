using XiaWiki.Core.Models;
using XiaWiki.Core.Repositories;

namespace XiaWiki.Core.Services;

public class PageLiteService(IPageRepository pageRepository,
                             IPageDetailRepository pageDetailRepository,
                             IRendererService rendererService,
                             ISearchService searchService) : IPageLiteService
{
    public async IAsyncEnumerable<PageLite> GetLatestUpdatesAsync(int count)
    {
        var pageIds = pageRepository.GetAllWithoutChildren()
                                    .Values.Where(x => !x.IsFolder)
                                    .OrderByDescending(x => x.UpdatedTime)
                                    .Take(count)
                                    .Select(x => x.Id);

        foreach (var pageId in pageIds)
        {
            var pageLite = await GetPageLite(pageId);

            if (pageLite is not null)
                yield return pageLite;
        }
    }

    public async IAsyncEnumerable<PageLite> GetRandomListAsync(int count)
    {
        var pageIds = pageRepository.GetAllWithoutChildren()
                                    .Values.Where(x => !x.IsFolder)
                                    .OrderByDescending(x => Guid.NewGuid())
                                    .Take(count)
                                    .Select(x => x.Id);

        foreach (var pageId in pageIds)
        {
            var pageLite = await GetPageLite(pageId);

            if (pageLite is not null)
                yield return pageLite;
        }
    }

    private async Task<PageLite?> GetPageLite(PageId pageId)
    {
        var pageDetail = await pageDetailRepository.GetAsync(pageId);

        if (pageDetail is null)
            return null;

        var summary = rendererService.GetSummary(pageDetail.Content, 100);
        var images = rendererService.GetImages(pageDetail.Id.ToString(), pageDetail.Content);

        return new PageLite(pageDetail.Id, pageDetail.Title, pageDetail.Author, summary, images, pageDetail.UpdatedTime) { Parent = pageDetail.Parent };
    }

    public async IAsyncEnumerable<PageLite> SearchAsync(string keyword)
    {
        var searchResult = searchService.Search(keyword, new Dictionary<string, float>() {
            { nameof(PageDetail.Title), 5 },
            { nameof(PageDetail.Content), 10 }
        });

        foreach (var item in searchResult)
        {
            var id = item.GetValueOrDefault(nameof(PageDetail.Id));

            if (id is null)
                continue;

            var temp = await GetPageLite(PageId.Parse(id));
            if (temp is null)
                continue;

            var title = item.GetValueOrDefault(nameof(PageDetail.Title));
            var content = item.GetValueOrDefault(nameof(PageDetail.Content));

            yield return new PageLite(temp.Id, title ?? temp.Title, temp.Author, content ?? temp.Summary, temp.Images, temp.UpdatedTime) { Parent = temp.Parent };
        }
    }
}
