using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using XiaWiki.Core.Repositories;
using XiaWiki.Infrastructure.Options;
using XiaWiki.Infrastructure.Repositories;

namespace XiaWiki.Infrastructure;

public static class Startup
{
    public static void ConfigureWikiServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RuntimeOption>(x =>
        {
            x.Workspace = builder.Configuration["Runtime:Workspace"] ??
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(XiaWiki));
        });

        builder.Services.AddTransient<IPageRepository, PageRepository>();
        builder.Services.AddTransient<IPageDetailRepository, PageDetailRepository>();
    }
}
