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
        private readonly IDownloadService _downloadService;
        private readonly IBlobStorageService _blobStorageService;

        public CreateModel(ILogger<IndexModel> logger, IRepository<Post> postRepository, IMarkdownService markdownService,
            IDownloadService downloadService, IBlobStorageService blobStorageService)
        {
            _logger = logger;
            _postRepository = postRepository;
            _markdownService = markdownService;
            _downloadService = downloadService;
            _blobStorageService = blobStorageService;
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

            PostVM.Content = await _markdownService.ConvertImageUrlAsync(PostVM.Content, ImageUrlHandle);

            await _postRepository.AddAsync(new Post(PostVM.Title, PostVM.Content));

            return RedirectToPage("./Index");
        }

        private async Task<string> ImageUrlHandle(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            if (url.StartsWith("/api/blob"))
                return url;

            if (!url.StartsWith("http"))
                return url;

            using var stream = await _downloadService.DownloadAsync(url);

            var blobName = $"{Guid.NewGuid():N}{Path.GetExtension(url)}";

            await _blobStorageService.PutAsync(blobName, stream);

            return $"/api/blob/{blobName}";
        }

    }
}
