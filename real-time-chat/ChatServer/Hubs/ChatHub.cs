using System.Security.Claims;
using ChatServer.Dto;
using ChatServer.Services.Implementations;
using ChatServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Hubs;

public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly IChatService _chatService;
    
    public ChatHub(IMessageService messageService, IChatService chatService)
    {
        _messageService = messageService;
        _chatService = chatService;
    }

    [Authorize]
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User.FindFirstValue("id");
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        foreach (var chat in await _chatService.GetChatsByUserId(new Guid(userId), CancellationToken.None))
        {
            Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
        }
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.User.FindFirstValue("id");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
    }
    
    public async Task SendMessage(AddMessageDto messageDto)
    {
        var addedMessage = await _messageService.AddMessage(messageDto);
        await Clients.Group(messageDto.ChatId.ToString()).SendAsync("ReceiveMessage", addedMessage);
    }
}
