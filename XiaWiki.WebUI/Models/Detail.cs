using XiaWiki.Core.Models;

namespace XiaWiki.WebUI.Models;

public record Detail(string Path, string Title, string Author, string Content, DateTimeOffset UpdatedTime)
{
    public string Content { get; set; } = Content;
}
