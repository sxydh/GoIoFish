using System.Collections.ObjectModel;
using GoIoFish.Logging;

namespace GoIoFish.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<LogItem> Logs => UiLogStore.Logs;
    }
}