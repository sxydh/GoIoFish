using GoIoFish.ViewModels;
using System.Windows.Controls;

namespace GoIoFish.Views.Pages
{
    /// <summary>
    /// LoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage(LoginViewModel loginViewModel)
        {
            InitializeComponent();

            DataContext = loginViewModel;
            Loaded += async (sender, args) =>
            {
                await loginViewModel.InitAsync();
            };
        }

    }
}
