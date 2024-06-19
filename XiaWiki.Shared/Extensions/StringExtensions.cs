using System.Text;

namespace XiaWiki.Shared.Extensions;

public static class StringExtensions
{
    public static string ToBase64(this string str)
    {
        var base64String = Encoding.UTF8.GetBytes(str);
        return Convert.ToBase64String(base64String);
    }

    public static string FromBase64(this string base64str)
    {
        var str = Convert.FromBase64String(base64str);
        return Encoding.UTF8.GetString(str);
    }

    public static string ToBase64Url(this string str)
    {
        var base64String = str.ToBase64();
        return base64String.TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    public static string FromBase64Url(this string base64Str)
    {
        var base64Str1 = base64Str.Replace('-', '+').Replace('_', '/');
        switch (base64Str1.Length % 4)
        {
            case 2: base64Str1 += "=="; break;
            case 3: base64Str1 += "="; break;
        }

        return base64Str1.FromBase64();
    }

    public static string UrlDecode(this string str)
    {
        return System.Net.WebUtility.UrlDecode(str);
    }

    public static string UrlEncode(this string str)
    {
        return System.Net.WebUtility.UrlEncode(str);
    }
}
