using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogX.Core.Entities
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Post : EntityBase
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string? Author { get; set; }

        public string Summary { get; set; }

        public string? Img { get; set; }

        public Post(string title, string content, string summary)
        {
            Title = title;
            Content = content;
            Summary = summary;
        }
    }
}
