using ChatClientWpf.Enums;

namespace ChatClientWpf.Dto;

public class ChatItemDto
{
    public Guid Id { get; set; }
    public ChatType Type { get; set; }
    public string? Name { get; set; }
    public List<UserDto> Users { get; set; }
    public List<GetMessageDto> Messages { get; set; }
}
