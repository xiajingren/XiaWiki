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

    private IEnumerable<Page> TraverseDirectory(DirectoryInfo dir)
    {
        var subDirs = dir.GetDirectories();
        foreach (var subDir in subDirs)
        {
            yield return new Page(ConvertToRelativePath(subDir.FullName), subDir.Name, true)
            {
                Children = TraverseDirectory(subDir)
            };
        }

        var files = dir.GetFiles();
        foreach (var file in files)
        {
            yield return new Page(ConvertToRelativePath(file.FullName), file.Name, false);
        }
    }

    private string ConvertToRelativePath(string path)
    {
        var option = _runtimeOptionDelegate.CurrentValue;

        return path.Replace(option.Workspace, string.Empty).Replace('\\', '/');
    }

}
