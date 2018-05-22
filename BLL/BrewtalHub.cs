using Brewtal.CQRS;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Brewtal.BLL
{

    public class BrewtalHub : Hub
    {

        private readonly IMediator _mediator;

        public BrewtalHub(IMediator mediator)
        {
            _mediator = mediator;
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