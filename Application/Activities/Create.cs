using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
  public class Create
  {
    // Command doesn't return data so that's why IRequest is without return type
    public class Command : IRequest
    {
      public Activity Activity { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
      public CommandValidator()
      {
        RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
      }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
      private readonly DataContext _context;

      public Handler(DataContext context)
      {
        this._context = context;
      }

      public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
      {
        _context.Activities.Add(request.Activity);

        var result = await _context.SaveChangesAsync() > 0;

        // This returns Unit, which is way of saying "command completed, but no result
        if (!result) return Result<Unit>.Failure("Failed to create activity");
        return Result<Unit>.Success(Unit.Value);
      }
    }

  }

}