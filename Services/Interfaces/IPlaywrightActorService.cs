using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace GoIoFish.Services.Interfaces
{
    public interface IPlaywrightActorService : IAsyncDisposable
    {
        Task<bool> InitAsync(Func<IPage, Task> initCallback);
        Task<T> EnqueueAsync<T>(Func<IPage, Task<T>> action);
    }
}