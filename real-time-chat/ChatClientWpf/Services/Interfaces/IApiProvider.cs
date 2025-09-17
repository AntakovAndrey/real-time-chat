using ChatClientWpf.Dto;

namespace ChatClientWpf.Services.Interfaces;

public interface IApiProvider
{
    public Task RegisterUser(RegisterDto registerDto);
}
