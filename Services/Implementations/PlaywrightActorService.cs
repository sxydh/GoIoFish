using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GoIoFish.Helpers;
using GoIoFish.Services.Interfaces;
using Microsoft.Playwright;
using NLog;

namespace GoIoFish.Services.Implementations
{
    public class PlaywrightActorService : IPlaywrightActorService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private IPlaywright _playwright;
        private IBrowserContext _context;
        private IBrowser _browser;
        private IPage _page;

        private readonly BlockingCollection<IActorMessage> _msgQueue = new BlockingCollection<IActorMessage>(100);
        private readonly CancellationTokenSource _msgQueueCts = new CancellationTokenSource();
        private int _initFlag;
        private Func<IPage, Task> _initCallback;

        public Task<T> EnqueueAsync<T>(Func<IPage, Task<T>> action)
        {
            if (_msgQueue.IsAddingCompleted)
            {
                Log.Warn("服务已终止");
                throw new InvalidOperationException("Actor stopped");
            }

            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            _msgQueue.Add(new ActorMessage<T>(action, tcs));
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
                Log.Info("初始化...");
                await InitBrowserAsync();
                var isCallbackOk = false;
                for (var i = 3; i > 0; i--)
                {
                    try
                    {
                        Log.Info("执行初始化回调...");
                        await initCallback(_page);
                        isCallbackOk = true;
                        break;
                    }
                    catch(Exception e)
                    {
                        Log.Error(e, "执行初始化回调异常，剩余重试次数：{0}", i - 1);
                    }
                }

                if (!isCallbackOk)
                {
                    return false;
                }

                return true;
            }
            catch(Exception e)
            {
                Log.Error(e, "初始化异常");
            }

            return false;
        }

        private async Task InitBrowserAsync()
        {
            await ExecUtil.SafeExecAsync(() => _page.CloseAsync());
            await ExecUtil.SafeExecAsync(() => _context.CloseAsync());
            await ExecUtil.SafeExecAsync(() => _browser.CloseAsync());
            var playwright = _playwright;
            ExecUtil.SafeExec(() => playwright.Dispose());
            await Task.Delay(3000);

            _playwright = await Playwright.CreateAsync();
            var userDataDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "GoIoFish", "Google", "Chrome", "User Data");
            Log.Info("启动浏览器，数据目录：{0}", userDataDir);
            _context = await _playwright.Chromium.LaunchPersistentContextAsync(
                userDataDir,
                new BrowserTypeLaunchPersistentContextOptions
                {
                    Headless = false,
                    Args = new[] { "--start-maximized" },
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
                Log.Info("执行任务循环...");
                foreach (var msg in _msgQueue.GetConsumingEnumerable(_msgQueueCts.Token))
                {
                    try
                    {
                        await msg.ExecuteAsync(_page).ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "执行任务异常");
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
            catch(Exception e)
            {
                Log.Error(e, "执行任务循环异常");
            }
        }

        public async ValueTask DisposeAsync()
        {
            Log.Info("释放资源...");
            ExecUtil.SafeExec(() => _msgQueue.CompleteAdding());
            ExecUtil.SafeExec(() => _msgQueueCts.Cancel());

            await ExecUtil.SafeExecAsync(() => _page.CloseAsync());
            await ExecUtil.SafeExecAsync(() => _context.CloseAsync());
            await ExecUtil.SafeExecAsync(() => _browser.CloseAsync());
            var playwright = _playwright;
            ExecUtil.SafeExec(() => playwright.Dispose());
        }
        
        private interface IActorMessage
        {
            Task ExecuteAsync(IPage page);
            void Fail(Exception ex);
        }

        private sealed class ActorMessage<T> : IActorMessage
        {
            private readonly Func<IPage, Task<T>> _action;
            private readonly TaskCompletionSource<T> _tcs;

            public ActorMessage(Func<IPage, Task<T>> action, TaskCompletionSource<T> tcs)
            {
                _action = action;
                _tcs = tcs;
            }

            public async Task ExecuteAsync(IPage page)
            {
                var ret = await _action(page);
                _tcs.TrySetResult(ret);
            }

            public void Fail(Exception ex) => _tcs.TrySetException(ex);
        }
    }
}