namespace XiaWiki.Infrastructure.Options;

internal class RuntimeOption
{
    public string Workspace { get; set; } = string.Empty;

    public string LuceneDir => $"{Workspace}/Lucene";

    public string PagesDir => $"{Workspace}/DemoPages";
}
