using System.Windows;
using ChatClientWpf.Configuration;
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
    
        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.Configure<ApiConfiguration>(context.Configuration.GetSection("ApiConfiguration"));
            services.AddTransient<ITokenStorage, JwtTokenStorage>();
            services.AddTransient<IApiProvider, ApiProvider>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<LoginViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await _host.StartAsync();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            var mainViewModel = _host.Services.GetRequiredService<MainViewModel>();
            mainWindow.DataContext = mainViewModel;
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