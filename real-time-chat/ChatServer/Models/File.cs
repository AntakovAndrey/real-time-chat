namespace ChatServer.Models;

public class File
{
    public Guid Id { get; set; }
    public required string Filename { get; set; }
    public double Size { get; set; }
    public Guid MessageId { get; set; }
    public Message? Message { get; set; }    
}
