using System;
using Application.Activities;
using Application.Core;
using Application.Interfaces;
using Infrastructure.Photos;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Persistence;

namespace API.Extensions
{
  // Registering services in the Dependency Injection (DI) container.
  // It is extension class => best way is to declare it as static so we don't have to create new instance of this class when we use our extension methods
  public static class ApplicationServiceExtensions
  {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
      services.AddSwaggerGen(c =>
          {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
          });

      services.AddDbContext<DataContext>(options =>
{
  var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

  string connStr;

  if (env == "Development")
  {
    // Use connection string from file.
    connStr = config.GetConnectionString("DefaultConnection");
  }
  else
  {
    var pgHost = Environment.GetEnvironmentVariable("pgHost");
    var pgPort = Environment.GetEnvironmentVariable("pgPort");
    var pgUser = Environment.GetEnvironmentVariable("pgUser");
    var pgPass = Environment.GetEnvironmentVariable("pgPass");
    var pgDb = Environment.GetEnvironmentVariable("pgDb");

    connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";
  }

  options.UseNpgsql(connStr);

});



      services.AddCors(opt =>
      {
        opt.AddPolicy("CorsPolicy", policy =>
        {
          //once we deploy ourapplication, this will become irrelevant as we will be serving our appliction from same domain
          policy.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("WWW-Authenticate", "Pagination")
                .WithOrigins(
                  "https://meetups.smihula.com",
                  "https://www.meetups.smihula.com",
                  "http://localhost:3000",
                  "http://127.0.0.1:3000"
            );
          // policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().AllowAnyOrigin();
        });
      });

      services.AddMediatR(typeof(List.Handler).Assembly);
      services.AddAutoMapper(typeof(MappingProfiles).Assembly);
      services.AddScoped<IUserAccessor, UserAccessor>();
      services.AddScoped<IPhotoAccessor, PhotoAccessor>();
      services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));
      services.AddSignalR();

      return services;
    }
  }
}