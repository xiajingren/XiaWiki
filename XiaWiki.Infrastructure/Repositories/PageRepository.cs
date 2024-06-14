using Microsoft.Extensions.Options;
using XiaWiki.Core.Models;
using XiaWiki.Core.Repositories;
using XiaWiki.Infrastructure.Options;

namespace XiaWiki.Infrastructure.Repositories;

internal class PageRepository(IOptionsMonitor<RuntimeOption> runtimeOptionDelegate) : IPageRepository
{
    private readonly IOptionsMonitor<RuntimeOption> _runtimeOptionDelegate = runtimeOptionDelegate;

    public IEnumerable<Page> GetAll()
    {
        var option = _runtimeOptionDelegate.CurrentValue;

        var workspaceDir = new DirectoryInfo(option.Workspace);

        return TraverseDirectory(workspaceDir);
    }

    private IEnumerable<Page> TraverseDirectory(DirectoryInfo dir, ParentPage? parentPage = null)
    {
        var subDirs = dir.GetDirectories();
        foreach (var subDir in subDirs)
        {
            var page = new Page(ConvertToRelativePath(subDir.FullName), subDir.Name, true) { ParentPage = parentPage };

            var children = TraverseDirectory(subDir, new ParentPage(page.Id, page.Title, parentPage));
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

            yield return new Page(ConvertToRelativePath(file.FullName), file.Name.Remove(file.Name.Length - 3), false) { ParentPage = parentPage };
        }
    }

    public Page? GetPageById(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        var pages = GetAll();
        return FindPage(pages, id);
    }

    public string? GetPathById(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        var page = GetPageById(id);
        return page?.Path;
    }

    private static Page? FindPage(IEnumerable<Page> pages, string id)
    {
        if (pages is null || !pages.Any())
            return null;

        var findPage = pages.FirstOrDefault(x => x.Id == id);

        if (findPage is not null)
            return findPage;

        foreach (var page in pages)
        {
            findPage = FindPage(page.Children, id);

            if (findPage is not null)
                return findPage;
        }

        return null;
    }

    private string ConvertToRelativePath(string path)
    {
        var option = _runtimeOptionDelegate.CurrentValue;

        return path.Replace(option.Workspace, string.Empty).Replace('\\', '/');
    }
}
