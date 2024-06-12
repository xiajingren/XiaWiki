﻿using Microsoft.Extensions.Options;
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

    private IEnumerable<Page> TraverseDirectory(DirectoryInfo dir)
    {
        var subDirs = dir.GetDirectories();
        foreach (var subDir in subDirs)
        {
            var children = TraverseDirectory(subDir);
            if (!children.Any())
                continue;

            yield return new Page(ConvertToRelativePath(subDir.FullName), subDir.Name, true)
            {
                Children = children
            };
        }

        var files = dir.GetFiles();
        foreach (var file in files)
        {
            if (!file.FullName.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                continue;

            yield return new Page(ConvertToRelativePath(file.FullName), file.Name, false);
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
        if (pages == null || !pages.Any())
            return null;

        var findPage = pages.FirstOrDefault(x => x.Id == id);

        if (findPage != null)
            return findPage;

        foreach (var page in pages)
        {
            findPage = FindPage(page.Children, id);

            if (findPage != null)
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