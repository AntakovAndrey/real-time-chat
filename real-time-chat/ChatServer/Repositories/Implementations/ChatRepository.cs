using ChatServer.DbContext;
using ChatServer.Models;
using ChatServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.Repositories.Implementations;

public class ChatRepository : IChatRepository
{
    private readonly IAppDbContext _dbContext;

    public ChatRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateAsync(Chat chat, CancellationToken cancellationToken)
    {
        _dbContext.Chats.Add(chat);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Chat?> GetByIdAsync(Guid chatId, CancellationToken cancellationToken)
    {
        try
        {
            var chat = await _dbContext.Chats
                .FirstAsync(u=>u.Id == chatId, cancellationToken);
            return chat;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<List<Chat>> GetAllAsync(CancellationToken cancellationToken)
    {
        var foundChats = await _dbContext.Chats
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return foundChats;
    }

    public async Task UpdateAsync(Guid id, Chat chat, CancellationToken cancellationToken)
    {
        await _dbContext.Chats
            .Where(c => c.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(c=>c.Name, chat.Name)
                .SetProperty(c=>c.Users, chat.Users),
                cancellationToken
            );
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _dbContext.Chats
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Chat>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var foundChats = await _dbContext.Chats
            .Include(c=>c.Users)
            .Include(c=>c.Messages)
            .ThenInclude(m=>m.UserSent)
            .AsNoTracking()
            .Where(x=>x.Users.Any(u=>u.Id == userId))
            .ToListAsync(cancellationToken);
        return foundChats;
    }
}
