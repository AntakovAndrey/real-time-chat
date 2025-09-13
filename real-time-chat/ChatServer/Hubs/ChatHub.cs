using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Hubs;

public class ChatHub : Hub
{
    //ToDo: divide client to groups by chatId
    //ToDo: implement message history load after join chat
    public async Task Send(string message)
    {
        await Clients.Others.SendAsync("Receive", message);
    }   
}
