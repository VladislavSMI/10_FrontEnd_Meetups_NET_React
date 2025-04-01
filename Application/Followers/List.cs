using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
  public class List
  {
    public class Query : IRequest<Result<List<Profiles.Profile>>>
    {
      public string Predicate { get; set; }
      public string Username { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<Profiles.Profile>>>
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

      public async Task<Result<List<Profiles.Profile>>> Handle(Query request, CancellationToken cancellationToken)
      {
        // We have to return list of profiles based on whether this is a user following a user of followed by a user
        var profiles = new List<Profiles.Profile>();

        switch (request.Predicate)
        {
          // in case of follower we are not interested in target user we are interested in observer
          case "followers":
            profiles = await _context.UserFollowings.Where(x => x.Target.UserName == request.Username)
              .Select(u => u.Observer)
              .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUserName() })
              .ToListAsync();
            break;
          case "following":
            profiles = await _context.UserFollowings.Where(x => x.Observer.UserName == request.Username)
              .Select(u => u.Target)
              .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUserName() })
              .ToListAsync();
            break;
        }

        return Result<List<Profiles.Profile>>.Success(profiles);
      }
    }
  }
}