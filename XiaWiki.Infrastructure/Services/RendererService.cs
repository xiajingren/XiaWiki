using System.Text;
using Markdig;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using XiaWiki.Core.Models;
using XiaWiki.Core.Repositories;
using XiaWiki.Core.Services;
using XiaWiki.Infrastructure.Options;
using XiaWiki.Shared.Extensions;

namespace XiaWiki.Infrastructure.Services;

internal class RendererService : IRendererService
{
    private static readonly MarkdownPipeline markdownPipeline = new MarkdownPipelineBuilder()
                                                                    .UseAutoLinks(new Markdig.Extensions.AutoLinks.AutoLinkOptions { OpenInNewWindow = true })
                                                                    .UseAutoIdentifiers(Markdig.Extensions.AutoIdentifiers.AutoIdentifierOptions.GitHub)
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

            linkInline.Url = $"/media/{id}/{linkInline.Url.UrlDecode().ToBase64Url()}";
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

            yield return $"/media/{id}/{linkInline.Url.UrlDecode().ToBase64Url()}";
        }
    }

    public static async Task<IResult> MediaServer(string id, string path, [FromQuery(Name = "q")] string? quality,
            HttpContext context, IPageRepository pageRepository, IOptionsMonitor<RuntimeOption> runtimeOptionDelegate, CancellationToken cancellationToken)
    {
        var page = pageRepository.GetPageById(PageId.Parse(id));

        if (page is null)
            return Results.NotFound();

        var option = runtimeOptionDelegate.CurrentValue;

        var mediaFile = new FileInfo($"{option.Workspace}{page.FolderPath}{path.FromBase64Url()}");
        if (mediaFile is null || !mediaFile.Exists)
            return Results.NotFound();

        var stream = mediaFile.OpenRead();

        // if (!new FileExtensionContentTypeProvider().TryGetContentType(mediaFile.Name, out var contentType) || string.IsNullOrEmpty(contentType))
        //     contentType = "application/octet-stream";

        context.Response.Headers.CacheControl = "public,max-age=3600"; // 缓存1小时

        var compressionStream = await CompressionImage(stream, quality, cancellationToken);
        return Results.File(compressionStream, "image/webp");
    }

    private static async Task<Stream> CompressionImage(FileStream stream, string? quality = ImageQuality.None, CancellationToken cancellationToken = default)
    {
        using var img = await Image.LoadAsync(stream, cancellationToken);
        //if (img.Metadata.DecodedImageFormat == JpegFormat.Instance) { }

        WebpEncoder encoder = (quality?.ToUpper()) switch
        {
            ImageQuality.Low => new WebpEncoder() { Quality = 25 },
            ImageQuality.Medium => new WebpEncoder() { Quality = 50 },
            ImageQuality.High => new WebpEncoder() { Quality = 75 },
            _ => new WebpEncoder() { Quality = 100 },
        };

        var ms = new MemoryStream();
        await img.SaveAsWebpAsync(ms, encoder, cancellationToken);
        ms.Seek(0, SeekOrigin.Begin); //seek begin

        return ms;
    }

    record ImageQuality
    {
        public const string None = "";
        public const string Low = "L";
        public const string Medium = "M";
        public const string High = "H";
    }

}
