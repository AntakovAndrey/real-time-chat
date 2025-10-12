using ChatServer.Dto;
using ChatServer.Models;

namespace ChatServer.Services.Interfaces;

public interface IMessageService
{
    public Task<GetMessageDto> AddMessage(AddMessageDto message);
    public Task<List<GetMessageDto>> GetMessagesByChatId(Guid chatId, CancellationToken cancellationToken, int skip = 0, int? take = null);
}
