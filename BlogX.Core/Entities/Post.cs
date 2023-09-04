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

        public string Author { get; set; } = "xhznl";

        public Post(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}
