using BlogX.Core.Entities;
using BlogX.Core.Repositories;
using BlogX.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogX.WebUI.Pages.Admin.Posts
{
    public class EditModel : PageModel
    {
        private readonly IRepository<Post> _postRepository;

        public EditModel(IRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);

            if (post == null)
                return NotFound();

            PostVM = PostVM.Mapping(post);

            return Page();
        }

        [BindProperty]
        public PostVM PostVM { get; set; } = default!;
    }
}
