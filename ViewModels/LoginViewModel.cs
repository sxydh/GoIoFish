using GoIoFish.Helpers;
using GoIoFish.Services.Interfaces;
using System.Windows.Input;

namespace GoIoFish.ViewModels
{
    internal class LoginViewModel
    {

        public LoginViewModel(INavigationService navigationService) {
            LoginCommand = new RelayCommand(() => navigationService.NavigateTo<MainWindow, MainViewModel>());
        }

        public ICommand LoginCommand { get; }

    }
}
