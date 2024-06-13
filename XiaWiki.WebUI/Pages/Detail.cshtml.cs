using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XiaWiki.Core.Repositories;
using XiaWiki.Core.Services;
using XiaWiki.WebUI.Models;

namespace XiaWiki.WebUI.Pages
{
    public class DetailModel(IPageRepository pageRepository, IPageDetailRepository pageDetailRepository, IRendererService rendererService) : PageModel
    {
        public PageDetail? PageDetail { get; set; }

        public string Outline { get; set; } = string.Empty;

        public IEnumerable<SideNavItem> SideNavItems { get; set; } = [];

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var pageDetail = await pageDetailRepository.GetAsync(id);

            if (pageDetail == null)
                return NotFound();

            PageDetail = pageDetail.Adapt<PageDetail>();
            PageDetail.Content = rendererService.RenderContent(id, pageDetail.Content);

            Outline = rendererService.RenderOutline(pageDetail.Content);

            var allPages = pageRepository.GetAll();

            SideNavItems = allPages.Adapt<IEnumerable<SideNavItem>>();

            return Page();
        }
    }
}
