using BlogX.Core.Interfaces;
using Markdig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogX.Infrastructure.Services
{
    internal class MarkdownService : IMarkdownService
    {
        public string ToHtml(string markdown)
        {
            return Markdown.ToHtml(markdown);
        }

        public string ToPlainText(string markdown)
        {
            return Markdown.ToPlainText(markdown);
        }
    }
}
