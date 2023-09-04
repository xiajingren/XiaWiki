using BlogX.Core.Entities;
using BlogX.Core.Repositories;
using BlogX.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogX.WebUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRepository<Post> _postRepository;

        public IndexModel(ILogger<IndexModel> logger, IRepository<Post> postRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
        }

        public IList<PostVM> PostVMs { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var posts = await _postRepository.ListAsync();

            PostVMs = posts.Select(PostVM.Mapping).ToList();

            return Page();
        }
    }
}