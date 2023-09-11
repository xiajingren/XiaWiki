using BlogX.Core.Interfaces;
using Markdig;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
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

        public async Task<string> ConvertImageUrlAsync(string markdown, Func<string, Task<string>> covertFunc)
        {
            var document = Markdown.Parse(markdown);

            foreach (var node in document)
            {
                if (node is not ParagraphBlock { Inline: { } } paragraphBlock) continue;

                foreach (var inline in paragraphBlock.Inline)
                {
                    if (inline is not LinkInline { IsImage: true } linkInline) continue;

                    if (string.IsNullOrWhiteSpace(linkInline.Url)) continue;

                    linkInline.Url = await covertFunc(linkInline.Url);
                }
            }

            using var writer = new StringWriter();
            var render = new NormalizeRenderer(writer);
            render.Render(document);

            return writer.ToString();
        }

        public List<string> GetImageUrls(string markdown)
        {
            var imageUrls = new List<string>();

            var document = Markdown.Parse(markdown);

            foreach (var node in document)
            {
                if (node is not ParagraphBlock { Inline: { } } paragraphBlock) continue;

                foreach (var inline in paragraphBlock.Inline)
                {
                    if (inline is not LinkInline { IsImage: true } linkInline) continue;

                    if (string.IsNullOrWhiteSpace(linkInline.Url)) continue;

                    imageUrls.Add(linkInline.Url);
                }
            }

            return imageUrls;
        }
    }
}

