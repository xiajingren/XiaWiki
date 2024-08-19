using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XiaWiki.Core.Repositories;
using XiaWiki.Core.Services;
using XiaWiki.Infrastructure.Cache;
using XiaWiki.Infrastructure.Git;
using XiaWiki.Infrastructure.Options;
using XiaWiki.Infrastructure.Search;

namespace XiaWiki.Infrastructure.Services;

internal class TaskService(ILogger<TaskService> logger,
    IPageDetailRepository pageDetailRepository,
    SearchEngine searchEngine,
    IRendererService rendererService,
    IOptionsMonitor<WikiOption> wikiOptionDelegate,
    GitCmdUtils gitCmdUtils,
    CacheUtils cacheUtils) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await InternalExecuteAsync(stoppingToken);

        using var timer = new PeriodicTimer(TimeSpan.FromHours(1));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await InternalExecuteAsync(stoppingToken);
        }
    }

    private async Task InternalExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("task runing... {time}", DateTimeOffset.Now);
        var option = wikiOptionDelegate.CurrentValue;
        logger.LogInformation("workspace={workspace}", option.Workspace);

        // 1. git pull
        // await PullDocs();

        cacheUtils.RemovePagesCache();

        // 2. write index
        await WriteIndex(stoppingToken);
    }

    private async Task<bool> PullDocs()
    {
        var option = wikiOptionDelegate.CurrentValue;

        if (!await gitCmdUtils.GitCheck())
        {
            logger.LogError("Please check the Git environment...");
            return false;
        }

        if (Directory.Exists(option.PagesDir))
        {
            var (pullSuccessed, pullResult) = await gitCmdUtils.GitPullDocs();
            if (pullSuccessed)
                return !pullResult.StartsWith("Already up to date.");

            var (statusSuccessed, _) = await gitCmdUtils.GitStatus();
            if (statusSuccessed)
                return false;

            Directory.Delete(option.PagesDir, true);
        }

        var (cloneSuccessed, _) = await gitCmdUtils.GitCloneDocs();
        return cloneSuccessed;
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
