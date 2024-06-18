using System.Text;
using Markdig;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using XiaWiki.Core.Models;
using XiaWiki.Core.Repositories;
using XiaWiki.Core.Services;
using XiaWiki.Infrastructure.Options;

namespace XiaWiki.Infrastructure.Services;

internal class RendererService : IRendererService
{
    // private static readonly MarkdownPipeline markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
    private static readonly MarkdownPipeline markdownPipeline = new MarkdownPipelineBuilder()
                                                                    //.UseAutoIdentifiers(Markdig.Extensions.AutoIdentifiers.AutoIdentifierOptions.GitHub)
                                                                    .UseAdvancedExtensions()
                                                                    .Build();

    public string ToHtml(string id, string content)
    {
        var document = Markdown.Parse(content, markdownPipeline);

        foreach (var linkInline in document.Descendants<LinkInline>())
        {
            if (!linkInline.IsImage)
                continue;

            if (string.IsNullOrEmpty(linkInline.Url))
                continue;

            linkInline.Url = $"/media/{id}/{System.Net.WebUtility.UrlEncode(linkInline.Url)}";
        }

        return document.ToHtml(markdownPipeline);
    }

    public string ToPlainText(string content)
    {
        return Markdown.ToPlainText(content);
    }

    public string GetOutline(string content)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<ul class=\"list-unstyled\">");

        var document = Markdown.Parse(content, markdownPipeline);

        foreach (var heading in document.Descendants<HeadingBlock>())
        {
            if (heading.Inline?.FirstChild is not LiteralInline inline)
                continue;

            sb.AppendLine($"<li><a class=\"my-level-{heading.Level}\" href=\"#{heading.GetAttributes().Id}\">{inline.Content.ToString()}</a></li>");
        }

        sb.AppendLine("</ul>");

        return sb.ToString();
    }

    public string GetSummary(string content, int length)
    {
        var plainText = ToPlainText(content);

        return $"{plainText[..(plainText.Length > length ? length : plainText.Length)]}...";
    }

    public string? GetImage(string id, string content)
    {
        var images = GetImages(id, content);

        return images.FirstOrDefault();
    }

    public IEnumerable<string> GetImages(string id, string content)
    {
        var document = Markdown.Parse(content, markdownPipeline);

        foreach (var linkInline in document.Descendants<LinkInline>())
        {
            if (!linkInline.IsImage)
                continue;

            if (string.IsNullOrEmpty(linkInline.Url))
                continue;

            yield return $"/media/{id}/{System.Net.WebUtility.UrlEncode(linkInline.Url)}";
        }
    }

    public static IResult MediaServer(string id, string path, IPageRepository pageRepository, IOptionsMonitor<RuntimeOption> runtimeOptionDelegate)
    {
        var page = pageRepository.GetPageById(PageId.Parse(id));

        if (page is null)
            return Results.NotFound();

        var option = runtimeOptionDelegate.CurrentValue;

        var mediaFile = new FileInfo($"{option.Workspace}{page.FolderPath}{System.Net.WebUtility.UrlDecode(path)}");
        if (mediaFile is null)
            return Results.NotFound();

        var stream = mediaFile.OpenRead();

        if (!new FileExtensionContentTypeProvider().TryGetContentType(mediaFile.Name, out var contentType) || string.IsNullOrEmpty(contentType))
            contentType = "application/octet-stream";

        return Results.File(stream, contentType);
    }
}
