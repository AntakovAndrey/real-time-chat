namespace ChatServer.Dto;

public class QuickSearchResultDto
{
    public List<UserDto> Users { get; set; }
    public List<GetChatDto> Chats { get; set; }
    public List<GetMessageDto> Messages { get; set; }
}
