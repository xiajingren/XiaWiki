using JiebaNet.Segmenter;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Search.Highlight;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Microsoft.Extensions.Options;
using XiaWiki.Infrastructure.Options;

namespace XiaWiki.Infrastructure.Search;

internal class SearchEngine
{
    private readonly FSDirectory _fSDirectory;
    private readonly IOptionsMonitor<WikiOption> _wikiOptionDelegate;
    private const LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;

    private readonly Analyzer analyzer = new StandardAnalyzer(luceneVersion);

    public SearchEngine(IOptionsMonitor<WikiOption> wikiOptionDelegate)
    {
        _wikiOptionDelegate = wikiOptionDelegate;

        var options = _wikiOptionDelegate.CurrentValue;

        _fSDirectory = FSDirectory.Open(options.LuceneDir);
    }

    public void WriteIndex<T>(IEnumerable<T> docs) where T : DocBase<T>
    {
        var indexWriterConfig = new IndexWriterConfig(luceneVersion, analyzer);

        using IndexWriter writer = new(_fSDirectory, indexWriterConfig);

        writer.DeleteAll();

        foreach (var doc in docs)
        {
            writer.AddDocument(doc.ToLuceneDoc());
        }

        //writer.Flush(triggerMerge: true, applyAllDeletes: true);
        writer.Commit();
    }

    public IEnumerable<T> Search<T>(string keyword, string[] fields, IDictionary<string, float>? boosts = null) where T : DocBase<T>
    {
        var ctor = typeof(T).GetConstructor([typeof(Document)]) ??
            throw new ApplicationException($"{typeof(T)} ctor must has a [Document] param");

        if (fields is null || fields.Length == 0)
            throw new ArgumentException("fields is empty...");

        Query query;

        if (fields.Length == 1)
        {
            var queryParser = new QueryParser(luceneVersion, fields[0], analyzer);
            query = queryParser.Parse(keyword);
        }
        else
        {
            var queryParser = boosts is null ?
                                new MultiFieldQueryParser(luceneVersion, fields, analyzer) :
                                new MultiFieldQueryParser(luceneVersion, fields, analyzer, boosts);

            query = queryParser.Parse(keyword);
        }

        using var reader = DirectoryReader.Open(_fSDirectory);
        var searcher = new IndexSearcher(reader);

        var hits = searcher.Search(query, 20).ScoreDocs;

        var highlighter = new Highlighter(new SimpleHTMLFormatter("<strong style='color:red;'>", "</strong>"), new QueryScorer(query))
        {
            TextFragmenter = new SimpleFragmenter(100)
        };

        foreach (var hit in hits)
        {
            var luceneDoc = searcher.Doc(hit.Doc);

            var obj = (T)ctor.Invoke([luceneDoc]);

            foreach (var filed in fields)
            {
                var v = typeof(T).GetProperty(filed)?.GetValue(obj)?.ToString();

                if (string.IsNullOrEmpty(v))
                    continue;

                var fragment = highlighter.GetBestFragment(analyzer, filed, v);
                typeof(T).GetProperty(filed)?.SetValue(obj, fragment);
            }

            yield return obj;
        }
    }

}
