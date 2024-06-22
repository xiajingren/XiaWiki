namespace XiaWiki.Core.Models;

public class PageLite(PageId id, string title, string author, string summary, IEnumerable<string> images, DateTimeOffset updatedTime)
{
    public string Title { get; } = title;

    public string Author { get; } = author;

    public string Summary { get; } = summary;

    public IEnumerable<string> Images { get; set; } = images;

    public PageParent? Parent { get; set; } = null;

    public DateTimeOffset UpdatedTime { get; } = updatedTime;

    public PageId Id = id;

    public IReadOnlyCollection<PageParent> Parents
    {
        get
        {
            if (_parents is null)
            {
                _parents = [];

                ListParentPages(Parent);

                _parents.Reverse();
            }

            return _parents.AsReadOnly();
        }
    }

    private List<PageParent>? _parents = null;

    private void ListParentPages(PageParent? parent)
    {
        _parents ??= [];

        if (parent is not null)
            _parents.Add(parent);

        if (parent?.Parent is not null)
            ListParentPages(parent.Parent);
    }

}

