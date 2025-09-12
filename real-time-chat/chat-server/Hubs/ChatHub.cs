using Microsoft.AspNetCore.SignalR;

namespace chat_server;

public class ChatHub : Hub
{
    public async Task Send(string message, CancellationToken cancellationToken)
    {
        await Clients.All.SendAsync("Receive", message, cancellationToken);
    }
        
}
