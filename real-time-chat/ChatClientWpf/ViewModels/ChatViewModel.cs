using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ChatClientWpf.Dto;
using ChatClientWpf.Enums;
using ChatClientWpf.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ChatClientWpf.ViewModels;

public class ChatViewModel : BaseViewModel
{
    
    #region Fields
    private ChatAreaViewModel _chatAreaViewModel;
    private readonly MainViewModel _mainViewModel;
    
    private readonly ISearchService _searchService;
    private readonly IChatService _chatService;
    
    private ObservableCollection<ChatItemDto> _chats;
    private ChatItemDto _selectedChat;
    
    private string _searchText;
    private bool _isSearchActive;
    private bool _isLoading;
    private string _loadingMessage;
    #endregion
    
    #region Properties
    public SearchResultsViewModel SearchResultsViewModel { get; }
    public ChatAreaViewModel ChatAreaViewModel
    {
        get => _chatAreaViewModel;
        set
        {
            _chatAreaViewModel = value;
            OnPropertyChanged();
        }
    }
    public ObservableCollection<ChatItemDto> Chats
    {
        get => _chats;
        set
        {
            _chats = value;
            OnPropertyChanged();
        }
    }
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            OnSearchTextChanged();
        }
    }
    public bool IsSearchActive
    {
        get => _isSearchActive;
        set
        {
            _isSearchActive = value;
            OnPropertyChanged();
        }
    }
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }
    public string LoadingMessage
    {
        get => _loadingMessage;
        set
        {
            _loadingMessage = value;
            OnPropertyChanged();
        }
    }
    public ChatItemDto SelectedChat
    {
        get => _selectedChat;
        set
        {
            _selectedChat = value;
            OnPropertyChanged();
            SelectChat(value);
        }
    }
    #endregion
    
    #region Commands
    public ICommand OpenSettingsCommand { get; }
    public ICommand SelectChatCommand { get; }
    public ICommand RefreshChatsCommand { get; }
    #endregion
    
    #region Constructor
    public ChatViewModel(MainViewModel mainViewModel, IServiceProvider serviceProvider)
    {
        _mainViewModel = mainViewModel;
        _searchService = serviceProvider.GetRequiredService<ISearchService>();
        _chatService = serviceProvider.GetRequiredService<IChatService>();
        Chats = new ObservableCollection<ChatItemDto>();
        SearchResultsViewModel = new SearchResultsViewModel();
        ChatAreaViewModel = new ChatAreaViewModel(serviceProvider);
        OpenSettingsCommand = new RelayCommand(OpenSettings);
        SelectChatCommand = new RelayCommand<ChatItemDto>(SelectChat);
        RefreshChatsCommand = new RelayCommand(async () => await LoadChatsAsync());
        SearchResultsViewModel.SearchResultSelected += OnSearchResultSelected;
        SearchResultsViewModel.SearchTypeChanged += OnSearchTypeChanged;
        InitializeAsync();
    }
    #endregion
    
    #region Methods
    private async void InitializeAsync()
    {
        try
        {
            await LoadChatsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка инициализации: {ex.Message}");
        }
    }
    private void OnSearchResultSelected(object sender, SearchResultItemDto result)
    {
        SelectSearchResult(result);
        SearchText = string.Empty;
    }
    private async void OnSearchTypeChanged(object sender, SearchType searchType)
    {
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            await SearchAsync();
        }
    }
    private async void OnSearchTextChanged()
    {
        IsSearchActive = !string.IsNullOrWhiteSpace(SearchText);
        if (IsSearchActive)
        {
            await Task.Delay(300);
            if (SearchText == _searchText)
            {
                await SearchAsync();
            }
        }
        else
        {
            SearchResultsViewModel.SearchResults.Clear();
        }
    }
    
    private void OpenSettings()
    {
        MessageBox.Show("Открытие настроек...");
    }
    private async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            SearchResultsViewModel.SearchResults.Clear();
            return;
        }

        try
        {
            SearchResultsViewModel.IsSearching = true;
            IEnumerable<SearchResultItemDto> searchResult;
            switch (SearchResultsViewModel.CurrentSearchType)
            {
                case SearchType.Quick:
                    searchResult = await _searchService.QuickSearch(SearchText);
                    break;
                case SearchType.Messages:
                    searchResult = await _searchService.MessageSearch(SearchText);
                    break;
                case SearchType.Users:
                    searchResult = await _searchService.UserSearch(SearchText);
                    break;
                case SearchType.Chats:
                    searchResult = await _searchService.ChatSearch(SearchText);
                    break;
                default:
                    searchResult = await _searchService.QuickSearch(SearchText);
                    break;
            }
            
            SearchResultsViewModel.SearchResults.Clear();
            foreach (var item in searchResult)
            {
                SearchResultsViewModel.SearchResults.Add(item);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка поиска: {ex.Message}");
        }
        finally
        {
            SearchResultsViewModel.IsSearching = false;
        }
    }
    private void SelectChat(ChatItemDto chat)
    {
        _ = ChatAreaViewModel.LoadChatAsync(chat);
    }
    private void SelectSearchResult(SearchResultItemDto result)
    {
        switch (result.Type)
        {
            case SearchResultType.User:
                break;
            case SearchResultType.Chat:
                break;
            case SearchResultType.Message:
                break;
        }
    }
    private async Task LoadChatsAsync()
    {
        try
        {
            IsLoading = true;
            LoadingMessage = "Загрузка чатов...";
            var foundChats = await _chatService.GetUserChats();
            Chats.Clear();
            foundChats.ForEach(chat =>
                Chats.Add(
                    new ChatItemDto
                    {
                        Id = chat.Id,
                        Name = chat.Name,
                    }));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки чатов: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
            LoadingMessage = string.Empty;
        }
    }
    #endregion
}
