namespace ChatClientWpf.Dto.MessageDto;

public class MessageDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public Guid SenderId { get; set; }
    public string SenderName { get; set; }
    public Guid ChatId { get; set; }
    public MessageType MessageType { get; set; } = MessageType.Text;
    //public List<FileAttachmentDto> Attachments { get; set; } = new List<FileAttachmentDto>();
    public bool IsMine { get; set; }
}

public enum MessageType
{
    Text,
    Image,
    File,
    System
}

public class FileAttachmentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public long FileSize { get; set; }
    public string ContentType { get; set; }
}