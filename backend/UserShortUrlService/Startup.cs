
using UserShortUrlService.Data;
using UserShortUrlService.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserShortUrlService.AsyncDataServices;
using UserShortUrlService;
using UserShortUrlService.SyncDataServices.Grpc;
using UserShortUrlService.SyncDataServices.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
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
        services.AddGrpc();
        services.AddControllers();
        services.AddSwaggerGen(opt => {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        // if (_env.IsDevelopment()){
        //     services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("UserShortUrlCodes"));
        // }
        // else{
        //     services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("Default")));
        // }
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("Default")));
        services.AddScoped<IUserShortUrlCodeRepository, UserShortUrlCodeRepository>();
        services.AddHostedService<RabbitMQSubscriber>();
        services.AddScoped<IUserDataClient, UserDataClient>();
        services.AddHttpClient<IShortUrlDataClient, HttpShortUrlDataClient>();
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
        services.AddCors();
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
        
        app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*"));
        
        // Note, authentication has to come before authorization
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/security/test", () => "Accessed").RequireAuthorization(); // test token auth
            endpoints.MapControllers();
        });

        

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var db = scope.ServiceProvider.GetService<AppDbContext>();

            try{
                //if (_env.IsProduction())
                    db.Database.Migrate();
            }
            catch(Exception ex){
                Console.WriteLine("--> Error while applying migrations for usershorturl db: " + ex.Message);
            }
        }
        
        PrepDb.PrepPopulation(app);
    }
}
