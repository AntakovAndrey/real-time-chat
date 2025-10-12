using ChatServer.Dto;

namespace ChatServer.Services.Interfaces;

public interface IChatService
{
    public Task<List<GetChatDto>> GetChats(CancellationToken cancellationToken);
    public Task<List<GetChatDto>> GetChatsByUserId(Guid userId, CancellationToken cancellationToken);
}
