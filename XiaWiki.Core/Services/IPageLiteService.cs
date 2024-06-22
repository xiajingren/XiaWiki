using XiaWiki.Core.Models;

namespace XiaWiki.Core.Services;

public interface IPageLiteService
{
    IAsyncEnumerable<PageLite> GetLatestUpdatesAsync(int count);

    IAsyncEnumerable<PageLite> GetRandomListAsync(int count);

    IAsyncEnumerable<PageLite> SearchAsync(string keyword);
}
