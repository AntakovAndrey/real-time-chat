using ChatClientWpf.Dto;

namespace ChatClientWpf.Services.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns>Returns JWT token</returns>
    public Task<string> Login(string username, string password);
    public Task Register(RegisterDto registerDto);
}
