using ChatServer.DbContext;
using ChatServer.Models;
using ChatServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.Repositories.Implementations;

public class SessionRepository : ISessionRepository
{
    private readonly IAppDbContext _dbContext;

    public SessionRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateAsync(Session session, CancellationToken cancellationToken)
    {
        _dbContext.Sessions.Add(session);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Session?> GetByIdAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        try
        {
            var session = await _dbContext.Sessions.FirstAsync(u=>u.Id == sessionId, cancellationToken);
            return session;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<List<Session>> GetAllAsync(CancellationToken cancellationToken)
    {
        var foundSessions = await _dbContext.Sessions.AsNoTracking()
            .ToListAsync(cancellationToken);
        return foundSessions;
    }

    public async Task UpdateAsync(Guid id, Session session, CancellationToken cancellationToken)
    {
        await _dbContext.Sessions.Where(s => s.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s=>s.UserId, session.UserId)
                .SetProperty(s=>s.StartTime, session.StartTime)
                .SetProperty(s=>s.EndTime, session.EndTime),
                cancellationToken
            );
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _dbContext.Sessions.Where(s => s.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
