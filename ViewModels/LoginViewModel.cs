using System;
using System.Threading.Tasks;
using GoIoFish.Models;
using GoIoFish.Services.Interfaces;

namespace GoIoFish.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILoginService _loginService;
        private LoginState _state;
        private string _qrCode;

        public LoginState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        public string QrCode
        {
            get => _qrCode;
            set => SetProperty(ref _qrCode, value);
        }

        public LoginViewModel(INavigationService navigationService, ILoginService loginService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
        }

        public async Task InitAsync()
        {
            State = LoginState.LoadingPage;
            await _loginService.LoginAsync(new Progress<LoginProgressMsg>(e =>
            {
                State = e.State;
                switch (e.State)
                {
                    case LoginState.QrCodeReady:
                        QrCode = e.Body;
                        break;
                }
            }));
            State = LoginState.LoginSucceeded;
        }
    }
}