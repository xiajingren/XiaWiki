namespace XiaWiki.Core.Models;

public class PageDetail(string path, string title, string author, string content, DateTimeOffset updatedTime)
{
    public string Path { get; } = path;

    public string Title { get; } = title;

    public string Author { get; } = author;

    public string Content { get; } = content;

    public ParentPage? ParentPage { get; set; } = null;

    public DateTimeOffset UpdatedTime { get; } = updatedTime;

    public IReadOnlyCollection<ParentPage> ParentPages
    {
        get
        {
            if (_parentPages is null)
            {
                _parentPages = [];

                ListParentPages(ParentPage);

                _parentPages.Reverse();
            }

            return _parentPages.AsReadOnly();
        }
    }

    private List<ParentPage>? _parentPages = null;

    private void ListParentPages(ParentPage? parent)
    {
        _parentPages ??= [];

        if (parent is not null)
            _parentPages.Add(parent);

        if (parent?.Parent is not null)
            ListParentPages(parent.Parent);
    }

}
