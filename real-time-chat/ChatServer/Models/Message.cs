namespace ChatServer.Models;

public class Message
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public DateTime Date { get; set; }
    public Guid ChatId { get; set; }
    public Chat? Chat { get; set; }
    public Guid UserSentId { get; set; }
    public User? UserSent { get; set; }
}
