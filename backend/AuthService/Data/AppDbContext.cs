using AuthService.DTO;
using AuthService.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration, IMapper mapper) : base(options)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var adminRegInfo = new RegisterRequestDTO
            {
                Username = _configuration["AdminAccountInfo:Username"],
                Email = _configuration["AdminAccountInfo:Email"],
                Password = _configuration["AdminAccountInfo:Password"]
            };
            User adminUser = _mapper.Map<User>(adminRegInfo);
            adminUser.Role = "admin"; 

            modelBuilder.Entity<User>().HasData(new List<User>{
                adminUser
            });
        }
    }
}