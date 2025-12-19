using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace GoIoFish.Services.Interfaces
{
    public interface IPlaywrightService : IAsyncDisposable
    {

        Task InitAsync();
        IPage GetPage();

    }
}
