using System.Security.Cryptography;
using System.Text;

namespace XiaWiki.Core.Models;

public class Page(string path, string title, bool isFolder)
{
    public string Path { get; } = path;

    public string Title { get; } = title;

    public bool IsFolder { get; } = isFolder;

    public IEnumerable<Page> Children { get; set; } = [];

    public int Depth = path.Split('/', StringSplitOptions.RemoveEmptyEntries).Length;

    public string FolderPath = path[..(path.LastIndexOf('/') + 1)];

    public string Id = GeneratePageId(path);

    private static string GeneratePageId(string path)
    {
        var data = MD5.HashData(Encoding.UTF8.GetBytes(path));
        var sBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        return sBuilder.ToString();
    }
}