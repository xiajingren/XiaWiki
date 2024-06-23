using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XiaWiki.Core.Repositories;
using XiaWiki.Core.Services;
using XiaWiki.WebUI.Models;

namespace XiaWiki.WebUI.Pages;

public class SearchModel(IPageRepository pageRepository, IPageLiteService pageLiteService) : PageModel
{
    public SideNav SideNav { get; set; } = new SideNav([], "");

    public TopNav TopNav { get; set; } = new TopNav([]);

    public IList<CardItem> SearchResults { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(string? keywords)
    {
        var allPages = pageRepository.GetAll();

        SideNav = new SideNav(allPages.Adapt<IEnumerable<SideNavItem>>(), "s");

        if (keywords is not null)
        {
            var pageLites = pageLiteService.SearchAsync(keywords);
            await foreach (var page in pageLites)
            {
                SearchResults.Add(new CardItem(page.Id.ToString(),
                                            page.Title,
                                            page.Summary,
                                            page.Images.FirstOrDefault(),
                                            page.UpdatedTime));
            }
        }

        return Page();
    }
}