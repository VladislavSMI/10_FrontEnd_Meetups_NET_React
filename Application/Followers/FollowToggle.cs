using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
  public class FollowToggle
  {
    public class Command : IRequest<Result<Unit>>
    {
      //Learning info: we will get observer username from the token that we will be using to authenticate the request
      public string TargetUsername { get; set; }
    }

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
        // 1st we need to get to our user that will follow the other users => user will be authenticated in this step so we will get our user from GetUserName() method
        var observer = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

        var target = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.TargetUsername);

        //We will check if we have a target as we are receiving this from our client
        if (target == null) return null;

        var following = await _context.UserFollowings.FindAsync(observer.Id, target.Id);

        if (following == null)
        {
          following = new UserFollowing
          {
            Observer = observer,
            Target = target
          };

          _context.UserFollowings.Add(following);
        }
        else
        {
          _context.UserFollowings.Remove(following);
        }

        var success = await _context.SaveChangesAsync() > 0;

        if (success) return Result<Unit>.Success(Unit.Value);

        return Result<Unit>.Failure("Failed to update following");

      }
    }
  }
}