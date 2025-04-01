using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities

{
  public class UpdateAttendance
  {
    public class Command : IRequest<Result<Unit>>
    {
      public Guid Id { get; set; }
    }

    // This handler serves 3 purposes:
    // 1. If the user is attending the event but is not the host, they are removed from the activity.
    // 2. If the user is not attending the event, they are added to the activity.
    // 3. If the user is the host, the activity is canceled.
    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
      private readonly DataContext _context;
      private readonly IUserAccessor _userAccessor;
      public Handler(DataContext context, IUserAccessor userAccessor)
      {
        this._userAccessor = userAccessor;
        this._context = context;
      }
      public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
      {
        var activity = await _context.Activities
        .Include(a => a.Attendees).ThenInclude(u => u.AppUser)
        .SingleOrDefaultAsync(x => x.Id == request.Id);

        if (activity == null) return null;

        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

        if (user == null) return null;

        var hostUsername = activity.Attendees.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

        var attendance = activity.Attendees.FirstOrDefault(x => x.AppUser.UserName == user.UserName);

        var isHost = attendance != null && hostUsername == user.UserName;

        if (isHost)
        {
          activity.IsCancelled = !activity.IsCancelled;
        }

        if (attendance != null && hostUsername != user.UserName)
        {
          activity.Attendees.Remove(attendance);
        }

        if (attendance == null)
        {
          attendance = new ActivityAttendee
          {
            AppUser = user,
            Activity = activity,
            IsHost = false
          };

          activity.Attendees.Add(attendance);
        }

        var result = await _context.SaveChangesAsync() > 0;

        return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating attendance");

      }
    }
  }
}