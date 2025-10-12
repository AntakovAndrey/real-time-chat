using ChatClientWpf.Dto;
using ChatClientWpf.Exceptions;
using ChatClientWpf.Models;
using ChatClientWpf.Services.Interfaces;

namespace ChatClientWpf.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IApiProvider _apiProvider;

    public AuthService(IApiProvider apiProvider)
    {
        _apiProvider = apiProvider;
    }
    
    public async Task<StoredToken> Login(LoginDto loginDto)
    {
        var token = await _apiProvider.Login(loginDto);
        return token;
    }

    public async Task Register(RegisterDto registerDto)
    {
        await _apiProvider.RegisterUser(registerDto);
    }
}
