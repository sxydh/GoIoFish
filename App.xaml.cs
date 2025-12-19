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
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            services.AddTransient<MainViewModel>();
            services.AddTransient<LoginPage>();
            services.AddTransient<LoginViewModel>();

            services.AddSingleton<IFrameProvider, FrameProvider>();
            services.AddSingleton<INavigationService>(sp =>
            {
                var frameProvider = sp.GetRequiredService<IFrameProvider>();
                return new NavigationService(frameProvider.GetFrame(), sp);
            });

            ServiceProvider = services.BuildServiceProvider();
            var mainWindow = new MainWindow(ServiceProvider);
            mainWindow.Show();

            base.OnStartup(e);
        }

    }
}
