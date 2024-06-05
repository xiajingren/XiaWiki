using XiaWiki.Core.Models;

namespace XiaWiki.Core.Services;

public interface IPageService
{
    IEnumerable<Page> GetAll();
}
