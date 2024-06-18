using XiaWiki.Core.Models;

namespace XiaWiki.Core.Repositories;

public interface IPageRepository
{
    IEnumerable<Page> GetAll();

    string? GetPathById(PageId id);

    Page? GetPageById(PageId id);

    IDictionary<string, Page> GetAllWithoutChildren();
}
