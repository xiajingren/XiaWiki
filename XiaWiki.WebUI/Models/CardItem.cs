namespace XiaWiki.WebUI.Models;

public record CardItem(string Id, string Title, string Content, string? Image, DateTimeOffset UpdatedTime);
