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

        public SideNav SideNav { get; set; } = new SideNav([], "");

        public TopNav TopNav { get; set; } = new TopNav([]);

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var pageDetail = await pageDetailRepository.GetAsync(id);

            if (pageDetail is null)
                return NotFound();

            PageDetail = pageDetail.Adapt<PageDetail>();
            PageDetail.Content = rendererService.RenderContent(id, pageDetail.Content);

            Outline = rendererService.RenderOutline(pageDetail.Content);

            var allPages = pageRepository.GetAll();
            SideNav = new SideNav(allPages.Adapt<IEnumerable<SideNavItem>>(), id);

            var breadcrumbs = pageDetail.ParentPages.Select(x => new Breadcrumb(x.Id, x.Title)).ToList();
            breadcrumbs.Add(new Breadcrumb(id, pageDetail.Title));
            TopNav = new TopNav(breadcrumbs);

            return Page();
        }
    }
}
