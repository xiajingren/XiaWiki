namespace XiaWiki.Core.Models;

public class Page(string path, string title, bool isFolder)
{
    public string Path { get; set; } = path;

    public string Title { get; set; } = title;

    public bool IsFolder { get; set; } = isFolder;

    public IEnumerable<Page> Children { get; set; } = [];

    public int Depth = path.Split('/', StringSplitOptions.RemoveEmptyEntries).Length;
}
