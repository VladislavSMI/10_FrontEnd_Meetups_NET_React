using System.Collections.Generic;
using System.Threading;
using Domain;
using MediatR;
using System.Threading.Tasks;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;

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

      public Handler(DataContext context, IMapper mapper)
      {
        this._mapper = mapper;
        this._context = context;
      }

      // This logic was previously in the API controller.
      // It's now in the Application layer to follow Clean Architecture and separate business concerns.
      public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
      {
        // ProjectTo returns ActivityDto directly, so no need to manually map with _mapper.Map

        var activities = await _context.Activities.ProjectTo<ActivityDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        return Result<List<ActivityDto>>.Success(activities);
      }
    }
  }
}
