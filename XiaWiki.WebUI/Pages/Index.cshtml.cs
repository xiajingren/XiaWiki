using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XiaWiki.Core.Repositories;
using XiaWiki.Core.Services;
using XiaWiki.WebUI.Models;

namespace XiaWiki.WebUI.Pages;

public class IndexModel(IPageRepository pageRepository, IPageLiteService pageLiteService, ILogger<IndexModel> logger) : PageModel
{
    public SideNav SideNav { get; set; } = new SideNav([], "");

    public TopNav TopNav { get; set; } = new TopNav([]);

    public IList<CardItem> LatestUpdates { get; set; } = [];

    public IList<CardItem> RandomList { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogWarning("index 1 {time}", DateTimeOffset.Now.ToString());
        var allPages = pageRepository.GetAll();
        logger.LogWarning("index 2 {time}", DateTimeOffset.Now.ToString());

        SideNav = new SideNav(allPages.Adapt<IEnumerable<SideNavItem>>(), "");

        logger.LogWarning("index 3 {time}", DateTimeOffset.Now.ToString());
        var latestUpdates = pageLiteService.GetLatestUpdatesAsync(12);
        await foreach (var page in latestUpdates)
        {
            LatestUpdates.Add(new CardItem(page.Id.ToString(),
                                           page.Title,
                                           page.Summary,
                                           AppendQualityToImageUrl(page.Images.FirstOrDefault()),
                                           page.UpdatedTime));
        }
        logger.LogWarning("index 4 {time}", DateTimeOffset.Now.ToString());

        var randomList = pageLiteService.GetRandomListAsync(12);
        await foreach (var page in randomList)
        {
            RandomList.Add(new CardItem(page.Id.ToString(),
                                        page.Title,
                                        page.Summary,
                                        AppendQualityToImageUrl(page.Images.FirstOrDefault()),
                                        page.UpdatedTime));
        }

        logger.LogWarning("index 5 {time}", DateTimeOffset.Now.ToString());

        return Page();
    }

    static string? AppendQualityToImageUrl(string? imageUrl)
    {
        return string.IsNullOrEmpty(imageUrl) ? null : $"{imageUrl}?q=l";
    }

}
