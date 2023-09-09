using BlogX.Core.Entities;
using BlogX.Core.Interfaces;
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

        private readonly IMarkdownService _markdownService;

        public CreateModel(ILogger<IndexModel> logger, IRepository<Post> postRepository, IMarkdownService markdownService)
        {
            _logger = logger;
            _postRepository = postRepository;
            _markdownService = markdownService;
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

            PostVM.Content = _markdownService.ConvertImageUrl(PostVM.Content, null);
            await _postRepository.AddAsync(new Post(PostVM.Title, PostVM.Content));

            return RedirectToPage("./Index");
        }
    }
}
