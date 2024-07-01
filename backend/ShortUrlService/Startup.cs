
using ShortUrlService.Data;
using ShortUrlService.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
public class Startup
{
    public IConfiguration Configuration { get; set; }

    private readonly IWebHostEnvironment _env;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        _env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("ShortUrls"));
        services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
        services.AddAuthentication(opt => {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt => {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]))
            };
        });
        services.AddAuthorization();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService"));
        }
        
        // Note, authentication has to come before authorization
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/security/test", () => "Accessed").RequireAuthorization(); // test token auth
            endpoints.MapControllers();
        });
    }
}