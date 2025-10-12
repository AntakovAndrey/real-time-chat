using ChatClientWpf.Dto;
using ChatClientWpf.Models;

namespace ChatClientWpf.Services.Interfaces;

public interface IAuthService
{
    public Task<StoredToken> Login(LoginDto loginDto);
    public Task Register(RegisterDto registerDto);
}
