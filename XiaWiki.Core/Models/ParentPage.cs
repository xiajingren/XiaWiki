namespace XiaWiki.Core.Models;

public record ParentPage(PageId Id, string Title, ParentPage? Parent);
