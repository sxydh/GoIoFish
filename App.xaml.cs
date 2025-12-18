using GoIoFish.Services.Implementations;
using GoIoFish.Services.Interfaces;
using GoIoFish.ViewModels;
using GoIoFish.Views;
using GooFish.Services.Implementations;
using GooFish.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GoIoFish
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        private IServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();

            // Services 注册
            services.AddSingleton<IPlaywrightService, PlaywrightService>(sp =>
                new PlaywrightService(page => Task.CompletedTask)
            );
            services.AddSingleton<ILoginService, LoginService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // ViewModels 注册
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<MainViewModel>();

            // 入口窗口
            _serviceProvider = services.BuildServiceProvider();
            var navigationService = _serviceProvider.GetService<INavigationService>();
            navigationService.NavigateTo<LoginWindow, LoginViewModel>();
        }

    }
}
