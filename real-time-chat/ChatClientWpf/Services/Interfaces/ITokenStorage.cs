using ChatClientWpf.Models;

namespace ChatClientWpf.Services.Interfaces;

public interface ITokenStorage
{
    public Task<StoredToken?> GetTokenAsync();
    public bool IsTokenExists();
    public Task SetTokenAsync(StoredToken token);
    public Task ClearTokenAsync();
}
