using GoIoFish.Helpers;
using GoIoFish.Services.Interfaces;
using System.Windows.Input;

namespace GoIoFish.ViewModels
{
    public class LoginViewModel
    {

        public LoginViewModel(INavigationService navigationService)
        {
            LoginCommand = new RelayCommand(() => { });
        }

        public ICommand LoginCommand { get; }

    }
}
