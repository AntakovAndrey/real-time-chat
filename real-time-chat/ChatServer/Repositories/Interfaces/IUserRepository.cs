using ChatServer.Models;

namespace ChatServer.Repositories.Interfaces;

public interface IUserRepository
{
    public Task CreateAsync(User user, CancellationToken cancellationToken);
    public Task<User?> GetByIdAsync(Guid userId , CancellationToken cancellationToken);
    public Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
    public Task UpdateAsync(Guid id, User user, CancellationToken cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
