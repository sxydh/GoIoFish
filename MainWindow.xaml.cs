using System;
using System.Collections.Specialized;
using System.Windows;
using GoIoFish.Services.Implementations;
using GoIoFish.Services.Interfaces;
using GoIoFish.ViewModels;
using GoIoFish.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace GoIoFish
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public MainWindow()
        {
            InitializeComponent();

            // 注册 Pages
            Log.Info("注册 Pages...");
            var services = new ServiceCollection();
            services.AddTransient<MainViewModel>();
            services.AddTransient<LoginPage>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<HomePage>();
            services.AddTransient<HomeViewModel>();

            // 注册 Services
            Log.Info("注册 Services...");
            services.AddSingleton<INavigationService>(sp => new NavigationService(MainFrame, sp));
            services.AddSingleton<IGooFishService, GooFishService>();
            services.AddSingleton<IPlaywrightActorService, PlaywrightActorService>();

            // 就绪
            var serviceProvider = services.BuildServiceProvider();
            var mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();
            DataContext = mainViewModel;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Topmost = true;
            Loaded += (sender, args) =>
            {
                var navigationService = serviceProvider.GetRequiredService<INavigationService>();
                navigationService.NavigateTo<LoginPage>();
                mainViewModel.Logs.CollectionChanged += Logs_CollectionChanged;
            };
        }
        
        private void Logs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (LogListBox.Items.Count > 0)
                    LogListBox.ScrollIntoView(LogListBox.Items[LogListBox.Items.Count - 1]);
            }));
        }
    }
}