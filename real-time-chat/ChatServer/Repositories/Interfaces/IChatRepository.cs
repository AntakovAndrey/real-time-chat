using ChatServer.Models;

namespace ChatServer.Repositories.Interfaces;

public interface IChatRepository
{
    public Task CreateAsync(Chat chat, CancellationToken cancellationToken);
    public Task<Chat?> GetByIdAsync(Guid chatId , CancellationToken cancellationToken);
    public Task<List<Chat>> GetAllAsync(CancellationToken cancellationToken);
    public Task UpdateAsync(Guid id, Chat chat, CancellationToken cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);    
}
