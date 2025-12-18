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
            services.AddSingleton<IPlaywrightService, PlaywrightService>(sp =>
                new PlaywrightService(page => Task.CompletedTask)
            );

            _serviceProvider = services.BuildServiceProvider();
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

    }
}
