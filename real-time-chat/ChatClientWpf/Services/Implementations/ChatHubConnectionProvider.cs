using ChatClientWpf.Configuration;
using ChatClientWpf.Dto;
using ChatClientWpf.Dto.MessageDto;
using ChatClientWpf.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace ChatClientWpf.Services.Implementations;

public class ChatHubConnectionProvider : IChatHubConnectionProvider
{
    private readonly HubConnection _connection;
    private readonly ITokenStorage _tokenStorage;
    
    public ChatHubConnectionProvider(ITokenStorage tokenStorage, IOptions<ApiConfiguration> apiConfiguration)
    {
        _tokenStorage = tokenStorage;
        _connection = new HubConnectionBuilder()
            .WithUrl(apiConfiguration.Value.Url, options =>
                options.AccessTokenProvider = () => 
                    Task.FromResult(tokenStorage.GetTokenAsync().Result.Token))
            .WithAutomaticReconnect()
            .Build();
        ConfigureEvents();
        _ = ConnectAsync();
    }

    private void ConfigureEvents()
    {
        _connection.On<GetMessageDto>("ReceiveMessage", receivedMessage =>
            {
                var token =  _tokenStorage.GetTokenAsync().Result;
                var message = new MessageDto
                {
                    Id = receivedMessage.Id,
                    Content = receivedMessage.Content,
                    SentAt = receivedMessage.Date,
                    ChatId = receivedMessage.ChatId,
                    IsMine = receivedMessage.UserSentId.Equals(token.User.Id),
                    SenderName = receivedMessage.UserSent.Username,
                    MessageType = MessageType.Text,
                    SenderId = receivedMessage.UserSentId
                };
                OnMessageReceived?.Invoke(message);
            }
        );
    }
    public async Task ConnectAsync()
    {
        try
        {
            await _connection.StartAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
    
    public void Dispose()
    {
        _connection.DisposeAsync();
    }
    
    public HubConnection Connection { get=> _connection; }
    
    public event Action<MessageDto>? OnMessageReceived;
}