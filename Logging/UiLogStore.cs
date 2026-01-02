using System.Collections.ObjectModel;
using System.Windows;

namespace GoIoFish.Logging
{
    public static class UiLogStore
    {
        public static ObservableCollection<LogItem> Logs { get; } = new ObservableCollection<LogItem>();

        public static void Add(string message, string level = "Info")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Logs.Add(new LogItem { Message = message, Level = level });
                if (Logs.Count > 500) Logs.RemoveAt(0);
            });
        }
    }

    public class LogItem
    {
        public string Message { get; set; }
        public string Level { get; set; }
    }
}