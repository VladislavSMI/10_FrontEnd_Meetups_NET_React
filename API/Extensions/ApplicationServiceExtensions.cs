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
  //this class will create services that will be inherited and used in our Startup class
  //It is extension class => best way is to declare it as static so we don't have to create new instance of this class when we use our extension methods
  public static class ApplicationServiceExtensions
  {
    //We are extending IServiceCollection with additional methods => that's why we have to use in parameter this.IServiceCollection services
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
      services.AddSwaggerGen(c =>
          {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
          });

      //AdddDbContext => we are getting it from entity framwork, type will be class of our database DataContext.cs
      //config will get info from appsettings.Development.json or appsettings.json => we have specified there that "DefaultConnection": "Data source=reactivities.db" 
      //For production we have switched from UseSqlite to UseNpgsql

      services.AddDbContext<DataContext>(options =>
{
  string connStr;

  var pgHost = Environment.GetEnvironmentVariable("pgHost");
  var pgPort = Environment.GetEnvironmentVariable("pgPort");
  var pgUser = Environment.GetEnvironmentVariable("pgUser");
  var pgPass = Environment.GetEnvironmentVariable("pgPass");
  var pgDb = Environment.GetEnvironmentVariable("pgDb");

  connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";


  // Whether the connection string came from the local development configuration file
  // or from the environment variable from Heroku, use it to set up your DbContext.
  options.UseNpgsql(connStr);
});



      //we have to add it to our middleware in Configure method
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

      //this will tell our mediator where to find our handlers => Matiator is nuget package
      services.AddMediatR(typeof(List.Handler).Assembly);
      // AutoMapper is nuget package that will help us with edit putHttprequest to update activity => it will help us to map object properties from one object into another
      services.AddAutoMapper(typeof(MappingProfiles).Assembly);
      //Here we are adding service that will allow us to access currently logged in user => method is defined in Security UserAccessor.cs, first we have to use interface IUserAccessor and its implementation in UserAccessor. IUserAccessor is defined in Application layer as interface. UserAccessor is defined in Infrastructure. With this service we got the ability to got our currently logged in user name from anywhere in the application as everything is connected to API layer. 
      services.AddScoped<IUserAccessor, UserAccessor>();
      // The same logic as with IUserAccessor and UserAccessor
      // PhotoAccessor is defined in Infrastrucuture project, we are accessing data in our Application layer via interface IPhotoAccessor and then connecting them here in ApplicationServiceExtensions.cs class
      services.AddScoped<IPhotoAccessor, PhotoAccessor>();
      services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));
      services.AddSignalR();

      return services;
    }
  }
}