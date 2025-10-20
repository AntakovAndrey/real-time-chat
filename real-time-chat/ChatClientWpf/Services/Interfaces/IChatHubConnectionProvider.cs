using ChatClientWpf.Dto.MessageDto;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatClientWpf.Services.Interfaces;

public interface IChatHubConnectionProvider : IDisposable
{
    public HubConnection Connection { get; }
    
    public event Action<MessageDto>? OnMessageReceived;
}
