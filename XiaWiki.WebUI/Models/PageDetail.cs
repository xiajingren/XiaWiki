namespace XiaWiki.WebUI.Models;

public record PageDetail(string Path, string Title, string Author, string Content, DateTimeOffset UpdatedTime)
{
    public string Content { get; set; } = Content;
}
