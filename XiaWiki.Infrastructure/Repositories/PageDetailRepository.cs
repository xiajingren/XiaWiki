using Microsoft.Extensions.Options;
using XiaWiki.Core.Models;
using XiaWiki.Core.Repositories;
using XiaWiki.Infrastructure.Options;

namespace XiaWiki.Infrastructure.Repositories;

internal class PageDetailRepository(IPageRepository pageRepository, IOptionsMonitor<RuntimeOption> runtimeOptionDelegate) : IPageDetailRepository
{
    public async Task<PageDetail?> GetAsync(string id)
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

}
