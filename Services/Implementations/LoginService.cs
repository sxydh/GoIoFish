using System;
using System.Threading.Tasks;
using GoIoFish.Helpers;
using GoIoFish.Services.Interfaces;
using Microsoft.Playwright;
using NLog;

namespace GoIoFish.Services.Implementations
{
    public class LoginService : ILoginService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly IPlaywrightActorService _playwrightActorService;

        public LoginService(IPlaywrightActorService playwrightActorService)
        {
            _playwrightActorService = playwrightActorService;
        }

        public async Task<bool> LoginAsync()
        {
            return await _playwrightActorService.InitAsync(async page => await LoginDoAsync(page));
        }

        private async Task LoginDoAsync(IPage page)
        {
            Log.Info("进入网址...");
            await page.GotoAsync("https://www.goofish.com/", msg: "进入网址超时");
            await page.WaitForVisibleAsync("//div[text()='登录']", msg: "进入首页超时");
            await Task.Delay(3000);

            Log.Info("获取登录二维码...");
            await page.ClickAsync("//div[text()='登录']", msg: "点击登录超时");
            var qrCodeFrame = await page.GetFrameAsync("//iframe[@id='alibaba-login-box']", timeout: 6000, msg: "获取登录二维码框架超时");
            const string qrCodeSelector = "//div[@id='qrcode-img']/canvas";
            await qrCodeFrame.WaitForAsync(qrCodeSelector, timeout: 6000, msg: "获取登录二维码超时");
            var qrCodeCanvas = await qrCodeFrame.QuerySelectorAsync(qrCodeSelector) ?? throw new Exception("获取登录二维码异常");
            var qrCodeCanvasBuffer = await qrCodeCanvas.ScreenshotAsync();
            var qrCodeBase64 = Convert.ToBase64String(qrCodeCanvasBuffer);
            Log.Info($"获得登录二维码：{qrCodeBase64}");
        }
    }
}