using ChatServer.Models;
using File = ChatServer.Models.File;

namespace ChatServer.Dto;

public class GetMessageDto
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public List<File>? Files { get; set; }
    public DateTime Date { get; set; }
    public Guid ChatId { get; set; }
    public Guid UserSentId { get; set; }
    public UserDto? UserSent { get; set; }
}
