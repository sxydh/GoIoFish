using System.Threading.Tasks;
using GoIoFish.Helpers;
using GoIoFish.Services.Interfaces;
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
        }

        public async Task<bool> LoginAsync()
        {
            return await _playwrightActorService.InitAsync(initCallback: async page =>
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
            });
        }
    }
}