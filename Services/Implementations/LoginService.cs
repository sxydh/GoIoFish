using System;
using System.Threading.Tasks;
using GoIoFish.Helpers;
using GoIoFish.Services.Interfaces;
using Microsoft.Playwright;

namespace GoIoFish.Services.Implementations
{
    public class LoginService : ILoginService
    {
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
            await page.GotoAsync("https://www.goofish.com/", msg: "进入网址超时");
            await page.WaitForVisibleAsync("//div[text()='登录']", msg: "进入首页超时");
            await Task.Delay(1000);

            await page.ClickAsync("//div[text()='登录']", msg: "点击登录超时");
            const string qrCodeSelector = "//div[@id='qrcode-img']/canvas";
            await page.WaitForVisibleAsync(qrCodeSelector, msg: "登录二维码超时");
            var qrCodeCanvas = await page.QuerySelectorAsync(qrCodeSelector) ?? throw new Exception("获取登录二维码异常");
            var qrCodeCanvasBuffer = await qrCodeCanvas.ScreenshotAsync();
            var qrCodeBase64 = Convert.ToBase64String(qrCodeCanvasBuffer);
            Console.WriteLine(qrCodeBase64);
        }
    }
}