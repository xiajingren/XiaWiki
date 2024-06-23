namespace XiaWiki.Core.Services;

public interface ISearchService
{
    IEnumerable<Dictionary<string, string>> Search(string keyword, Dictionary<string, float> fields);
}
