namespace XiaWiki.Core.Models;

public class Wiki
{
    internal Wiki(string title, string author, string content, DateTimeOffset updatedTime)
    {
        Title = title;
        Author = author;
        Content = content;
        UpdatedTime = updatedTime;
    }

    public string Title { get; internal set; }

    public string Author { get; internal set; }

    public string Content { get; internal set; }

    public DateTimeOffset UpdatedTime { get; internal set; } = DateTimeOffset.Now;
}
