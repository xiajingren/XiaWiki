using BlogX.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogX.Infrastructure.Data
{
    internal class BlogXDbContext : DbContext
    {
        public BlogXDbContext(DbContextOptions<BlogXDbContext> options) : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; } = default!;

        public void SeedData()
        {
            var pendingMigrations = Database.GetPendingMigrations();
            if (pendingMigrations.Any())
                Database.Migrate();

            //if (Posts.Any())
            //    return;

            Posts.Add(new Post("title " + DateTimeOffset.Now, "hahah " + DateTimeOffset.Now));

            SaveChanges();
        }
    }
}
