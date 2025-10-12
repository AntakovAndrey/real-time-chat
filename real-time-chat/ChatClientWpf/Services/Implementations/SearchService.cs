using ChatClientWpf.Dto;
using ChatClientWpf.Enums;
using ChatClientWpf.Services.Interfaces;
using Exception = System.Exception;

namespace ChatClientWpf.Services.Implementations;

public class SearchService : ISearchService
{
    private readonly IApiProvider _apiProvider;
    private readonly ITokenStorage _tokenStorage;

    public SearchService(IApiProvider apiProvider, ITokenStorage tokenStorage)
    {
        _apiProvider = apiProvider;
        _tokenStorage = tokenStorage;
    }
    
    public async Task<List<SearchResultItemDto>> QuickSearch(string searchTerm)
    {
        var token = await _tokenStorage.GetTokenAsync();
        if (token == null)
        {
            throw new Exception("No token provided");
        }
        var fetchedSearchResult = await _apiProvider.QuickSearchAsync(searchTerm, token);
        var searchResult = new List<SearchResultItemDto>();
        searchResult.AddRange(fetchedSearchResult.Chats
            .Select(c=> new SearchResultItemDto
            {
                Id = c.Id,
                Title = c.Name,
                Description = $"{
                    string.Join(
                        String.Empty,
                        c.Messages
                            .OrderBy(m=>m.Date)
                            .FirstOrDefault()
                            .Content
                            .Take(10).ToArray())
                }... {
                    c.Messages
                        .OrderBy(m=>m.Date)
                        .FirstOrDefault()
                        .Date
                }",
                Type = SearchResultType.Chat,
                Avatar = "💬"
            })
        );
        searchResult.AddRange(fetchedSearchResult.Users
            .Select(u=> new SearchResultItemDto
            {
                Id = u.Id,
                Title = u.Username,
                Description = u.Email,
                Type = SearchResultType.User,
                Avatar = "🧑"
            })
        );
        searchResult.AddRange(fetchedSearchResult.Messages
            .Select(m=> new SearchResultItemDto
            {
                Id = m.Id,
                Title = m.UserSent.Username,
                Description = m.Content,
                Type = SearchResultType.Message,
                Avatar = "😽"
            })
        );
        return searchResult;
    }

    public async Task<List<SearchResultItemDto>> ChatSearch(string searchTerm)
    {
        var token = await _tokenStorage.GetTokenAsync();
        if (token == null)
        {
            throw new NullReferenceException("No token provided");
        }
        var fetchedSearchResult = await _apiProvider.ChatSearchAsync(searchTerm, token);
        var searchResult = fetchedSearchResult.Select(x =>
            new SearchResultItemDto
            {
                Id = x.Id,
                Title = x.Name,
                Type = SearchResultType.Chat,
                Avatar = "💬",
                Description = $"{
                    String.Join(String.Empty,
                    x.Messages
                        .OrderBy(m => m.Date)
                        .FirstOrDefault()
                        .Content.Take(10).ToArray())
                }... {
                    x.Messages
                        .OrderBy(m => m.Date)
                        .FirstOrDefault()
                        .Date
                }"
            }).ToList();
        return searchResult;
    }

    public async Task<List<SearchResultItemDto>> MessageSearch(string searchTerm)
    {
        var token = await _tokenStorage.GetTokenAsync();
        if (token == null)
        {
            throw new NullReferenceException("No token provided");
        }
        var fetchedSearchResult = await _apiProvider.MessageSearchAsync(searchTerm, token);
        var searchResult = fetchedSearchResult.Select(x=>
            new SearchResultItemDto
            {
                Id = x.Id,
                Avatar = "😽",
                Type = SearchResultType.Message,
                Title = x.UserSent.Username,
                Description = x.Content.Take(10).ToString(),
            }).ToList();
        return searchResult;
    }

    public async Task<List<SearchResultItemDto>> UserSearch(string searchTerm)
    {
        var token = await _tokenStorage.GetTokenAsync();
        if (token == null)
        {
            throw new NullReferenceException("No token provided");
        }
        var fetchedSearchResult = await _apiProvider.UserSearchAsync(searchTerm, token);
        var searchResult = fetchedSearchResult.Select(x=>
            new SearchResultItemDto
            {
                Id = x.Id,
                Avatar = "🧑",
                Type = SearchResultType.User,
                Title = x.Username,
                Description = x.Email,
            }).ToList();
        return searchResult;
    }
}
