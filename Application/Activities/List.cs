using System.Collections.Generic;
using System.Threading;
using MediatR;
using System.Threading.Tasks;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Interfaces;

namespace Application.Activities
{
  // Follows Clean Architecture and CQRS.
  // Query and Handler are nested to keep related logic together and align with MediatR conventions.

  public class List
  {
    public class Query : IRequest<Result<List<ActivityDto>>> { }


    public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
    {
      private readonly DataContext _context;
      private readonly IMapper _mapper;
      private readonly IUserAccessor _userAccessor;

      public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
      {
        this._userAccessor = userAccessor;
        this._mapper = mapper;
        this._context = context;
      }

      // Learning info: this method handles fetching a list of activities.
      // Previously, this logic existed in the API controller.
      // It's now moved to the Application layer to follow Clean Architecture and separate business logic from the API layer.
      public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
      {
        // ProjectTo returns ActivityDto directly, so no need to manually map with _mapper.Map

        // Learning info: in earlier versions, we used Include and ThenInclude to eagerly load related entities:
        // var activities = await _context.Activities
        //     .Include(a => a.Attendees)
        //     .ThenInclude(u => u.AppUser)
        //     .ToListAsync(cancellationToken);
        //
        // While functional, this approach was less efficient and pulled more data than needed.

        // Instead, we now use AutoMapper's ProjectTo to map directly to ActivityDto.
        // This allows us to shape the data in the query itself for better performance.
        // We also pass in the current username to the mapping context, which is used inside mapping configuration if needed.
        var activities = await _context.Activities.ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUserName() }).ToListAsync(cancellationToken);

        return Result<List<ActivityDto>>.Success(activities);
      }
    }
  }
}
