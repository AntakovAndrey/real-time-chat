using ChatClientWpf.Dto;
using ChatClientWpf.Dto.MessageDto;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatClientWpf.Services.Interfaces;

public interface IMessageService : IDisposable
{
    public Task<List<MessageDto>> GetMessages(Guid chatId);
    public Task SendMessage(AddMessageDto message);
    public Task ConnectAsync();
    
    public event Action<MessageDto> OnMessageReceived;
}
