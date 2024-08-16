namespace XiaWiki.Infrastructure.Options;

internal class WikiOption
{
    private string _workspace = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(XiaWiki));

    public string Workspace
    {
        get { return _workspace; }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _workspace = value;
            }
        }
    }

    public string GitRepository { get; set; } = string.Empty;

    public string LuceneDir => $"{Workspace}/Lucene";

    public string PagesDir => $"{Workspace}/{PagesFolderName}";

    public string PagesFolderName = $"Docs";
}
