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

  // ToDo: Docker in local development is not running under env == Development, I have to delete else for local development
  if (env == "Development")
  {
    // Use connection string from file.
    connStr = config.GetConnectionString("DefaultConnection");
  }
  else
  {
    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    // Split the URL into parts using '@' to separate credentials and the rest of the URL
    var parts = connUrl.Split('@');
    var credentials = parts[0];
    var url = parts[1];

    // Extract username and password
    var userPass = credentials.Split(':')[2]; // Adjust index based on your URL format
    var pgUser = userPass.Split(':')[0];
    var pgPass = userPass.Split(':')[1];

    // Extract host, port, and database
    var hostPortDb = url.Split('/');
    var pgHostPort = hostPortDb[0];
    var pgDb = hostPortDb[1];

    var pgHost = pgHostPort.Split(':')[0];
    var pgPort = pgHostPort.Split(':')[1];

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
                .WithOrigins("http://localhost:3000", "http://127.0.0.1:3000");
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