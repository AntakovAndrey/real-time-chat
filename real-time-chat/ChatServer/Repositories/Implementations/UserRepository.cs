using ChatServer.DbContext;
using ChatServer.Models;
using ChatServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly IAppDbContext _dbContext;

    public UserRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _dbContext.Users.FirstAsync(u=>u.Id == userId, cancellationToken);
            return user;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        var foundUsers = await _dbContext.Users.AsNoTracking()
            .ToListAsync(cancellationToken);
        return foundUsers;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _dbContext.Users.Where(s => s.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Guid id, User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.Where(u => u.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s=>s.Name, user.Name)
                .SetProperty(s=>s.Surname, user.Surname)
                .SetProperty(s=>s.Email, user.Email)
                .SetProperty(s=>s.Password, user.Password)
                .SetProperty(s=>s.Username, user.Username),
                cancellationToken
            );
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
