namespace ChatServer.Models;

public class Session
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
