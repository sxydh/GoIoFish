using GoIoFish.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GoIoFish.Services.Implementations
{
    public class NavigationService : INavigationService
    {

        private readonly Frame _frame;
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(Frame frame, IServiceProvider serviceProvider)
        {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void NavigateTo<T>(object parameter = null) where T : Page
        {
            var page = _serviceProvider.GetService<T>();
            if (page == null)
                throw new InvalidOperationException($"Page {typeof(T)} is not registered in DI container.");

            _frame.Navigate(page, parameter);
        }

        public void GoBack() => _frame.GoBack();
        public void GoForward() => _frame.GoForward();
        public bool CanGoBack => _frame.CanGoBack;
        public bool CanGoForward => _frame.CanGoForward;

    }
}
