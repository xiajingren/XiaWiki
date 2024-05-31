namespace XiaWiki.Core.Models;

public class Wiki
{
    internal Wiki(string path, string title, string author, string content, DateTimeOffset updatedTime)
    {
        Path = path;
        Title = title;
        Author = author;
        Content = content;
        UpdatedTime = updatedTime;
    }

    public string Path { get; internal set; }

    public string Title { get; internal set; }

    public string Author { get; internal set; }

    public string Content { get; internal set; }

    public DateTimeOffset UpdatedTime { get; internal set; } = DateTimeOffset.Now;
}
