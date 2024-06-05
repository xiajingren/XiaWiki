using XiaWiki.Core.Models;

namespace XiaWiki.Core.Repositories;

public interface IPageRepository
{
    IEnumerable<Page> GetAll();

    string? GetPathById(string id);

    Page? GetPageById(string id);
}
