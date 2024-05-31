using XiaWiki.Core.Models;

namespace XiaWiki.Core.Services;


public interface IWikiService
{
    IEnumerable<Page> GetPages();
}
