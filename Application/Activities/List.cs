using System.Collections.Generic;
using System.Threading;
using Domain;
using MediatR;
using System.Threading.Tasks;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Core;

namespace Application.Activities
{
  // This file follows the Clean Architecture and CQRS pattern.
  // We define the Query and its Handler as nested classes inside the List class to group related logic.
  // This improves code organization and aligns with the MediatR request/handler structure.

  public class List
  {
    // Query represents a MediatR request to retrieve a list of activities.
    // In CQRS, queries return data, while commands modify state and typically return nothing.
    public class Query : IRequest<Result<List<Activity>>> { }

    // Handler processes the Query and retrieves the activities from the database.
    public class Handler : IRequestHandler<Query, Result<List<Activity>>>
    {
      private readonly DataContext _context;

      public Handler(DataContext context)
      {
        _context = context;
      }

      // Handles the Query request.
      // This logic was previously in the ActivitiesController,
      // but was moved here to follow Clean Architecture by keeping business logic in the Application layer.
      public async Task<Result<List<Activity>>> Handle(Query request, CancellationToken cancellationToken)
      {
        var activity = await _context.Activities.ToListAsync();
        return Result<List<Activity>>.Success(activity);
      }
    }
  }
}
