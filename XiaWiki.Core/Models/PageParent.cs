namespace XiaWiki.Core.Models;

public record PageParent(PageId Id, string Title, PageParent? Parent);
