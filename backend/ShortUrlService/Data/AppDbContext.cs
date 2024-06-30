using Microsoft.EntityFrameworkCore;
using ShortUrlService.Model;

namespace ShortUrlService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<ShortUrl> ShortUrls { get; set; }
    }
}