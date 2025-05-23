using System;
using System.Threading.Tasks;
using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
  public class ChatHub : Hub
  {
    private readonly IMediator _mediator;
    public ChatHub(IMediator mediator)
    {
      this._mediator = mediator;
    }

    public async Task SendComment(Create.Command command)
    {
      var comment = await _mediator.Send(command);

      await Clients.Group(command.ActivityId.ToString()).SendAsync("ReceiveComment", comment.Value);
    }

    public override async Task OnConnectedAsync()
    {
      // Learning info: when ever client connects, we are going to join them to the group with the name of activityId and we are going to send them list of comments that we get from our database.
      var httpContext = Context.GetHttpContext();
      var activityId = httpContext.Request.Query["activityId"];
      await Groups.AddToGroupAsync(Context.ConnectionId, activityId);
      var result = await _mediator.Send(new List.Query { ActivityId = Guid.Parse(activityId) });
      await Clients.Caller.SendAsync("LoadComments", result.Value);
    }
  }
}