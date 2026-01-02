using System.Threading.Tasks;
using GoIoFish.Models;
using GoIoFish.Services.Interfaces;
using GoIoFish.Views.Pages;

namespace GoIoFish.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILoginService _loginService;
        private LoginState _state;

        public LoginState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        public LoginViewModel(INavigationService navigationService, ILoginService loginService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
        }

        public async Task InitAsync()
        {
            State = LoginState.LoadingPage;
            var isLoginOk = await _loginService.LoginAsync();
            if (isLoginOk)
            {
                State = LoginState.LoginSucceeded;
                _navigationService.NavigateTo<HomePage>();
            }
        }
    }
}