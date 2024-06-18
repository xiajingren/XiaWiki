using System.Security.Cryptography;
using System.Text;

namespace XiaWiki.Core.Models;

public class PageId
{
    private PageId(string value)
    {
        Value = value;
    }

    public string Value { get; init; }

    public bool IsNullOrEmpty => string.IsNullOrEmpty(Value);

    public static PageId Create(string path)
    {
        var data = MD5.HashData(Encoding.UTF8.GetBytes(path));
        var sb = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sb.Append(data[i].ToString("x2"));
        }
        return new PageId(sb.ToString());
    }

    public static PageId Parse(string id) => new(id);

    public override string ToString()
    {
        return Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is PageId pageId)
            return Value == pageId.Value;

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    public static bool operator ==(PageId left, PageId right)
    {
        return EqualityComparer<PageId>.Default.Equals(left, right);
    }

    public static bool operator !=(PageId left, PageId right)
    {
        return !(left == right);
    }

}
