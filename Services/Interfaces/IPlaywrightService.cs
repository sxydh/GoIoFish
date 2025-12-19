using Microsoft.Playwright;
using System;

namespace GooFish.Services.Interfaces
{
    public interface IPlaywrightService : IDisposable
    {

        IPage GetPage();

    }
}
