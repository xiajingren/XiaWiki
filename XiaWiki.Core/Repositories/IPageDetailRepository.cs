using XiaWiki.Core.Models;

namespace XiaWiki.Core.Repositories;

public interface IPageDetailRepository
{
    Task<PageDetail?> GetAsync(PageId id);

    IAsyncEnumerable<PageDetail> GetLatestUpdatesAsync(int count);

    IAsyncEnumerable<PageDetail> GetRandomListAsync(int count);

    IAsyncEnumerable<PageDetail> GetAllAsync();

    IAsyncEnumerable<PageDetail> SearchAsync(string keyword);
}
