using System.IO;
using System.Text;
using ChatClientWpf.Dto;
using ChatClientWpf.Models;
using ChatClientWpf.Services.Interfaces;

namespace ChatClientWpf.Services.Implementations;

public class JwtTokenStorage : ITokenStorage
{
    private readonly string _filePath;

    public JwtTokenStorage()
    {
        _filePath = "tokens.bin";
    }

    public async Task<StoredToken?> GetTokenAsync()
    {
        if (!File.Exists(_filePath))
            return null;

        try
        {
            await using var stream = new FileStream(
                _filePath, 
                FileMode.Open, 
                FileAccess.Read, 
                FileShare.Read, 
                4096, 
                true
            );
            using var reader = new BinaryReader(stream, Encoding.UTF8, true);
            var token = reader.ReadString();
            var userId = new Guid(reader.ReadBytes(16));
            var username = reader.ReadString();
            var email = reader.ReadString();
            return new StoredToken
            {
                Token = token,
                User = new UserDto
                {
                    Id = userId,
                    Username = username,
                    Email = email
                }
            };
        }
        catch
        {
            return null;
        }
    }

    public bool IsTokenExists()
    {
        return File.Exists(_filePath) && new FileInfo(_filePath).Length > 0;
    }

    public async Task SetTokenAsync(StoredToken token)
    {
        await using var stream = new FileStream(
            _filePath, 
            FileMode.Create, 
            FileAccess.Write, 
            FileShare.None, 
            4096, 
            true
        );
        using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
        writer.Write(token.Token);
        writer.Write(token.User.Id.ToByteArray());
        writer.Write(token.User.Username);
        writer.Write(token.User.Email);
    }

    public Task ClearTokenAsync()
    {
        if (File.Exists(_filePath))
            File.Delete(_filePath);
            
        return Task.CompletedTask;
    }
}
