using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
  public class ListActivities
  {
    public class Query : IRequest<Result<List<UserActivityDto>>>
    {
      public string Username { get; set; }
      public string Predicate { get; set; }
    }


    public class Handler : IRequestHandler<Query, Result<List<UserActivityDto>>>
    {
      private readonly DataContext _context;
      private readonly IMapper _mapper;
      public Handler(DataContext context, IMapper mapper)
      {
        this._mapper = mapper;
        this._context = context;
      }

      public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
      {
        var query = _context.ActivityAttendees
            .Where(u => u.AppUser.UserName == request.Username)
            .OrderBy(a => a.Activity.Date)
            .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider).AsQueryable();
        // asQueryable means that we not executing anything to the database

        query = request.Predicate switch
        {
          "past" => query.Where(a => a.Date <= DateTime.Now),
          "hosting" => query.Where(a => a.HostUsername == request.Username),
          "future" => query.Where(a => a.Date >= DateTime.Now),
          _ => query
        };

        var activities = await query.ToListAsync();

        return Result<List<UserActivityDto>>.Success(activities);



      }
    }
  }
}