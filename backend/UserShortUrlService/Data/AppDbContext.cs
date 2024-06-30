using UserShortUrlService.Model;
using Microsoft.EntityFrameworkCore;

namespace UserShortUrlService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<UserShortUrlCode> UserShortUrlCodes { get; set; }
        public DbSet<User> Users { get; set; }
    }
}