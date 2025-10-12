using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ChatClientWpf.Dto;
using ChatClientWpf.Dto.MessageDto;
using ChatClientWpf.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace ChatClientWpf.ViewModels;

public class ChatAreaViewModel : BaseViewModel
{
    #region Fields

    private Guid userId;
    private string _newMessage;
    private ChatItemDto _currentChat;
    private ObservableCollection<MessageDto> _messages;
    private readonly IMessageService _messageService;
    #endregion
    
    #region Constructor
    public ChatAreaViewModel(IServiceProvider serviceProvider)
    {
        _messageService = serviceProvider.GetRequiredService<IMessageService>();
        InitializeSignalR();
        _messageService.ConnectAsync();
        SendMessageCommand = new RelayCommand(async () => { await SendMessageAsync();});
    }
    #endregion
    
    #region Methods
    public async Task LoadChatAsync(ChatItemDto chat)
    {
        CurrentChat = chat;
        var messages = await _messageService.GetMessages(chat.Id);
        Messages = new ObservableCollection<MessageDto>(messages);
    }

    public async Task SendMessageAsync()
    {
        var message = new AddMessageDto
        {
            Content = NewMessage,
            Date = DateTime.Now,
            ChatId = CurrentChat.Id,
        };
        await _messageService.SendMessage(message);
    }
    
    private async void InitializeSignalR()
    {
        try
        {
            _messageService.OnMessageReceived += (message) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (message.ChatId == CurrentChat?.Id)
                    {
                        Messages.Add(message);
                        ScrollToBottom?.Invoke(this, EventArgs.Empty);
                    }
                });
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SignalR initialization error: {ex.Message}");
        }
    }
    
    #endregion
    
    #region Commands
    public ICommand SendMessageCommand;
    #endregion
    
    #region Properties
    public ChatItemDto CurrentChat
    {
        get => _currentChat;
        set
        {
            _currentChat = value;
            OnPropertyChanged();
        }
    }
    public ObservableCollection<MessageDto> Messages
    {
        get => _messages;
        set
        {
            _messages = value;
            OnPropertyChanged();
        }
    }
    public string NewMessage
    {
        get => _newMessage;
        set
        {
            _newMessage = value;
            OnPropertyChanged();
        }
    }
    #endregion
    
    #region Events
    public event EventHandler ScrollToBottom;
    #endregion
}
