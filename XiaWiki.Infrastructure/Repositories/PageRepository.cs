using Microsoft.Extensions.Options;
using XiaWiki.Core.Models;
using XiaWiki.Core.Repositories;
using XiaWiki.Infrastructure.Options;
using XiaWiki.Shared.Extensions;

namespace XiaWiki.Infrastructure.Repositories;

internal class PageRepository(IOptionsMonitor<RuntimeOption> runtimeOptionDelegate) : IPageRepository
{
    private readonly IOptionsMonitor<RuntimeOption> _runtimeOptionDelegate = runtimeOptionDelegate;

    public IEnumerable<Page> GetAll()
    {
        var option = _runtimeOptionDelegate.CurrentValue;

        var workspaceDir = new DirectoryInfo(option.PagesDir);

        if (workspaceDir is null || !workspaceDir.Exists)
            throw new ApplicationException("workspace is not exists...");

        return TraverseDirectory(workspaceDir);
    }

    public IDictionary<string, Page> GetAllWithoutChildren()
    {
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
        var option = _runtimeOptionDelegate.CurrentValue;

        return path.Replace(option.PagesDir, string.Empty).Replace('\\', '/');
    }
}
