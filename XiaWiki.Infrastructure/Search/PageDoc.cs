using Lucene.Net.Documents;

namespace XiaWiki.Infrastructure.Search;

internal class PageDoc : DocBase<PageDoc>
{
    public PageDoc(string id, string title) : base(id)
    {
        Title = title;
    }

    public PageDoc(Document luceneDoc) : base(luceneDoc)
    {
        Title = luceneDoc.Get(nameof(Title));
        Content = luceneDoc.Get(nameof(Content));
    }

    public string Title { get; set; }

    public string? Content { get; set; }

    public override Document ToLuceneDoc()
    {
        var doc = base.ToLuceneDoc();

        doc.Add(new TextField(nameof(Title), Title, Field.Store.YES));
        doc.Add(new TextField(nameof(Content), Content, Field.Store.YES));

        return doc;
    }
}
