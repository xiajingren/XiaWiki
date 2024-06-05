using XiaWiki.Core.Models;

namespace XiaWiki.Core.Services;


public interface IPageDetailService
{
    PageDetail Get(string id);
}
