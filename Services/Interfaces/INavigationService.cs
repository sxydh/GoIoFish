using System.Windows.Controls;

namespace GoIoFish.Services.Interfaces
{
    internal interface INavigationService
    {

        void NavigateTo<T>(object parameter = null) where T : Page;
        void GoBack();
        void GoForward();
        bool CanGoBack { get; }
        bool CanGoForward { get; }

    }
}
