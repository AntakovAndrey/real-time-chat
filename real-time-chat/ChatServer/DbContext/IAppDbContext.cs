using ChatServer.Models;
using Microsoft.EntityFrameworkCore;
using File = ChatServer.Models.File;

namespace ChatServer.DbContext;

public interface IAppDbContext
{
    public abstract DbSet<User> Users { get; set; }
    public abstract DbSet<Message> Messages { get; set; }
    public abstract DbSet<Chat> Chats { get; set; }
    public abstract DbSet<Session> Sessions { get; set; }
    public abstract DbSet<File> Files { get; set; }
  
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
