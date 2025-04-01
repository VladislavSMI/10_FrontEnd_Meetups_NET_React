using System;
using API.Extensions;
using API.Middleware;
using API.SignalR;
using Application.Activities;
using Domain;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// 1st add services to container
builder.Services.AddControllers(opt =>
 {
   var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
   opt.Filters.Add(new AuthorizeFilter(policy));
 }
 ).AddFluentValidation(config =>
 {
   config.RegisterValidatorsFromAssemblyContaining<Create>();
 });
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

// 2nd Configure the http request pipeline
var app = builder.Build();

// We have created this custom middleware to help us handle exception eg. in Delete.cs when activity to delete doesn't exist
app.UseMiddleware<ExceptionMiddleware>();

// Security settings via package NWebsec.AspNetCore.Middleware
app.UseXContentTypeOptions();
app.UseReferrerPolicy(opt => opt.NoReferrer());
app.UseXXssProtection(opt => opt.EnabledWithBlockMode());
app.UseXfo(opt => opt.Deny());
app.UseCsp(opt => opt
      .BlockAllMixedContent()
      .StyleSources(s => s.Self().CustomSources("https://fonts.googleapis.com", "https://cdn.jsdelivr.net"))
      .FontSources(s => s.Self().CustomSources("https://fonts.gstatic.com", "https://cdn.jsdelivr.net", "data:"))
      .FormActions(s => s.Self())
      .FrameAncestors(s => s.Self())
// .ImageSources(s => s.Self().CustomSources("https://res.cloudinary.com", "https://reactivities-testing.herokuapp.com"))
// .ScriptSources(s => s.Self())
);

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
}

else
{
  app.Use(async (context, next) =>
  {
    context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");
    await next.Invoke();
  });
}

//React app build
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("CorsPolicy");

// UserAuthentication has to go before UseAuthorization
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapHub<ChatHub>("/chat");

// React routes endpoint
app.MapFallbackToController("Index", "Fallback");

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// Check if database and if not create database and apply any migrations to it
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
  var context = services.GetRequiredService<DataContext>();
  var userManager = services.GetRequiredService<UserManager<AppUser>>();
  await context.Database.MigrateAsync();
  await Seed.SeedData(context, userManager);

}
catch (Exception ex)
{
  var logger = services.GetRequiredService<ILogger<Program>>();
  logger.LogError(ex, "An error occured during migration");
}

await app.RunAsync();
