using ChatClientWpf.Dto;
using ChatClientWpf.Services.Interfaces;

namespace ChatClientWpf.Services.Implementations;

public class ChatService : IChatService
{
    private readonly IApiProvider _apiProvider;
    private readonly ITokenStorage _tokenStorage;

    public ChatService(IApiProvider apiProvider, ITokenStorage tokenStorage)
    {
        _apiProvider = apiProvider;
        _tokenStorage = tokenStorage;
    }
    
    public async Task<List<GetChatDto>> GetUserChats()
    {
        var token = await _tokenStorage.GetTokenAsync();
        var foundChats = await _apiProvider.GetUserChats(token);
        return foundChats;
    }
}
