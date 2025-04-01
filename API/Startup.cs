using API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;
using Application.Activities;
using API.Middleware;

namespace API
{
  public class Startup
  {
    private readonly IConfiguration _config;
    public Startup(IConfiguration config)
    {
      _config = config;
    }
    // To register services with the dependency injection container
    // This method gets called by the runtime.
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddControllers().AddFluentValidation(config =>
      {
        config.RegisterValidatorsFromAssemblyContaining<Create>();
      });
      services.AddApplicationServices(_config);

    }

    // To defined the middleware pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseMiddleware<ExceptionMiddleware>();

      if (env.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
      }

      // Redirects HTTP to HTTPS — safe to comment out during development if not using HTTPS locally
      // app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors("CorsPolicy");

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
