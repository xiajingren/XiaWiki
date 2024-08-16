using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XiaWiki.Core.Repositories;
using XiaWiki.Core.Services;
using XiaWiki.Infrastructure.Helpers;
using XiaWiki.Infrastructure.Options;
using XiaWiki.Infrastructure.Repositories;
using XiaWiki.Infrastructure.Search;
using XiaWiki.Infrastructure.Services;

namespace XiaWiki.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddWiki(this IServiceCollection services, IConfiguration configuration)
    {
        // services.Configure<WikiOption>(x =>
        // {
        //     x.Workspace = configuration["Runtime:Workspace"] ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace");
        //     //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(XiaWiki));
        // });

        services.Configure<WikiOption>(configuration.GetSection("Wiki"));

        services.AddTransient<IPageRepository, PageRepository>();
        services.AddTransient<IPageDetailRepository, PageDetailRepository>();
        services.AddTransient<IRendererService, RendererService>();
        services.AddTransient<ISearchService, SearchService>();
        services.AddTransient<IPageLiteService, PageLiteService>();

        services.AddSingleton<SearchEngine>();
        services.AddSingleton<GitCmdHelper>();

        services.AddHostedService<TaskService>();

        return services;
    }

    public static void UseWiki(this WebApplication app)
    {
        app.MapGet("/media/{id}/{path}", RendererService.MediaServer);
    }
}
