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
using System.Linq;

namespace Application.Activities
{
  // Follows Clean Architecture and CQRS.
  // Query and Handler are nested to keep related logic together and align with MediatR conventions.

  public class List
  {
    public class Query : IRequest<Result<PagedList<ActivityDto>>>
    {
      public ActivityParams Params { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
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

      //This was implemented by IRequestHandler interface
      public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
      {

        var query = _context.Activities
        .Where(d => d.Date >= request.Params.StartDate)
        .OrderBy(d => d.Date)
        .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
          new { currentUsername = _userAccessor.GetUserName() })
        .AsQueryable();

        // We have to set up that filtering is used only for currently logged in user
        if (request.Params.IsGoing && !request.Params.IsHost)
        {
          query = query.Where(x => x.Attendees.Any(a => a.UserName == _userAccessor.GetUserName()));
        }

        if (request.Params.IsHost && !request.Params.IsGoing)
        {
          query = query.Where(x => x.HostUsername == _userAccessor.GetUserName());
        }
        return Result<PagedList<ActivityDto>>.Success(
          await PagedList<ActivityDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
        );
      }
    }
  }
}
