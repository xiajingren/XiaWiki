using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XiaWiki.Core.Repositories;
using XiaWiki.WebUI.Models;

namespace XiaWiki.WebUI.Pages;

public class IndexModel(ILogger<IndexModel> logger, IPageRepository pageRepository) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;

    private readonly IPageRepository _pageRepository = pageRepository;

    public SideNav SideNav { get; set; } = new SideNav([], "");

    public TopNav TopNav { get; set; } = new TopNav([]);

    public void OnGet()
    {
        var allPages = _pageRepository.GetAll();

        SideNav = new SideNav(allPages.Adapt<IEnumerable<SideNavItem>>(), "");
    }
}
