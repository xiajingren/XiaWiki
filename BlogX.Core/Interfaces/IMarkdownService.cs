using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogX.Core.Interfaces
{
    public interface IMarkdownService
    {
        string ToHtml(string markdown);

        string ToPlainText(string markdown);

        Task<string> ConvertImageUrlAsync(string markdown, Func<string, Task<string>> covertFunc);
    }
}
