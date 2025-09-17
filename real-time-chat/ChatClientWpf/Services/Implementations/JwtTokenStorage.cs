using System.IO;
using ChatClientWpf.Services.Interfaces;

namespace ChatClientWpf.Services.Implementations;

public class JwtTokenStorage : ITokenStorage
{
    private readonly String _filePath;

    public JwtTokenStorage()
    {
        _filePath = "tokens.txt";
    }
    public async Task<string?> GetToken()
    {
        var token = await File.ReadAllTextAsync(_filePath);
        return token;
    }

    public async Task<bool> IsTokenExistsAsync()
    {
        if(!File.Exists(_filePath))
            return false;
        if((await File.ReadAllTextAsync(_filePath)).Length > 0)
            return true;
        return false;
    }

    public async Task SetToken(string token)
    {
        await File.WriteAllTextAsync(_filePath, token);
    }

    public async Task ClearToken()
    {
        await File.WriteAllTextAsync(_filePath, "");
    }
}
