using System.Threading.Tasks;
using GoIoFish.Models;
using GoIoFish.Services.Interfaces;
using GoIoFish.Views.Pages;

namespace GoIoFish.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IGooFishService _gooFishService;
        private LoginState _state;

        public LoginState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        public LoginViewModel(INavigationService navigationService, IGooFishService gooFishService)
        {
            _navigationService = navigationService;
            _gooFishService = gooFishService;
        }

        public async Task InitAsync()
        {
            State = LoginState.LoadingPage;
            var isLoginOk = await _gooFishService.LoginAsync();
            if (isLoginOk)
            {
                State = LoginState.LoginSucceeded;
                _navigationService.NavigateTo<HomePage>();
            }
        }
    }
}