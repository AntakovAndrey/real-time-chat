using ChatClientWpf.Enums;

namespace ChatClientWpf.Dto;

public class SearchResultItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public SearchResultType Type { get; set; }
    public string Avatar { get; set; }
}
