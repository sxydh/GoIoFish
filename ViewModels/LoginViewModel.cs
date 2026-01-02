using System.Threading.Tasks;
using GoIoFish.Services.Interfaces;

namespace GoIoFish.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILoginService _loginService;
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public LoginViewModel(INavigationService navigationService, ILoginService loginService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
        }

        public async Task InitAsync()
        {
            IsLoading = true;
            await _loginService.LoginAsync();
            IsLoading = false;
        }
    }
}