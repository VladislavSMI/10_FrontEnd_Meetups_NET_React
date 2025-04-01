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

      services.AddDbContext<DataContext>(opt =>
      {
        opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
      });

      services.AddCors(opt =>
      {
        opt.AddPolicy("CorsPolicy", policy =>
        {
          // Once we deploy our app, this will become irrelevant as we will be serving our app from same domain
          policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
          // policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
        });
      });

      services.AddMediatR(typeof(List.Handler).Assembly);
      services.AddAutoMapper(typeof(MappingProfiles).Assembly);
      services.AddScoped<IUserAccessor, UserAccessor>();
      services.AddScoped<IPhotoAccessor, PhotoAccessor>();
      services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));

      return services;
    }
  }
}