using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
  public class Delete
  {
    // Learning info: returning Unit because we don't need to return anything
    public class Command : IRequest<Result<Unit>>
    {
      public string Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
      private readonly DataContext _context;
      private readonly IPhotoAccessor _photoAccessor;
      private readonly IUserAccessor _usesAccessor;
      public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor usesAccessor)
      {
        this._usesAccessor = usesAccessor;
        this._photoAccessor = photoAccessor;
        this._context = context;
      }

      public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
      {
        var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.UserName == _usesAccessor.GetUserName());

        if (user == null) return null;

        // Learning info: we are not using here async because we have already retrieved User from our db and included photo collection so in memory we should have access to the Photos
        var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

        if (photo == null) return null;

        if (photo.IsMain) return Result<Unit>.Failure("You cannot delete your main photo");

        var result = await _photoAccessor.DeletePhoto(photo.Id);

        if (result == null) return Result<Unit>.Failure("Problem deleting photo from Cloudinary");

        user.Photos.Remove(photo);

        var success = await _context.SaveChangesAsync() > 0;

        if (success) return Result<Unit>.Success(Unit.Value);

        return Result<Unit>.Failure("Problem deleting photo");
      }
    }
  }
}