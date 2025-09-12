using ChatServer.Enums;

namespace ChatServer.Models;

public class Chat
{
    public Guid Id { get; set; }
    public ChatType Type { get; set; }
    public string? Name { get; set; }
    public List<User> Users { get; set; }
    public List<Message> Messages { get; set; }
}
