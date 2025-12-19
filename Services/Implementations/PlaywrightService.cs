using GoIoFish.Services.Interfaces;
using Microsoft.Playwright;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GoIoFish.Services.Implementations
{
    public class PlaywrightService : IPlaywrightService
    {

        private IPage _page;
        public event Action Unavailable;

        public PlaywrightService() { }

        public async Task InitAsync()
        {
            var playwright = await Playwright.CreateAsync();
            var userDataDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "GoIoFish", "Google", "Chrome", "User Data");
            var context = await playwright.Chromium.LaunchPersistentContextAsync(userDataDir, new BrowserTypeLaunchPersistentContextOptions
            {
                Headless = true,
                Args = new[] { "--start-maximized", "--auto-open-devtools-for-tabs" },
                ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                ViewportSize = ViewportSize.NoViewport,
                IgnoreHTTPSErrors = true
            });
            var page = context.Pages[0];

            _page = page;
            _page.Close += async (sender, e) => await DisposeAsync();
            _page.Context.Browser.Disconnected += async (sender, e) => await DisposeAsync();
        }

        public IPage GetPage()
        {
            return _page;
        }

        public async ValueTask DisposeAsync()
        {
            Unavailable?.Invoke();
            await _page.Context.Browser?.CloseAsync();
        }
    }
}
