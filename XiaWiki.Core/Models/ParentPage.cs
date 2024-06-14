namespace XiaWiki.Core.Models;

public record ParentPage(string Id, string Title, ParentPage? Parent);
