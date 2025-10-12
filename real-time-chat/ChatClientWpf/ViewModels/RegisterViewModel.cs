using System.Windows;
using System.Windows.Input;
using ChatClientWpf.Dto;
using ChatClientWpf.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ChatClientWpf.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly MainViewModel _mainViewModel;
        private readonly RegisterDto _registerDto;
        
        public string Name
        {
            get => _registerDto.Name ?? throw new InvalidOperationException();
            set
            {
                _registerDto.Name = value;
                OnPropertyChanged();
            }
        }
        public string Surname
        {
            get => _registerDto.Surname ?? throw new InvalidOperationException();
            set
            {
                _registerDto.Surname = value;
                OnPropertyChanged();
            }
        }
        public string Username
        {
            get => _registerDto.Username ?? throw new InvalidOperationException();
            set
            {
                _registerDto.Username = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get => _registerDto.Email ?? throw new InvalidOperationException();
            set
            {
                _registerDto.Email = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _registerDto.Password ?? throw new InvalidOperationException();
            set
            {
                _registerDto.Password = value;
                OnPropertyChanged();
            }
        }
        public string ConfirmPassword
        {
            get => _registerDto.ConfirmPassword ?? throw new InvalidOperationException();
            set
            {
                _registerDto.ConfirmPassword = value;
                OnPropertyChanged();
            }
        }
        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }
        
        public RegisterViewModel(MainViewModel mainViewModel, IServiceProvider serviceProvider)
        {
            _registerDto = new RegisterDto();
            _authService = serviceProvider.GetRequiredService<IAuthService>();
            _mainViewModel = mainViewModel;
            RegisterCommand = new RelayCommand(async () => await RegisterAsync());
            NavigateToLoginCommand = new RelayCommand(() => _mainViewModel.NavigateToLogin());
        }
        
        //ToDo: add data validation
        private async Task RegisterAsync()
        {
            try
            {
                await _authService.Register(_registerDto);
                _mainViewModel.NavigateToLogin();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
