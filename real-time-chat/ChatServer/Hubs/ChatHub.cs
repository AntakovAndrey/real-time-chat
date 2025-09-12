using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Hubs;

public class ChatHub : Hub
{
    public async Task Send(string message)
    {
        await Clients.All.SendAsync("Receive", message);
    }
}
