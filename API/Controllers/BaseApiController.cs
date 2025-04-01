using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class BaseApiController : ControllerBase
  {
    // Initializes Mediator in the base controller so that all inheriting controllers can access it without redefining it
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();


    protected ActionResult HandleResult<T>(Result<T> result)
    {
      if (result == null)
      {
        return NotFound();
      }
      if (result.IsSuccess && result.Value != null)
      {
        return Ok(result.Value);
      }
      if (result.IsSuccess && result.Value == null)
      {
        return NotFound();
      }
      return BadRequest(result.Error);
    }
  }
}