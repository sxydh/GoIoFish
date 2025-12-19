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
        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            var frameProvider = serviceProvider.GetRequiredService<IFrameProvider>();
            frameProvider.SetFrame(MainFrame);

            var mainVm = serviceProvider.GetRequiredService<MainViewModel>();
            DataContext = mainVm;

            var navService = serviceProvider.GetRequiredService<INavigationService>();
            navService.NavigateTo<LoginPage>();
        }
    }
}
