using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ChatClientWpf.Configuration;
using ChatClientWpf.Dto;
using ChatClientWpf.Models;
using ChatClientWpf.Services.Interfaces;
using Microsoft.Extensions.Options;
using InvalidOperationException = System.InvalidOperationException;

namespace ChatClientWpf.Services.Implementations;

public class ApiProvider : IApiProvider
{
    private readonly HttpClient _httpClient;

    public ApiProvider(IOptions<ApiConfiguration> apiConfiguration)
    {
        _httpClient = new HttpClient();
        var apiConfig = apiConfiguration.Value;
        _httpClient.BaseAddress = new Uri(apiConfig.Url);
    }

    #region api/auth
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

    public async Task<StoredToken> Login(LoginDto loginDto)
    {
        var apiFetch = await _httpClient.PostAsJsonAsync("api/auth/login", 
            new
            {
                username = loginDto.Username,
                password = loginDto.Password
            });
        apiFetch.EnsureSuccessStatusCode();
        var token = await apiFetch.Content.ReadFromJsonAsync<StoredToken>();
        return token ?? throw new InvalidOperationException();
    }
    #endregion

    #region api/search
    public async Task<QuickSearchResultDto> QuickSearchAsync(string searchTerm, StoredToken jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", jwtToken.Token);
        var apiFetchResult = await _httpClient.GetAsync($"api/search/quickSearch/{searchTerm}");
        _httpClient.DefaultRequestHeaders.Clear();
        if (!apiFetchResult.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
        try
        {
            var quickSearchResultDto = await apiFetchResult.Content.ReadFromJsonAsync<QuickSearchResultDto>();
            if (quickSearchResultDto == null)
            {
                throw new InvalidOperationException();
            }
            return quickSearchResultDto;
        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<GetChatDto>> ChatSearchAsync(string searchTerm, StoredToken jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", jwtToken.Token);
        var apiFetchResult = await _httpClient.GetAsync($"api/search/chatSearch/{searchTerm}");
        _httpClient.DefaultRequestHeaders.Clear();
        if (!apiFetchResult.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
        var chatSearchResultDto = await apiFetchResult.Content.ReadFromJsonAsync<List<GetChatDto>>();
        if (chatSearchResultDto == null)
        {
            throw new InvalidOperationException();
        }
        return chatSearchResultDto;
    }

    public async Task<List<GetMessageDto>> MessageSearchAsync(string searchTerm, StoredToken jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", jwtToken.Token);
        var apiFetchResult = await _httpClient.GetAsync($"api/search/messageSearch/{searchTerm}");
        _httpClient.DefaultRequestHeaders.Clear();
        if (!apiFetchResult.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
        var messageSearchResultDto = await apiFetchResult.Content.ReadFromJsonAsync<List<GetMessageDto>>();
        if (messageSearchResultDto == null)
        {
            throw new InvalidOperationException();
        }
        return messageSearchResultDto;
    }

    public async Task<List<UserDto>> UserSearchAsync(string searchTerm, StoredToken jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", jwtToken.Token);
        var apiFetchResult = await _httpClient.GetAsync($"api/search/userSearch/{searchTerm}");
        _httpClient.DefaultRequestHeaders.Clear();
        if (!apiFetchResult.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
        var userSearchResultDto = await apiFetchResult.Content.ReadFromJsonAsync<List<UserDto>>();
        if (userSearchResultDto == null)
        {
            throw new InvalidOperationException();
        }
        return userSearchResultDto;
    }
    #endregion

    #region api/chat
    public async Task<List<GetChatDto>> GetUserChats(StoredToken jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", jwtToken.Token);
        var apiFetchResult = await _httpClient.GetAsync($"api/chat/getChatsByUserId");
        _httpClient.DefaultRequestHeaders.Clear();
        if (!apiFetchResult.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
        var foundChats = await apiFetchResult.Content.ReadFromJsonAsync<List<GetChatDto>>();
        if (foundChats == null)
        {
            throw new InvalidOperationException();
        }
        return foundChats;
    }
    #endregion
    
    #region api/message
    public async Task<List<GetMessageDto>> GetMessages(Guid chatId, StoredToken jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", jwtToken.Token);
        var apiFetchResult = await _httpClient.GetAsync($"api/message/getMessagesByChatId/{chatId}");
        _httpClient.DefaultRequestHeaders.Clear();
        if (!apiFetchResult.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
        var foundMessages = await apiFetchResult.Content.ReadFromJsonAsync<List<GetMessageDto>>();
        if (foundMessages == null)
        {
            throw new InvalidOperationException();
        }
        return foundMessages;
    }
    #endregion
}
