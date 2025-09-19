using ChatServer.Models;

namespace ChatServer.Repositories.Interfaces;

public interface IMessageRepository
{
    public Task CreateAsync(Message message, CancellationToken cancellationToken);
    public Task<Message?> GetByIdAsync(Guid messageId , CancellationToken cancellationToken);
    public Task<List<Message>> GetAllAsync(CancellationToken cancellationToken);
    public Task UpdateAsync(Guid id, Message message, CancellationToken cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);    
    public Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId, CancellationToken cancellationToken);
}
