using UserShortUrlService.Model;
using Microsoft.EntityFrameworkCore;

namespace UserShortUrlService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<UserShortUrl> UserShortUrls { get; set; }
        public DbSet<User> Users { get; set; }
    }
}