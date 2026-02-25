using System.Collections.Generic;
using System.Threading.Tasks;
using GoIoFish.Helpers;
using GoIoFish.Models;
using GoIoFish.Models.GooFish;
using GoIoFish.Services.Interfaces;
using Microsoft.Playwright;
using NLog;

namespace GoIoFish.Services.Implementations
{
    public class GooFishService : IGooFishService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly IPlaywrightActorService _playwrightActorService;

        public GooFishService(IPlaywrightActorService playwrightActorService)
        {
            _playwrightActorService = playwrightActorService;
            _playwrightActorService.InitAsync(LoginAsync);
        }

        private async Task LoginAsync(IPage page)
        {
            Log.Info("进入网址...");
            await page.GotoAsync("https://www.goofish.com/", msg: "进入网址超时");
            await page.WaitForVisibleAsync("//div[text()='登录']", msg: "进入首页超时");
            await Task.Delay(3000);

            const string personalSelector = "//a[contains(@href, 'https://www.goofish.com/personal')]";
            var personalCount = await page.Locator(personalSelector).CountAsync();
            if (personalCount == 0)
            {
                Log.Info("扫码登录...");
                await page.WaitForAsync(personalSelector, timeout: 240000, isThrow: true, msg: "扫码登录超时");
            }

            Log.Info("登录成功");
        }

        public async Task<PageResult<Product>> FetchProductPageAsync(ProductPageRequest request)
        {
            Log.Info("分页获取商品...");
            var ret = await _playwrightActorService.EnqueueAsync(async page =>
            {
                await page.EvaluateAsync("");
                return PageResult<Product>.Ok(1, 20, 100, new List<Product>());
            });
            return ret;
        }
    }
}