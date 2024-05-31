using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XiaWiki.Core.Repositories;

namespace XiaWiki.WebUI.Pages;

public class IndexModel(ILogger<IndexModel> logger, IPageRepository pageRepository) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;

    private readonly IPageRepository _pageRepository = pageRepository;

    public IEnumerable<PageVM> Pages { get; set; } = [];

    public void OnGet()
    {
        var allPages = _pageRepository.GetAll();

        Pages = allPages.Adapt<IEnumerable<PageVM>>();
    }

    public record PageVM(string Path, string Title, bool IsFolder, IEnumerable<PageVM> Children, int Depth);
}
