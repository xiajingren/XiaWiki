namespace XiaWiki.Core.Services;

public interface IRendererService
{
    string RenderContent(string id, string content);

    string RenderOutline(string content);
}
