using GoIoFish.Services.Implementations;
using GoIoFish.Services.Interfaces;
using GoIoFish.ViewModels;
using GoIoFish.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace GoIoFish
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            // 注册 Pages
            var services = new ServiceCollection();
            services.AddTransient<MainViewModel>();
            services.AddTransient<LoginPage>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<HomePage>();
            services.AddTransient<HomeViewModel>();

            // 注册 Services
            services.AddSingleton<INavigationService>(sp =>
            {
                return new NavigationService(MainFrame, sp);
            });
            services.AddSingleton<ILoginService, LoginService>();
            services.AddSingleton<IPlaywrightService, PlaywrightService>();

            // 就绪
            var serviceProvider = services.BuildServiceProvider();
            DataContext = serviceProvider.GetRequiredService<MainViewModel>();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Loaded += (sender, args) =>
            {
                var navigationService = serviceProvider.GetRequiredService<INavigationService>();
                navigationService.NavigateTo<LoginPage>();
            };

        }
    }
}
