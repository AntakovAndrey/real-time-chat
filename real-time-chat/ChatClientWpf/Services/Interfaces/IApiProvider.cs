using System.Windows.Documents;
using ChatClientWpf.Dto;
using ChatClientWpf.Models;

namespace ChatClientWpf.Services.Interfaces;

public interface IApiProvider
{
    public Task RegisterUser(RegisterDto registerDto);
    public Task<StoredToken> Login(LoginDto loginDto);
    
    public Task<QuickSearchResultDto> QuickSearchAsync(string searchTerm, StoredToken jwtToken);
    public Task<List<GetChatDto>> ChatSearchAsync(string searchTerm, StoredToken jwtToken);
    public Task<List<GetMessageDto>> MessageSearchAsync(string searchTerm, StoredToken jwtToken);
    public Task<List<UserDto>> UserSearchAsync(string searchTerm, StoredToken jwtToken);
    public Task<List<GetChatDto>> GetUserChats(StoredToken jwtToken);
    public Task<List<GetMessageDto>> GetMessages(Guid chatId, StoredToken jwtToken);
}
