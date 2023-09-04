using BlogX.Core.Entities;

namespace BlogX.WebUI.Models
{
    public class PostVM
    {
        public int Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string Title { get; set; } = default!;

        public string Content { get; set; } = default!;

        public string Author { get; set; } = default!;

        public static PostVM Mapping(Post post)
        {
            return new PostVM
            {
                Id = post.Id,
                CreatedTime = post.CreatedTime,
                UpdatedTime = post.UpdatedTime,
                Title = post.Title,
                Content = post.Content,
                Author = post.Author,
            };
        }
    }
}
