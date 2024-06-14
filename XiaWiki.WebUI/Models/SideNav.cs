namespace XiaWiki.WebUI.Models;

public record SideNav(IEnumerable<SideNavItem> NavItems, string ActiveItemKey);