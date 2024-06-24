namespace XiaWiki.WebUI.Models;

public record CardItem(string Id, string Title, string Summary, string? Image, DateTimeOffset UpdatedTime, IEnumerable<Breadcrumb> Breadcrumbs);
