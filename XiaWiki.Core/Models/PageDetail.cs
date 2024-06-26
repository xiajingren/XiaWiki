﻿namespace XiaWiki.Core.Models;

public class PageDetail(string path, string title, string author, string content, DateTimeOffset updatedTime)
{
    public string Path { get; } = path;

    public string Title { get; } = title;

    public string Author { get; } = author;

    public string Content { get; } = content;

    public PageParent? Parent { get; set; } = null;

    public DateTimeOffset UpdatedTime { get; } = updatedTime;

    public PageId Id = PageId.Create(path);

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
