using Microsoft.AspNetCore.SignalR.Client;

namespace ChatClientWpf.Services.Interfaces;

public interface IHubConnector
{
    public HubConnection Connection { get; }
    public Task ConnectAsync();
    public Task DisconnectAsync();
    public Task SendMessageAsync(Guid chatId, string message);
}
