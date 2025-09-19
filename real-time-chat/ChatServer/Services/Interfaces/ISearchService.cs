using ChatServer.Dto;

namespace ChatServer.Services.Interfaces;

public interface ISearchService
{
    public Task<QuickSearchResultDto> QuickSearch(string searchTerm, Guid userId, CancellationToken cancellationToken);
    public Task<List<GetChatDto>> ChatSearch(string searchTerm, Guid userId, CancellationToken cancellationToken);
    public Task<List<UserDto>> UserSearch(string searchTerm, Guid userId, CancellationToken cancellationToken);
    public Task<List<GetMessageDto>> MessageSearch(string searchTerm, Guid userId, CancellationToken cancellationToken);
}
