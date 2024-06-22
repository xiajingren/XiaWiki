namespace XiaWiki.Core.Models;

public class Page(string path, string title, bool isFolder)
{
    public string Path { get; } = path;

    public string Title { get; } = title;

    public bool IsFolder { get; } = isFolder;

    public IEnumerable<Page> Children { get; set; } = [];

    public int Depth = path.Split('/', StringSplitOptions.RemoveEmptyEntries).Length;

    public string FolderPath = path[..(path.LastIndexOf('/') + 1)];

    public PageParent? Parent { get; set; } = null;

    public PageId Id = PageId.Create(path);

    public DateTimeOffset? UpdatedTime { get; set; }
}
