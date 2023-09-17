using BlogX.Core.Interfaces;
using BlogX.Core.Repositories;
using BlogX.Infrastructure.Config;
using BlogX.Infrastructure.Data;
using BlogX.Infrastructure.Repositories;
using BlogX.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlogX.Infrastructure
{
    public static class Startup
    {
        public static void AddBlogX(this IServiceCollection services, Action<AppConfig>? options = null)
        {
            Initialize(services, options);

            AddDbContext(services);

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton<IMarkdownService, MarkdownService>();

            services.AddSingleton<IBlobStorageService, FileSystemStorageService>();

            services.AddHttpClient();
            services.AddSingleton<IDownloadService, DownloadService>();
        }

        private static void Initialize(IServiceCollection services, Action<AppConfig>? options = null)
        {
            var appConfig = AppConfig.BuildDefaultAppConfig();
            options?.Invoke(appConfig);

            if (!Directory.Exists(appConfig.AppDataPath))
                Directory.CreateDirectory(appConfig.AppDataPath);

            if (!Directory.Exists(appConfig.DataBasePath))
                Directory.CreateDirectory(appConfig.DataBasePath);

            if (!Directory.Exists(appConfig.BlobPath))
                Directory.CreateDirectory(appConfig.BlobPath);

            services.AddSingleton(appConfig);
        }

        private static void AddDbContext(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            // var config = serviceProvider.GetRequiredService<IConfiguration>();
            // services.AddDbContext<BlogXDbContext>(options => options.UseSqlite(config.GetConnectionString("BlogXDb")));

            var appConfig = serviceProvider.GetRequiredService<AppConfig>();
            var dbPath = Path.Join(appConfig.DataBasePath, "BlogX.db");
            services.AddDbContext<BlogXDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));
        }

        public static void UseBlogX(this WebApplication app)
        {
            var appConfig = app.Services.GetRequiredService<AppConfig>();

            app.MapGet(appConfig.BlobUri + "/{blobName}", GetByBlobName);
            app.MapPost(appConfig.BlobUri, CreateBlob);

            //SeedDataInitialize
            using var scope = app.Services.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<BlogXDbContext>();

            db.SeedData();
        }

        private static async Task<IResult> GetByBlobName(string blobName, IBlobStorageService blobStorageService)
        {
            var stream = await blobStorageService.GetAsync(blobName);

            if (!new FileExtensionContentTypeProvider().TryGetContentType(blobName, out var contentType) || string.IsNullOrEmpty(contentType))
                contentType = "application/octet-stream";

            return Results.File(stream, contentType);
        }

        private static async Task<IResult> CreateBlob(IFormFile file, IBlobStorageService blobStorageService, AppConfig appConfig)
        {
            using var stream = file.OpenReadStream();

            var blobName = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";

            var success = await blobStorageService.PutAsync(blobName, stream);
            if (!success)
                return Results.BadRequest();

            return Results.Accepted($"{appConfig.BlobUri}/{blobName}", blobName);
        }

    }
}