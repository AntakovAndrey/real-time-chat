using System.Net.Http;
using System.Net.Http.Json;
using ChatClientWpf.Dto;
using ChatClientWpf.Services.Interfaces;

namespace ChatClientWpf.Services.Implementations;

public class ApiProvider : IApiProvider
{
    private readonly HttpClient _httpClient;

    public ApiProvider()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5000/");
    }
    public async Task RegisterUser(RegisterDto registerDto)
    {
        var apiFetch = await _httpClient.PostAsJsonAsync("api/auth/register", 
            new
            {
                name = registerDto.Name,
                surname = registerDto.Surname,
                username = registerDto.Username,
                password = registerDto.Password,
                email = registerDto.Email,
            });
        apiFetch.EnsureSuccessStatusCode();
    }
}
