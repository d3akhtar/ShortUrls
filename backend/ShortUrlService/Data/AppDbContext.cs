using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShortUrlService.Model;

namespace ShortUrlService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<ShortUrl> ShortUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortUrl>().HasData(new List<ShortUrl> {
                new ShortUrl { Code = "test", DestinationUrl = "https://www.youtube.com/watch?v=UyPnhOpngRA&ab_channel=Toast", IsAlias = false}
            });
        }
    }
}