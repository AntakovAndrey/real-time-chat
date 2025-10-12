using System.Windows.Input;
using ChatClientWpf.Dto;
using ChatClientWpf.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ChatClientWpf.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly ITokenStorage _tokenStorage;
        private readonly MainViewModel _mainViewModel;
        private LoginDto loginDto;
        
        public string Username
        {
            get => loginDto.Username;
            set
            {
                loginDto.Username = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => loginDto.Password;
            set
            {
                loginDto.Password = value;
                OnPropertyChanged();
            }
        }
        
        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }
        
        public LoginViewModel(MainViewModel mainViewModel, IServiceProvider serviceProvider)
        {
            loginDto = new LoginDto();
            _mainViewModel = mainViewModel;
            _tokenStorage = serviceProvider.GetRequiredService<ITokenStorage>();
            _authService = serviceProvider.GetRequiredService<IAuthService>();
            LoginCommand = new RelayCommand(async () => await LoginAsync());
            NavigateToRegisterCommand = new RelayCommand(() => _mainViewModel.NavigateToRegister());
        }
        
        private async Task LoginAsync()
        {
            var token = await _authService.Login(loginDto);
            await _tokenStorage.SetTokenAsync(token);
            _mainViewModel.NavigateToChat();
        }
    }
}
