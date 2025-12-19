using GoIoFish.ViewModels;
using System.Windows.Controls;

namespace GoIoFish.Views.Pages
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage(HomeViewModel homeViewModel)
        {
            InitializeComponent();

            DataContext = homeViewModel;
        }
    }
}
