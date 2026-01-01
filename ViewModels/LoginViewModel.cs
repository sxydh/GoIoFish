using GoIoFish.Helpers;
using GoIoFish.Services.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GoIoFish.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {

        private readonly INavigationService _navigationService;
        private readonly ILoginService _loginService;
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }
        public ICommand LoginCommand { get; }

        public LoginViewModel(INavigationService navigationService, ILoginService loginService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
            LoginCommand = new RelayCommand(() => { });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task InitAsync()
        {
            IsLoading = true;
            await _loginService.LoginAsync();
            IsLoading = false;
        }

    }
}
