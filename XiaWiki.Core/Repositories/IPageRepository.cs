using XiaWiki.Core.Models;

namespace XiaWiki.Core.Repositories;

public interface IPageRepository
{
    IEnumerable<Page> GetAll();
}
