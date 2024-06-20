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
    }

    public string Title { get; set; }

    public string? Content { get; set; }

    public override Document ToLuceneDoc()
    {
        return [
            new StringField(nameof(Id), Id, Field.Store.YES),
            new TextField(nameof(Title), Title, Field.Store.YES),
            new TextField(nameof(Content), Content, Field.Store.NO),
        ];
    }
}
