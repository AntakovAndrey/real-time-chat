using System.Security.Claims;
using ChatServer.Dto;
using ChatServer.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Hubs;

public class ChatHub : Hub
{
    private readonly IMessageService _messageService;

    public ChatHub(IMessageService messageService)
    {
        _messageService = messageService;
    }
        
    //ToDo: divide client to groups by chatId
    //ToDo: implement message history load after join chat
    public async Task SendMessage(Guid chatId, string content)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid userGuid))
        {
            throw new HubException("User not authenticated");
        }
        var message = new AddMessageDto
        {
            Content = content,
            Date = DateTime.UtcNow,
            ChatId = chatId,
            UserSentId = userGuid
        };
        await _messageService.AddMessage(message);
        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", message);
    }
    
    public async Task JoinChat(Guid chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        var messages = await _messageService.GetMessagesByChatId(chatId,skip:0,take:100);
        await Clients.Caller.SendAsync("ReceiveMessageHistory", messages);
    }
}
