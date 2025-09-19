using ChatServer.DbContext;
using ChatServer.Models;
using ChatServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = ChatServer.Models.File;

namespace ChatServer.Repositories.Implementations;

public class FileRepository : IFileRepository
{
    private readonly IAppDbContext _dbContext;

    public FileRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(File file, CancellationToken cancellationToken)
    {
        _dbContext.Files.Add(file);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<File?> GetByIdAsync(Guid fileId, CancellationToken cancellationToken)
    {
        try
        {
            var file = await _dbContext.Files.FirstAsync(u=>u.Id == fileId, cancellationToken);
            return file;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<List<File>> GetAllAsync(CancellationToken cancellationToken)
    {
        var foundFiles = await _dbContext.Files.AsNoTracking()
            .ToListAsync(cancellationToken);
        return foundFiles;
    }

    public async Task UpdateAsync(Guid id, File file, CancellationToken cancellationToken)
    {
        await _dbContext.Files.Where(f => f.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(f=>f.Filename, file.Filename)
                .SetProperty(f=>f.Size, file.Size),
                cancellationToken
            );
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _dbContext.Files.Where(s => s.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<File>> GetByChatIdAsync(Guid chatId, CancellationToken cancellationToken)
    {
        var foundFiles = await _dbContext.Files.Include(f=>f.Message).AsNoTracking()
            .Where(f => f.Message != null && f.Message.ChatId == chatId)
            .ToListAsync(cancellationToken);
        return foundFiles;
    }

    public async Task<List<File>> GetByMessageIdAsync(Guid messageId, CancellationToken cancellationToken)
    {
        var foundFiles = await _dbContext.Files.AsNoTracking()
            .Where(f => f.MessageId == messageId)
            .ToListAsync(cancellationToken);
        return foundFiles;
    }
}
