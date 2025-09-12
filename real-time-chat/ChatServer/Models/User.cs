namespace ChatServer.Models;

public class User
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public List<Chat> Chats { get; set; }
    public List<Session>? Sessions { get; set; }
}
