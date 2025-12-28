using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace GoIoFish.Services.Interfaces
{
    public interface IPlaywrightActorService : IAsyncDisposable
    {
        Task InitAsync();
        Task EnqueueAsync(Func<IPage, Task> action);
        bool IsAvailable { get; }
    }
}