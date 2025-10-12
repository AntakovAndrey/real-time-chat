using ChatClientWpf.Dto;

namespace ChatClientWpf.Services.Interfaces;

public interface IChatService
{
    public Task<List<GetChatDto>> GetUserChats();
    
}
