using BlogX.Core.Entities;
using BlogX.Core.Interfaces;
using BlogX.Core.Repositories;
using BlogX.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogX.WebUI.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRepository<Post> _postRepository;
        private readonly IMarkdownService _markdownService;

        public DetailsModel(ILogger<IndexModel> logger, IRepository<Post> postRepository, IMarkdownService markdownService)
        {
            _logger = logger;
            _postRepository = postRepository;
            _markdownService = markdownService;
        }


        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public PostVM PostVM { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var post = await _postRepository.GetByIdAsync(Id);
            if(post == null) 
                return NotFound();

            PostVM = PostVM.Mapping(post);

            PostVM.Content = _markdownService.ToHtml(post.Content);

            return Page();
        }
    }
}
