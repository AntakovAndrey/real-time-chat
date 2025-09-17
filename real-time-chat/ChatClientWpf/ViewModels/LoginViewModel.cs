using System.Windows.Input;

namespace ChatClientWpf.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private string _username;
        private string _password;
        
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        
        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }
        
        public LoginViewModel(MainViewModel mainViewModel, IServiceProvider serviceProvider)
        {
            _mainViewModel = mainViewModel;
            
            LoginCommand = new RelayCommand(async () => await LoginAsync());
            NavigateToRegisterCommand = new RelayCommand(() => _mainViewModel.NavigateToRegister());
        }
        
        private async Task LoginAsync()
        {
            /*
            // Здесь реализация логики авторизации
            if (await AuthService.LoginAsync(Username, Password))
            {
                _mainViewModel.NavigateToChat();
            }
            else
            {
                MessageBox.Show("Ошибка авторизации");
            }*/
        }
    }
}