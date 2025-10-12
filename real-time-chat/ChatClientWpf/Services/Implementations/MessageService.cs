using ChatClientWpf.Configuration;
using ChatClientWpf.Dto;
using ChatClientWpf.Dto.MessageDto;
using ChatClientWpf.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace ChatClientWpf.Services.Implementations;

public class MessageService : IMessageService
{
    #region Fields
    private readonly IApiProvider _apiProvider;
    private readonly ITokenStorage _tokenStorage;
    private readonly HubConnection _connection;
    #endregion
    
    #region Constructor
    public MessageService(IApiProvider apiProvider, ITokenStorage tokenStorage, IOptions<ApiConfiguration> apiConfiguration)
    {
        _apiProvider = apiProvider;
        _tokenStorage = tokenStorage;
        _connection = new HubConnectionBuilder()
            .WithUrl(apiConfiguration.Value.Url, options =>
                options.AccessTokenProvider = () => 
                    Task.FromResult(tokenStorage.GetTokenAsync().Result.Token))
            .WithAutomaticReconnect()
            .Build();
        ConfigureEvents();
    }
    #endregion
    
    #region Methods
    public async Task<List<MessageDto>> GetMessages(Guid chatId)
    {
        var token = await _tokenStorage.GetTokenAsync();
        var foundMessages = await _apiProvider.GetMessages(chatId, token);
        var result = foundMessages.Select(x =>
            new MessageDto
            {
                Id = x.Id,
                Content = x.Content,
                SentAt = x.Date,
                ChatId = x.ChatId,
                IsMine = x.UserSentId.Equals(token.User.Id),
                SenderName = x.UserSent.Username,
                MessageType = MessageType.Text,
                SenderId = x.UserSentId
            })
            .OrderBy(x => x.SentAt)
            .ToList();
        return result;
    }
    
    public async Task SendMessage(AddMessageDto message)
    {
        try
        {
            message.UserSentId = (await _tokenStorage.GetTokenAsync()).User.Id;
            await _connection.InvokeAsync("SendMessage", message);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
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
    #endregion
    
    #region Events
    public event Action<MessageDto>? OnMessageReceived;
    #endregion
}
