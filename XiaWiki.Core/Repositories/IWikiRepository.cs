using XiaWiki.Core.Models;

namespace XiaWiki.Core.Repositories;

public interface IWikiRepository
{
    IEnumerable<Wiki> GetAll();
}
