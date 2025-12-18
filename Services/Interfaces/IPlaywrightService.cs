using Microsoft.Playwright;
using System;

namespace GooFish.Services.Interfaces
{
    internal interface IPlaywrightService : IDisposable
    {

        IPage GetPage();

    }
}
