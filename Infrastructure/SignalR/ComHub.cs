using System;
using Brewtal2.Infrastructure.Models;
using Brewtal2.Pid.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Brewtal2.Infrastructure.SignalR
{
    public class TestMessageDto
    {
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
    }

    public class ComHub : Hub
    {
        private readonly IConfiguration _configuration;
        private readonly IHubContext<ComHub> _hubContext;
        private readonly ICurrentUser _currentUser;
        private readonly IHttpContextAccessor _ctx;
        private readonly IMediator _mediator;

        public ComHub(IConfiguration configuration, IHubContext<ComHub> hubContext, ICurrentUser currentUser, IHttpContextAccessor ctx, IMediator mediator)
        {
            _configuration = configuration;
            _hubContext = hubContext;
            _currentUser = currentUser;
            _ctx = ctx;
            _mediator = mediator;

        }

        public async void SendMessage(TestMessageDto message)
        {
            Log.Information("Her...{0}. {1}", message?.Message?? "Empty message", _ctx.HttpContext.User.Identity.Name);
            await _hubContext.Clients.All.SendAsync("SendMessage", message);
        }

        public void SendHardwareStatus(dynamic hwStatus)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.SendAsync("HarwareStatus", (object)hwStatus);
        }

        public void UpdateTarget(UpdatePidTargetCommand command)
        {
            _mediator.Send(command);
        }

        public void SetOutput(UpdateOutputCommand command)
        {
            _mediator.Send(command);
        }

        public void NotifyBrewUpdated(int brewId)
        {
            Clients.All.SendAsync("BrewUpdated", brewId);
        }

    }
}