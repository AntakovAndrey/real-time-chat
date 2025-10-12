using ChatServer.DbContext;
using ChatServer.Models;
using ChatServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.Repositories.Implementations;

public class MessageRepository : IMessageRepository
{
    private readonly IAppDbContext _dbContext;

    public MessageRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateAsync(Message message, CancellationToken cancellationToken)
    {
        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Message?> GetByIdAsync(Guid messageId, CancellationToken cancellationToken)
    {
        try
        {
            var message = await _dbContext.Messages
                .Include(m=>m.UserSent)
                .FirstAsync(u=>u.Id == messageId, cancellationToken);
            return message;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<List<Message>> GetAllAsync(CancellationToken cancellationToken)
    {
        var foundMessages = await _dbContext.Messages
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return foundMessages;
    }

    public async Task UpdateAsync(Guid id, Message message, CancellationToken cancellationToken)
    {
        await _dbContext.Messages
            .Where(m => m.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(m=>m.Content, message.Content)
                .SetProperty(m=>m.Date, message.Date),
                cancellationToken
            );
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _dbContext.Messages
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId, CancellationToken cancellationToken)
    {
        var foundMessages = await _dbContext.Messages
            .AsNoTracking()
            .Include(c=>c.UserSent)
            .Where(c=>c.ChatId == chatId)
            .ToListAsync(cancellationToken);
        return foundMessages;
    }
}
