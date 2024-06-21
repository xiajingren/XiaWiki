using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XiaWiki.Core.Repositories;
using XiaWiki.Core.Services;
using XiaWiki.WebUI.Models;

namespace XiaWiki.WebUI.Pages;

public class IndexModel(IPageRepository pageRepository, IPageDetailRepository pageDetailRepository, IRendererService rendererService, ILogger<IndexModel> logger) : PageModel
{
    public SideNav SideNav { get; set; } = new SideNav([], "");

    public TopNav TopNav { get; set; } = new TopNav([]);

    public IList<CardItem> LatestUpdates { get; set; } = [];

    public IList<CardItem> RandomList { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogDebug("index 1 {time}", DateTimeOffset.Now.ToString());
        var allPages = pageRepository.GetAll();
        logger.LogDebug("index 2 {time}", DateTimeOffset.Now.ToString());

        SideNav = new SideNav(allPages.Adapt<IEnumerable<SideNavItem>>(), "");

        logger.LogDebug("index 3 {time}", DateTimeOffset.Now.ToString());
        var latestUpdates = pageDetailRepository.GetLatestUpdatesAsync(12);
        await foreach (var page in latestUpdates)
        {
            LatestUpdates.Add(new CardItem(page.Id.ToString(),
                                           page.Title,
                                           rendererService.GetSummary(page.Content, 50),
                                           AppendQualityToImageUrl(rendererService.GetImage(page.Id.ToString(), page.Content)),
                                           page.UpdatedTime));
        }
        logger.LogDebug("index 4 {time}", DateTimeOffset.Now.ToString());

        var randomList = pageDetailRepository.GetRandomListAsync(12);
        await foreach (var page in randomList)
        {
            RandomList.Add(new CardItem(page.Id.ToString(),
                                           page.Title,
                                           rendererService.GetSummary(page.Content, 50),
                                           AppendQualityToImageUrl(rendererService.GetImage(page.Id.ToString(), page.Content)),
                                           page.UpdatedTime));
        }

        logger.LogDebug("index 5 {time}", DateTimeOffset.Now.ToString());

        return Page();
    }

    static string? AppendQualityToImageUrl(string? imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl)) return null;

        return $"{imageUrl}?q=l";
    }

}
