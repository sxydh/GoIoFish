using GooFish.Services.Interfaces;
using Microsoft.Playwright;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GooFish.Services.Implementations
{
    public class PlaywrightService : IPlaywrightService
    {

        private IPage _page;
        public event Action Unavailable;

        public PlaywrightService(Func<IPage, Task> initFunc)
        {
            Task.Run(async () =>
            {
                var playwright = await Playwright.CreateAsync();
                var userDataDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "GoIoFish", "Google", "Chrome", "User Data");
                var context = await playwright.Chromium.LaunchPersistentContextAsync(userDataDir, new BrowserTypeLaunchPersistentContextOptions
                {
                    Headless = false,
                    Args = new[] { "--start-maximized", "--auto-open-devtools-for-tabs" },
                    ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                    ViewportSize = ViewportSize.NoViewport,
                    IgnoreHTTPSErrors = true
                });
                var page = context.Pages[0];
                await initFunc(page);

                _page = page;
                _page.Close += (sender, e) => Dispose();
                _page.Context.Browser.Disconnected += (sender, e) => Dispose();
            }).Wait();
        }

        public IPage GetPage()
        {
            return _page;
        }
        
        public void Dispose()
        {
            Task.Run(async () =>
            {
                try
                {
                    Unavailable?.Invoke();
                    await _page?.Context.Browser.CloseAsync();
                }
                catch {
                }
            }).Wait();
        }

    }
}
