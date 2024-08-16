using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XiaWiki.Core.Repositories;
using XiaWiki.Core.Services;
using XiaWiki.Infrastructure.Helpers;
using XiaWiki.Infrastructure.Options;
using XiaWiki.Infrastructure.Search;

namespace XiaWiki.Infrastructure.Services;

internal class TaskService(ILogger<TaskService> logger,
    IPageDetailRepository pageDetailRepository,
    SearchEngine searchEngine,
    IRendererService rendererService,
    IOptionsMonitor<WikiOption> wikiOptionDelegate,
    GitCmdHelper gitCmdHelper) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await InternalExecuteAsync(stoppingToken);

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(60));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await InternalExecuteAsync(stoppingToken);
        }
    }

    private async Task InternalExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("task runing... {time}", DateTimeOffset.Now);

        // 1. git pull
        await PullDocs();

        // 2. write index
        await WriteIndex(stoppingToken);
    }

    private async Task PullDocs()
    {
        var option = wikiOptionDelegate.CurrentValue;

        if (!await gitCmdHelper.GitCheck())
            return;

        var cloneResult = await gitCmdHelper.GitCloneDocs();
    }

    private async Task WriteIndex(CancellationToken cancellationToken = default)
    {
        var all = pageDetailRepository.GetAllAsync();

        var docs = new List<PageDoc>();
        await foreach (var item in all)
        {
            docs.Add(new(item.Id.ToString(), item.Title, rendererService.ToPlainText(item.Content)));
        }

        searchEngine.WriteIndex(docs);
    }
}
