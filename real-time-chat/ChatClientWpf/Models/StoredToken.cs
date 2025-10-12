using ChatClientWpf.Dto;

namespace ChatClientWpf.Models;


[Serializable]
public class StoredToken
{
    public string? Token { get; set; }
    public UserDto User { get; set; }
}
