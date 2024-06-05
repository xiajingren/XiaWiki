namespace XiaWiki.Core.Models;

public class PageDetail(string path, string title, string author, string content, DateTimeOffset updatedTime)
{
    public string Path { get; } = path;

    public string Title { get; } = title;

    public string Author { get; } = author;

    public string Content { get; } = content;

    public DateTimeOffset UpdatedTime { get; } = updatedTime;
}
