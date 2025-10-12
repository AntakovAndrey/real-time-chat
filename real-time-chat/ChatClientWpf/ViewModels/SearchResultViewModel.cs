using System.Collections.ObjectModel;
using System.Windows.Input;
using ChatClientWpf.Dto;
using ChatClientWpf.Enums;

namespace ChatClientWpf.ViewModels
{
    public class SearchResultsViewModel : BaseViewModel
    {
        private ObservableCollection<SearchResultItemDto> _searchResults;
        private SearchResultItemDto _selectedSearchResult;
        private SearchType _currentSearchType;
        private bool _isSearching;

        public ObservableCollection<SearchResultItemDto> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }

        public SearchResultItemDto SelectedSearchResult
        {
            get => _selectedSearchResult;
            set
            {
                _selectedSearchResult = value;
                OnPropertyChanged();
                OnSearchResultSelected(value);
            }
        }

        public SearchType CurrentSearchType
        {
            get => _currentSearchType;
            set
            {
                _currentSearchType = value;
                OnPropertyChanged();
                OnSearchTypeChanged();
            }
        }

        public bool IsSearching
        {
            get => _isSearching;
            set
            {
                _isSearching = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchResultSelectedCommand { get; }
        public ICommand SetQuickSearchCommand { get; }
        public ICommand SetMessagesSearchCommand { get; }
        public ICommand SetUsersSearchCommand { get; }
        public ICommand SetChatsSearchCommand { get; }

        public event EventHandler<SearchResultItemDto> SearchResultSelected;
        public event EventHandler<SearchType> SearchTypeChanged;

        public SearchResultsViewModel()
        {
            IsSearching = false;
            SearchResults = new ObservableCollection<SearchResultItemDto>();
            CurrentSearchType = SearchType.Quick;
            
            SearchResultSelectedCommand = new RelayCommand<SearchResultItemDto>(OnSearchResultSelected);
            SetQuickSearchCommand = new RelayCommand(() => CurrentSearchType = SearchType.Quick);
            SetMessagesSearchCommand = new RelayCommand(() => CurrentSearchType = SearchType.Messages);
            SetUsersSearchCommand = new RelayCommand(() => CurrentSearchType = SearchType.Users);
            SetChatsSearchCommand = new RelayCommand(() => CurrentSearchType = SearchType.Chats);
        }

        private void OnSearchResultSelected(SearchResultItemDto result)
        {
            SearchResultSelected?.Invoke(this, result);
        }

        private void OnSearchTypeChanged()
        {
            SearchTypeChanged?.Invoke(this, CurrentSearchType);
        }
    }
}
