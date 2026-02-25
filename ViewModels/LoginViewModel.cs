using System.Threading.Tasks;
using GoIoFish.Models;
using GoIoFish.Services.Interfaces;
using GoIoFish.Views.Pages;

namespace GoIoFish.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private LoginState _state;

        public LoginState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        public LoginViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task LoginAsync()
        {
            State = LoginState.LoadingPage;
            var isLoginOk = await Task.FromResult(true);
            if (isLoginOk)
            {
                State = LoginState.LoginSucceeded;
                _navigationService.NavigateTo<HomePage>();
            }
        }
    }
}