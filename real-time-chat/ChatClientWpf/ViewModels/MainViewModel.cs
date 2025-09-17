using ChatClientWpf.Services.Interfaces;

namespace ChatClientWpf.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        private readonly IServiceProvider _services;
        private readonly ITokenStorage _tokenStorage;
        
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }
        
        public MainViewModel(IServiceProvider services, ITokenStorage tokenStorage)
        {
            _services = services;
            _tokenStorage = tokenStorage;
            var isTokenExists = _tokenStorage.IsTokenExistsAsync().GetAwaiter().GetResult();
            if (isTokenExists)
            {
                NavigateToChat();
            }
            else
            {
                NavigateToLogin();
            }
        }
        
        
        public void NavigateToLogin()
        {
            CurrentViewModel = new LoginViewModel(this, _services);
        }
        
        public void NavigateToRegister()
        {
            CurrentViewModel = new RegisterViewModel(this, _services);
        }
        
        public void NavigateToChat()
        {
            CurrentViewModel = new ChatViewModel(this);
        }
    }
}
