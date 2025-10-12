namespace ChatServer.Dto;

public class AddMessageDto
{
    public string? Content { get; set; }
    public DateTime Date { get; set; }
    public Guid ChatId { get; set; }
    public Guid UserSentId { get; set; }
}
