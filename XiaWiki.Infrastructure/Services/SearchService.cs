using XiaWiki.Core.Services;
using XiaWiki.Infrastructure.Search;

namespace XiaWiki.Infrastructure.Services;

internal class SearchService(SearchEngine searchEngine) : ISearchService
{
    public IEnumerable<Dictionary<string, string>> Search(string keyword, Dictionary<string, float> fields)
    {
        var docs = searchEngine.Search<PageDoc>(keyword, [.. fields.Keys], fields);

        foreach (var doc in docs)
        {
            yield return new Dictionary<string, string> {
                { nameof(doc.Id), doc.Id },
                { nameof(doc.Title), doc.Title },
                { nameof(doc.Content), doc.Content },
            };
        }
    }
}
