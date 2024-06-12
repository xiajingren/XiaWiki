using Markdig;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
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

        if (page == null)
            return null;

        if (page.IsFolder)
            throw new ApplicationException($"{id} is a folder...");

        var option = runtimeOptionDelegate.CurrentValue;

        var text = await File.ReadAllTextAsync($"{option.Workspace}{page.Path}");

        var content = ConvertMarkdownToHtml(id, text);

        return new PageDetail(page.Path, page.Title, "xiajingren", content, DateTime.Now);
    }

    private static string ConvertMarkdownToHtml(string id, string markdown)
    {
        var document = Markdown.Parse(markdown);

        foreach (var node in document)
        {
            if (node is not ParagraphBlock { Inline: { } } paragraphBlock) continue;

            foreach (var inline in paragraphBlock.Inline)
            {
                if (inline is not LinkInline { IsImage: true } linkInline) continue;

                if (linkInline.Url == null) continue;

                linkInline.Url = $"/media/{id}/{System.Net.WebUtility.UrlEncode(linkInline.Url)}";
            }
        }

        using var writer = new StringWriter();
        var render = new NormalizeRenderer(writer);
        render.Render(document);

        return Markdown.ToHtml(writer.ToString());
    }

}
