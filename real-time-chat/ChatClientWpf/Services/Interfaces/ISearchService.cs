using ChatClientWpf.Dto;

namespace ChatClientWpf.Services.Interfaces;

public interface ISearchService
{
    public Task<List<SearchResultItemDto>> QuickSearch(string searchTerm);
    public Task<List<SearchResultItemDto>> ChatSearch(string searchTerm);
    public Task<List<SearchResultItemDto>> MessageSearch(string searchTerm);
    public Task<List<SearchResultItemDto>> UserSearch(string searchTerm);
}
