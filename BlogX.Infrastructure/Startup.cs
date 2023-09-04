using BlogX.Core.Interfaces;
using BlogX.Core.Repositories;
using BlogX.Infrastructure.Data;
using BlogX.Infrastructure.Repositories;
using BlogX.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogX.Infrastructure
{
    public static class Startup
    {
        public static void AddBlogX(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.AddDbContext<BlogXDbContext>(options => options.UseSqlite(config.GetConnectionString("BlogXDb")));

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton<IMarkdownService, MarkdownService>();

            services.AddSingleton<IBlobStorageService, BlobStorageService>();
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