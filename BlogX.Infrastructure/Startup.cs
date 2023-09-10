using BlogX.Core.Interfaces;
using BlogX.Core.Repositories;
using BlogX.Infrastructure.Config;
using BlogX.Infrastructure.Data;
using BlogX.Infrastructure.Repositories;
using BlogX.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlogX.Infrastructure
{
    public static class Startup
    {
        public static void AddBlogX(this IServiceCollection services)
        {
            Initialize(services);

            AddDbContext(services);

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton<IMarkdownService, MarkdownService>();

            services.AddSingleton<IBlobStorageService, FileSystemStorageService>();

            services.AddHttpClient();
            services.AddSingleton<IDownloadService, DownloadService>();
        }

        private static void Initialize(IServiceCollection services)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var appDataPath = Path.Join(path, ".BlogX");
            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);

            var dataBasePath = Path.Join(appDataPath, "db");
            if (!Directory.Exists(dataBasePath))
                Directory.CreateDirectory(dataBasePath);

            var blobPath = Path.Join(appDataPath, "blob");
            if (!Directory.Exists(blobPath))
                Directory.CreateDirectory(blobPath);

            services.Configure<DefaultAppConfig>(options =>
            {
                options.AppDataPath = appDataPath;
                options.DataBasePath = dataBasePath;
                options.BlobPath = blobPath;
            });
        }

        private static void AddDbContext(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            // var config = serviceProvider.GetRequiredService<IConfiguration>();
            // services.AddDbContext<BlogXDbContext>(options => options.UseSqlite(config.GetConnectionString("BlogXDb")));

            var defaultAppConfig = serviceProvider.GetRequiredService<IOptions<DefaultAppConfig>>();
            var dbPath = Path.Join(defaultAppConfig.Value.DataBasePath, "BlogX.db");
            services.AddDbContext<BlogXDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));
        }

        public static void UseBlogX(this IApplicationBuilder app)
        {
            //SeedDataInitialize
            using var scope = app.ApplicationServices.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<BlogXDbContext>();

            db.SeedData();
        }
    }
}