using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XiaWiki.Core.Models;
using XiaWiki.Core.Repositories;
using XiaWiki.Infrastructure.Cache;
using XiaWiki.Infrastructure.Options;
using XiaWiki.Shared.Extensions;

namespace XiaWiki.Infrastructure.Repositories;

internal class PageRepository(IOptionsMonitor<WikiOption> wikiOptionDelegate, IMemoryCache memoryCache, ILogger<PageRepository> logger) : IPageRepository
{
    private readonly IOptionsMonitor<WikiOption> _wikiOptionDelegate = wikiOptionDelegate;

    public IEnumerable<Page> GetAll()
    {
        return memoryCache.GetOrCreate(CacheKeys.PagesGetAll, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);

            var option = _wikiOptionDelegate.CurrentValue;

            var workspaceDir = new DirectoryInfo(option.PagesDir);

            if (workspaceDir is null || !workspaceDir.Exists)
            {
                logger.LogError("workspace/docs is not exists...");
                return [];
            }

            return TraverseDirectory(workspaceDir);
        })!;
    }

    public IDictionary<string, Page> GetAllWithoutChildren()
    {
        return memoryCache.GetOrCreate(CacheKeys.PagesGetAllWithoutChildren, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);

            var result = new Dictionary<string, Page>();

            var pages = GetAll();

            void addToDict(IEnumerable<Page> pages)
            {
                foreach (var page in pages)
                {
                    result[page.Id.ToString()] = page;

                    if (!page.Children.Any())
                        continue;

                    addToDict(page.Children);
                    page.Children = [];
                }
            }

            addToDict(pages);

            return result;
        })!;
    }

    private IEnumerable<Page> TraverseDirectory(DirectoryInfo dir, PageParent? parent = null)
    {
        var subDirs = dir.GetDirectories();
        foreach (var subDir in subDirs)
        {
            var page = new Page(ConvertToRelativePath(subDir.FullName), subDir.Name, true)
            {
                Parent = parent
            };

            var children = TraverseDirectory(subDir, new PageParent(page.Id, page.Title, parent));
            if (!children.Any())
                continue;

            page.Children = children;

            yield return page;
        }

        var files = dir.GetFiles();
        foreach (var file in files)
        {
            if (!file.Name.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                continue;

            yield return new Page(ConvertToRelativePath(file.FullName), file.Name.Remove(file.Name.Length - 3), false)
            {
                Parent = parent,
                UpdatedTime = DateTimeOffset.Now
            };
        }
    }

    public Page? GetPageById(PageId id)
    {
        if (id.IsNullOrEmpty)
            return null;

        var pages = GetAllWithoutChildren();

        return pages.GetOrDefault(id.ToString());
    }

    private string ConvertToRelativePath(string path)
    {
        var option = _wikiOptionDelegate.CurrentValue;

        return path.Replace(option.PagesDir, string.Empty).Replace('\\', '/');
    }
}
