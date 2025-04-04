using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
  public class IsHostRequirement : IAuthorizationRequirement
  {
  }

  public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
  {
    private readonly DataContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public IsHostRequirementHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
      this._httpContextAccessor = httpContextAccessor;
      this._dbContext = dbContext;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
    {

      var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

      // If userId is null then we are not logged in so we don't need to check anything
      if (userId == null) return Task.CompletedTask;

      var activityId = Guid.Parse(_httpContextAccessor.HttpContext?.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value?.ToString());

      // We have to implement AsNoTracking as without it we are tracking our var attendee in memory and it is causing problems, AsNoTracking is not working with FindAsync so we have to user SingleOrDefaultAsync
      var attendee = _dbContext.ActivityAttendees
      .AsNoTracking()
      .SingleOrDefaultAsync(x => x.AppUserId == userId && x.ActivityId == activityId).Result;

      if (attendee == null) return Task.CompletedTask;
      if (attendee.IsHost) context.Succeed(requirement);

      return Task.CompletedTask;
    }
  }
}