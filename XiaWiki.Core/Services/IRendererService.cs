namespace XiaWiki.Core.Services;

public interface IRendererService
{
    string ToHtml(string id, string content);

    string ToPlainText(string content);

    string GetOutline(string content);

    string GetSummary(string content, int length);

    string? GetImage(string id, string content);

    IEnumerable<string> GetImages(string id, string content);
}
