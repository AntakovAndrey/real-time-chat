using System.Configuration;
using System.Windows;
using ChatClientWpf.Services.Implementations;
using ChatClientWpf.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ChatClientWpf.ViewModels;

namespace ChatClientWpf
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                .Build();
        }
    
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITokenStorage, JwtTokenStorage>();
            services.AddTransient<IApiProvider, ApiProvider>();
            services.AddTransient<IAuthService, AuthService>();
            
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainWindow>();
            
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await _host.StartAsync();
            var mainWindow = _host.Services.GetService<MainWindow>();
            mainWindow.DataContext = _host.Services.GetService<MainViewModel>();
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }
            
            base.OnExit(e);
        }
    }
}