using Microsoft.AspNetCore.SignalR;

namespace Brewtal.BLL
{

    public class BrewtalHub : Hub
    {
        public void PIDUpdate(dynamic pidUpdateStatus)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.InvokeAsync("PIDUpdate", pidUpdateStatus);
        }

    }

}