using ChatServer.Models;
using File = ChatServer.Models.File;

namespace ChatServer.Repositories.Interfaces;

public interface IFileRepository
{
    public Task CreateAsync(File file, CancellationToken cancellationToken);
    public Task<File?> GetByIdAsync(Guid fileId , CancellationToken cancellationToken);
    public Task<List<File>> GetAllAsync(CancellationToken cancellationToken);
    public Task UpdateAsync(Guid id, User file, CancellationToken cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);  
    public Task<List<File>> GetByChatIdAsync(Guid chatId, CancellationToken cancellationToken);
    public Task<List<File>> GetByMessageIdAsync(Guid messageId, CancellationToken cancellationToken);
}
