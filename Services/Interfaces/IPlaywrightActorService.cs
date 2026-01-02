using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace GoIoFish.Services.Interfaces
{
    public interface IPlaywrightActorService : IAsyncDisposable
    {
        Task EnqueueAsync(Func<IPage, Task> action);
        Task<bool> InitAsync(Func<IPage, Task> initCallback);
    }
}