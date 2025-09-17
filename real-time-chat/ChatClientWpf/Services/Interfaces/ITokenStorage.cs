namespace ChatClientWpf.Services.Interfaces;

public interface ITokenStorage
{
    public Task<string?> GetToken();
    public Task<bool> IsTokenExistsAsync();
    public Task SetToken(string token);
    public Task ClearToken();
}
