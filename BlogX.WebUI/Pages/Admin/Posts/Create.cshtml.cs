using BlogX.Core.Entities;
using BlogX.Core.Repositories;
using BlogX.WebUI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogX.WebUI.Pages.Admin.Posts
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRepository<Post> _postRepository;

        public CreateModel(ILogger<IndexModel> logger, IRepository<Post> postRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public PostVM PostVM { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || PostVM == null)
            {
                return Page();
            }

            await _postRepository.AddAsync(new Post(PostVM.Title, PostVM.Content));

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            await Task.CompletedTask;
            return new JsonResult(new { x = 1 });
        }
    }
}
