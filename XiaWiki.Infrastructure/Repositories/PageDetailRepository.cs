using Microsoft.Extensions.Options;
using XiaWiki.Core.Models;
using XiaWiki.Core.Repositories;
using XiaWiki.Infrastructure.Options;

namespace XiaWiki.Infrastructure.Repositories;

internal class PageDetailRepository(IPageRepository pageRepository, IOptionsMonitor<RuntimeOption> runtimeOptionDelegate) : IPageDetailRepository
{
    //private readonly IPageRepository _pageRepository = pageRepository;

    public async Task<PageDetail?> GetAsync(string id)
    {
        var page = pageRepository.GetPageById(id);

        if (page == null)
            return null;

        if (page.IsFolder)
            throw new ApplicationException($"{id} is a folder...");

        var option = runtimeOptionDelegate.CurrentValue;

        var text = await File.ReadAllTextAsync($"{option.Workspace}{page.Path}");

        var content = Markdig.Markdown.ToHtml(text);

        return new PageDetail(page.Path, page.Title, "xiajingren", content, DateTime.Now);
    }
}
