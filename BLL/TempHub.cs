using Microsoft.AspNetCore.SignalR;

namespace Brewtal.BLL
{

    public class TempHub : Hub
    {
        public void Send(string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.InvokeAsync("Send", message);
        }

        public void TempUpdate(decimal temperature)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.InvokeAsync("TempUpdate", temperature);
        }

        public void PIDUpdate(dynamic pidUpdateStatus)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.InvokeAsync("TempUpdate", pidUpdateStatus);
        }
        
    }

}