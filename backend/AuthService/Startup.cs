
using AuthService.AsyncDataServices;
using AuthService.BackgroundServices;
using AuthService.Data;
using AuthService.Data.Repository;
using AuthService.ExternalAuthServices;
using AuthService.SyncDataServices.Grpc;
using AuthService.SyncDataServices.Smtp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        //services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("Users"));
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("Default")));
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IGoogleAuth, GoogleAuth>();
        services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
        services.AddGrpc();
        services.AddCors();
        services.AddHostedService<VerificationChecker>();
        services.AddScoped<IEmailService, EmailService>();
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
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGrpcService<GrpcUserService>();
            endpoints.MapGet("/protos/users.proto", async context => 
            {
                await context.Response.WriteAsync(File.ReadAllText("Protos/users.proto")); 
            }); // see users.proto file
        });
    }
}