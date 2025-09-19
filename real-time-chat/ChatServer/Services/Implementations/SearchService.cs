using ChatServer.Dto;
using ChatServer.Repositories.Interfaces;
using ChatServer.Services.Interfaces;

namespace ChatServer.Services.Implementations;

public class SearchService : ISearchService
{
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;

    public SearchService(IUserRepository userRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository)
    {
        _userRepository = userRepository;
        _chatRepository = chatRepository;
    }
    
    public async Task<QuickSearchResultDto> QuickSearch(string searchTerm, Guid userId, CancellationToken cancellationToken)
    {
        var users = (await _userRepository.GetAllAsync(cancellationToken))
            .Where(user => user.Username.ToLower().Contains(searchTerm.ToLower())
                            || user.Email.ToLower().Contains(searchTerm.ToLower())
                            || user.Name.ToLower().Contains(searchTerm.ToLower())
                            || user.Surname.ToLower().Contains(searchTerm.ToLower())
            ).Take(5).Select(x => new UserDto
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email
            }).ToList();
        var chats = (await _chatRepository.GetByUserIdAsync(userId ,cancellationToken))
            .Where(x => x.Name != null && x.Name.ToLower().Contains(searchTerm.ToLower())).Take(5)
            .Select(x=>new GetChatDto
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Users = x.Users,
                Messages = x.Messages,
            }).ToList();
        var messages = (await _chatRepository.GetByUserIdAsync(userId ,cancellationToken))
            .SelectMany(x=>x.Messages)
            .Where(x=>x.Content != null && x.Content.ToLower().Contains(searchTerm.ToLower()))
            .Take(5)
            .Select(x=> new GetMessageDto
            {
                Id = x.Id,
                Content = x.Content,
                Files = x.Files,
                Date = x.Date,
                ChatId = x.ChatId,
                UserSentId = x.UserSentId
            }).ToList();
        var searchResult = new QuickSearchResultDto
        {
            Users = users,
            Chats = chats,
            Messages = messages
        };
        return searchResult;
    }

    public async Task<List<GetChatDto>> ChatSearch(string searchTerm, Guid userId, CancellationToken cancellationToken)
    {
        var chats = (await _chatRepository.GetByUserIdAsync(userId ,cancellationToken))
            .Where(x => x.Name != null && x.Name.ToLower().Contains(searchTerm.ToLower()))
            .Select(x=>new GetChatDto
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Users = x.Users,
                Messages = x.Messages,
            }).ToList();
        return chats;
    }

    public async Task<List<UserDto>> UserSearch(string searchTerm, Guid userId, CancellationToken cancellationToken)
    {
        var users = (await _userRepository.GetAllAsync(cancellationToken))
            .Where(user => user.Username.ToLower().Contains(searchTerm.ToLower())
                           || user.Email.ToLower().Contains(searchTerm.ToLower())
                           || user.Name.ToLower().Contains(searchTerm.ToLower())
                           || user.Surname.ToLower().Contains(searchTerm.ToLower()))
            .Select(x => new UserDto
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email
            }).ToList();
        return users;
    }

    public async Task<List<GetMessageDto>> MessageSearch(string searchTerm, Guid userId, CancellationToken cancellationToken)
    {
        var messages = (await _chatRepository.GetByUserIdAsync(userId ,cancellationToken))
            .SelectMany(x=>x.Messages)
            .Where(x=>x.Content != null && x.Content.ToLower().Contains(searchTerm.ToLower()))
            .Select(x=> new GetMessageDto
            {
                Id = x.Id,
                Content = x.Content,
                Files = x.Files,
                Date = x.Date,
                ChatId = x.ChatId,
                UserSentId = x.UserSentId
            }).ToList();
        return messages;
    }
}
