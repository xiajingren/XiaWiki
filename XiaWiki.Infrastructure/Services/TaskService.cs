using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using XiaWiki.Core.Repositories;
using XiaWiki.Core.Services;
using XiaWiki.Infrastructure.Search;

namespace XiaWiki.Infrastructure.Services;

internal class TaskService(ILogger<TaskService> logger, IPageDetailRepository pageDetailRepository, SearchEngine searchEngine, IRendererService rendererService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(60));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            logger.LogInformation("task runing... {time}", DateTimeOffset.Now);

            // 1. git pull

            // 2. write index
            await WriteIndex(stoppingToken);
        }
    }

    private async Task WriteIndex(CancellationToken cancellationToken = default)
    {
        var all = pageDetailRepository.GetAllAsync();

        var docs = new List<PageDoc>();
        await foreach (var item in all)
        {
            docs.Add(new(item.Id.ToString(), item.Title)
            {
                Content = rendererService.ToPlainText(item.Content)
            });
        }

        searchEngine.WriteIndex(docs);
    }
}
