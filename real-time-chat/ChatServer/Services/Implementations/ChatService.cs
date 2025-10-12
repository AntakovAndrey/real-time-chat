using ChatServer.Dto;
using ChatServer.Repositories.Interfaces;
using ChatServer.Services.Interfaces;

namespace ChatServer.Services.Implementations;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;

    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }
    
    public async Task<List<GetChatDto>> GetChats(CancellationToken cancellationToken)
    {
        var foundChats = await _chatRepository.GetAllAsync(cancellationToken);
        var result = foundChats.Select(x => new GetChatDto
        {
            Id = x.Id,
            Name = x.Name,
            Type = x.Type,
            Users = x.Users?.Select(user => new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            }).ToList() ?? new List<UserDto>(),
            Messages = x.Messages?.Select(message =>
                {
                    if (message.UserSent != null)
                        return new GetMessageDto
                        {
                            Id = message.Id,
                            Content = message.Content,
                            Files = message.Files,
                            Date = message.Date,
                            ChatId = message.ChatId,
                            UserSentId = message.UserSentId,
                            UserSent = new UserDto
                            {
                                Id = message.UserSentId,
                                Email = message.UserSent.Email,
                                Username = message.UserSent.Username,
                            }
                        };
                    return null;
                })
                .Where(msg => msg != null)
                .ToList() ?? new List<GetMessageDto>()
        }).ToList();
        return result;
    }

    public async Task<List<GetChatDto>> GetChatsByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var foundChats = await _chatRepository.GetByUserIdAsync(userId, cancellationToken);
        var result = foundChats.Select(x => new GetChatDto
        {
            Id = x.Id,
            Name = x.Name,
            Type = x.Type,
            Users = x.Users?.Select(user => new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            }).ToList() ?? new List<UserDto>(),
            Messages = x.Messages?.Select(message =>
                {
                    if (message.UserSent != null)
                        return new GetMessageDto
                        {
                            Id = message.Id,
                            Content = message.Content,
                            Files = message.Files,
                            Date = message.Date,
                            ChatId = message.ChatId,
                            UserSentId = message.UserSentId,
                            UserSent = new UserDto
                            {
                                Id = message.UserSentId,
                                Email = message.UserSent.Email,
                                Username = message.UserSent.Username,
                            }
                        };
                    return null;
                })
                .Where(msg => msg != null)
                .ToList() ?? new List<GetMessageDto>()
        }).ToList();
        return result;
    }
}
