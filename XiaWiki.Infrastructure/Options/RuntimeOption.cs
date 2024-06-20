namespace XiaWiki.Infrastructure.Options;

internal class RuntimeOption
{
    public string Workspace { get; set; } = string.Empty;

    public string LuceneDir => $"{Workspace}/lucene";

    public string PagesDir => $"{Workspace}/pages";
}
