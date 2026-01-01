using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GoIoFish.Helpers;
using GoIoFish.Services.Interfaces;
using Microsoft.Playwright;

namespace GoIoFish.Services.Implementations
{
    public class PlaywrightActorService : IPlaywrightActorService
    {
        private IPlaywright _playwright;
        private IBrowserContext _context;
        private IBrowser _browser;
        private IPage _page;

        private readonly BlockingCollection<ActorMessage> _msgQueue = new BlockingCollection<ActorMessage>(100);
        private readonly CancellationTokenSource _msgQueueCts = new CancellationTokenSource();
        private int _initFlag;
        private Func<IPage, Task> _initCallback;

        public Task EnqueueAsync(Func<IPage, Task> action)
        {
            if (_msgQueue.IsAddingCompleted)
                throw new InvalidOperationException("Actor stopped");

            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            _msgQueue.Add(new ActorMessage(action, tcs));
            return tcs.Task;
        }

        public async Task<bool> InitAsync(Func<IPage, Task> initCallback)
        {
            if (Interlocked.Exchange(ref _initFlag, 1) == 1)
                return false;

            _initCallback = initCallback;
            var isInitOk = await SafeInitAsync(_initCallback);
            if (!isInitOk)
            {
                return false;
            }

            _ = Task.Run(WorkerLoopAsync);
            return true;
        }

        private async Task<bool> SafeInitAsync(Func<IPage, Task> initCallback)
        {
            try
            {
                await InitBrowserAsync();
                var isCallbackOk = false;
                for (var i = 0; i < 3; i++)
                {
                    try
                    {
                        await initCallback(_page);
                        isCallbackOk = true;
                        break;
                    }
                    catch
                    {
                        // ignored
                    }
                }

                if (!isCallbackOk)
                {
                    return false;
                }
            }
            catch
            {
                // ignored
            }

            return true;
        }

        private async Task InitBrowserAsync()
        {
            await ExUtil.SafeExeAsync(() => _page.CloseAsync());
            await ExUtil.SafeExeAsync(() => _context.CloseAsync());
            await ExUtil.SafeExeAsync(() => _browser.CloseAsync());
            var playwright = _playwright;
            ExUtil.SafeExe(() => playwright.Dispose());
            await Task.Delay(3000);

            _playwright = await Playwright.CreateAsync();
            var userDataDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "GoIoFish", "Google", "Chrome", "User Data");
            _context = await _playwright.Chromium.LaunchPersistentContextAsync(
                userDataDir,
                new BrowserTypeLaunchPersistentContextOptions
                {
                    Headless = !Debugger.IsAttached,
                    Args = new[] { "--start-maximized", "--auto-open-devtools-for-tabs" },
                    ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                    ViewportSize = ViewportSize.NoViewport,
                    IgnoreHTTPSErrors = true
                });
            _browser = _context.Browser;
            _page = _context.Pages[0];
        }

        private async Task WorkerLoopAsync()
        {
            try
            {
                foreach (var msg in _msgQueue.GetConsumingEnumerable(_msgQueueCts.Token))
                {
                    try
                    {
                        await msg.ExecuteAsync(_page).ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        msg.Fail(e);
                        if (e.Message.Contains("Target page, context or browser has been closed"))
                        {
                            var isInitOk = await SafeInitAsync(_initCallback);
                            if (!isInitOk)
                            {
                                await DisposeAsync();
                                return;
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        public async ValueTask DisposeAsync()
        {
            ExUtil.SafeExe(() => _msgQueue.CompleteAdding());
            ExUtil.SafeExe(() => _msgQueueCts.Cancel());

            await ExUtil.SafeExeAsync(() => _page.CloseAsync());
            await ExUtil.SafeExeAsync(() => _context.CloseAsync());
            await ExUtil.SafeExeAsync(() => _browser.CloseAsync());
            var playwright = _playwright;
            ExUtil.SafeExe(() => playwright.Dispose());
        }

        private sealed class ActorMessage
        {
            private readonly Func<IPage, Task> _action;
            private readonly TaskCompletionSource<bool> _tcs;

            public ActorMessage(Func<IPage, Task> action, TaskCompletionSource<bool> tcs)
            {
                _action = action;
                _tcs = tcs;
            }

            public async Task ExecuteAsync(IPage page)
            {
                await _action(page);
                _tcs.TrySetResult(true);
            }

            public void Fail(Exception ex) => _tcs.TrySetException(ex);
        }
    }
}