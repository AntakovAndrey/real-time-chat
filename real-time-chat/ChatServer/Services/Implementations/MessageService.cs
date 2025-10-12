using ChatServer.Dto;
using ChatServer.Models;
using ChatServer.Repositories.Interfaces;
using ChatServer.Services.Interfaces;

namespace ChatServer.Services.Implementations;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;

    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    public async Task<GetMessageDto> AddMessage(AddMessageDto message)
    {
        var newMessage = new Message
        {
            ChatId = message.ChatId,
            Content = message.Content,
            Date = message.Date,
            UserSentId = message.UserSentId
        };
        await _messageRepository.CreateAsync(newMessage, CancellationToken.None);
        var addedMessage = await _messageRepository.GetByIdAsync(newMessage.Id, CancellationToken.None);
        var result = new GetMessageDto
        {
            Id = addedMessage.Id,
            Content = addedMessage.Content,
            Files = addedMessage.Files,
            Date = addedMessage.Date,
            ChatId = addedMessage.ChatId,
            UserSent = new UserDto
            {
                Id = addedMessage.UserSentId,
                Email = addedMessage.UserSent.Email,
                Username = addedMessage.UserSent.Username,
            },
            UserSentId = addedMessage.UserSentId
        };
        return result;
    }

    public async Task<List<GetMessageDto>> GetMessagesByChatId(Guid chatId, CancellationToken cancellationToken, int skip = 0, int? take = null)
    {
        var messages = await _messageRepository.GetMessagesByChatIdAsync(chatId, cancellationToken);
        var result = messages
            .Select(x=> new GetMessageDto
            {
                Id = x.Id,
                Content = x.Content,
                Files = x.Files,
                Date = x.Date,
                ChatId = x.ChatId,
                UserSent = new UserDto
                {
                    Id = x.UserSentId,
                    Email = x.UserSent.Email,
                    Username = x.UserSent.Username,
                },
                UserSentId = x.UserSentId
            }).ToList();
        return result;
    }
}
