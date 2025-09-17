using ChatClientWpf.Dto;
using ChatClientWpf.Exceptions;
using ChatClientWpf.Services.Interfaces;

namespace ChatClientWpf.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IApiProvider _apiProvider;

    public AuthService(IApiProvider apiProvider)
    {
        _apiProvider = apiProvider;
    }
    
    public async Task<string> Login(string username, string password)
    {
        throw new NotImplementedException();
    }

    public async Task Register(RegisterDto registerDto)
    {
        try
        {
            await _apiProvider.RegisterUser(registerDto);
        }
        catch (ServerNotRespondsException ex)
        {
            throw;
        }
        
    }
}
