using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using XiaWiki.Core.Repositories;
using XiaWiki.Infrastructure.Options;

namespace XiaWiki.Infrastructure.Services;

internal class RendererService
{
    public static IResult MediaServer(string id, string path, IPageRepository pageRepository, IOptionsMonitor<RuntimeOption> runtimeOptionDelegate)
    {
        var page = pageRepository.GetPageById(id);

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
