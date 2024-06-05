namespace XiaWiki.WebUI.Models;

public record SideNavItem(string Path, string Title, bool IsFolder, IEnumerable<SideNavItem> Children, int Depth, string Id);
