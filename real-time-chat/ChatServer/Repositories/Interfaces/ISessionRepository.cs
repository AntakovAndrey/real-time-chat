using ChatServer.Models;

namespace ChatServer.Repositories.Interfaces;

public interface ISessionRepository
{
    public Task CreateAsync(Session session, CancellationToken cancellationToken);
    public Task<Session?> GetByIdAsync(Guid sessionId , CancellationToken cancellationToken);
    public Task<List<Session>> GetAllAsync(CancellationToken cancellationToken);
    public Task UpdateAsync(Guid id, Session session, CancellationToken cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);    
}
