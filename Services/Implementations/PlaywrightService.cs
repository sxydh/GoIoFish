using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using GoIoFish.Services.Interfaces;
using Microsoft.Playwright;

namespace GoIoFish.Services.Implementations
{
    public class PlaywrightService : IPlaywrightService
    {
        private IPage _page;
        public event Action Unavailable;

        public PlaywrightService()
        {
        }

        public async Task InitAsync()
        {
            var playwright = await Playwright.CreateAsync();
            var userDataDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "GoIoFish", "Google", "Chrome", "User Data");
            var context = await playwright.Chromium.LaunchPersistentContextAsync(userDataDir, new BrowserTypeLaunchPersistentContextOptions
            {
                Headless = !Debugger.IsAttached,
                Args = new[] { "--start-maximized", "--auto-open-devtools-for-tabs" },
                ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                ViewportSize = ViewportSize.NoViewport,
                IgnoreHTTPSErrors = true
            });
            var page = context.Pages[0];

            _page = page;
            _page.Close += async (sender, e) => await DisposeAsync();
            if (_page.Context.Browser != null)
            {
                _page.Context.Browser.Disconnected += async (sender, e) => await DisposeAsync();
            }
        }

        public IPage GetPage()
        {
            return _page;
        }

        public async ValueTask DisposeAsync()
        {
            Unavailable?.Invoke();
            if (_page.Context.Browser != null)
            {
                await _page.Context.Browser.CloseAsync();
            }
        }
    }
}