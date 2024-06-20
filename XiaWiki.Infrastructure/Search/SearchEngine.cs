using JiebaNet.Segmenter;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using XiaWiki.Infrastructure.Options;

namespace XiaWiki.Infrastructure.Search;

internal class SearchEngine
{
    private readonly FSDirectory _fSDirectory;
    private readonly IOptionsMonitor<RuntimeOption> _runtimeOptionDelegate;
    private readonly LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;

    public SearchEngine(IOptionsMonitor<RuntimeOption> runtimeOptionDelegate)
    {
        _runtimeOptionDelegate = runtimeOptionDelegate;

        var options = _runtimeOptionDelegate.CurrentValue;

        _fSDirectory = FSDirectory.Open(options.LuceneDir);
    }

    public void WriteIndex<T>(IEnumerable<T> docs) where T : DocBase<T>
    {
        var indexWriterConfig = new IndexWriterConfig(luceneVersion, new StandardAnalyzer(luceneVersion));

        using IndexWriter writer = new(_fSDirectory, indexWriterConfig);

        writer.DeleteAll();

        foreach (var doc in docs)
        {
            writer.AddDocument(doc.ToLuceneDoc());
        }

        writer.Flush(triggerMerge: true, applyAllDeletes: true);
        writer.Commit();
    }

    public IEnumerable<T> SearchIndex<T>(string keyword) where T : DocBase<T>
    {
        var ctor = typeof(T).GetConstructor([typeof(Document)]) ??
            throw new ApplicationException($"{typeof(T)} ctor must has a [Document] param");

        var phrase = new MultiPhraseQuery
        {
            new Term("Title", keyword),
            new Term("Content", keyword)
        };

        using var reader = DirectoryReader.Open(_fSDirectory);
        var searcher = new IndexSearcher(reader);

        var hits = searcher.Search(phrase, 20).ScoreDocs;
        foreach (var hit in hits)
        {
            var luceneDoc = searcher.Doc(hit.Doc);

            var obj = (T)ctor.Invoke([luceneDoc.Get(nameof(DocBase<T>.Id))]);
            yield return obj;
        }
    }

}
