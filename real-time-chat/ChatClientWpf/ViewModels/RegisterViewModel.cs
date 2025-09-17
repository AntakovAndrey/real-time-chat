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
        private RegisterDto registerDto;
        
        public string Name
        {
            get => registerDto.Name ?? throw new InvalidOperationException();
            set
            {
                registerDto.Name = value;
                OnPropertyChanged();
            }
        }
        
        public string Surname
        {
            get => registerDto.Surname ?? throw new InvalidOperationException();
            set
            {
                registerDto.Surname = value;
                OnPropertyChanged();
            }
        }
        
        public string Username
        {
            get => registerDto.Username ?? throw new InvalidOperationException();
            set
            {
                registerDto.Username = value;
                OnPropertyChanged();
            }
        }
        
        public string Email
        {
            get => registerDto.Email ?? throw new InvalidOperationException();
            set
            {
                registerDto.Email = value;
                OnPropertyChanged();
            }
        }
        
        public string Password
        {
            get => registerDto.Password ?? throw new InvalidOperationException();
            set
            {
                registerDto.Password = value;
                OnPropertyChanged();
            }
        }
        
        public string ConfirmPassword
        {
            get => registerDto.ConfirmPassword ?? throw new InvalidOperationException();
            set
            {
                registerDto.ConfirmPassword = value;
                OnPropertyChanged();
            }
        }
        
        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }
        
        public RegisterViewModel(MainViewModel mainViewModel, IServiceProvider serviceProvider)
        {
            registerDto = new RegisterDto();
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
                await _authService.Register(registerDto);
                _mainViewModel.NavigateToLogin();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
