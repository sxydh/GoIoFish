using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace GoIoFish.Helpers
{
    public static class PageUtil
    {
        public static async Task GotoAsync(this IPage page, string url, long timeout = 60000, bool isThrow = true, string msg = null)
        {
            await ExUtil.SafeExecAsync(
                async () => await page.GotoAsync(url, new PageGotoOptions { Timeout = timeout }),
                isThrow, msg);
        }

        public static async Task WaitForAsync(this IPage page, string selector, long timeout = 60000, bool isThrow = true, string msg = null)
        {
            await WaitForAsync(page.Locator(selector), timeout: timeout, isThrow: isThrow, msg: msg);
        }

        public static async Task WaitForVisibleAsync(this IPage page, string selector, long timeout = 60000, bool isThrow = true, string msg = null)
        {
            await WaitForAsync(page.Locator(selector), state: WaitForSelectorState.Visible, timeout: timeout, isThrow: isThrow, msg: msg);
        }

        public static async Task WaitForAsync(this ILocator locator, WaitForSelectorState state = WaitForSelectorState.Attached, long timeout = 60000, bool isThrow = true, string msg = null)
        {
            await ExUtil.SafeExecAsync(
                async () => await locator.WaitForAsync(new LocatorWaitForOptions { State = state, Timeout = timeout }),
                isThrow, msg);
        }

        public static async Task ClickAsync(this IPage page, string selector, long timeout = 6000, bool isThrow = true, string msg = null)
        {
            await ExUtil.SafeExecAsync(
                async () => await page.Locator(selector).ClickAsync(new LocatorClickOptions { Timeout = timeout }),
                isThrow, msg);
        }

        public static async Task<IFrame> GetFrameAsync(this IPage page, string selector, long timeout = 60000, bool isThrow = true, string msg = null)
        {
            await page.WaitForAsync(selector, timeout: timeout, isThrow: isThrow, msg: msg);
            await Task.Delay(1000);
            return await ExUtil.SafeExecAsync(
                async () =>
                {
                    var ele = await page.QuerySelectorAsync(selector) ?? throw new NullReferenceException();
                    return await ele.ContentFrameAsync();
                },
                isThrow, msg);
        }

        public static async Task WaitForAsync(this IFrame frame, string selector, long timeout = 60000, bool isThrow = true, string msg = null)
        {
            await WaitForAsync(frame.Locator(selector), timeout: timeout, isThrow: isThrow, msg: msg);
        }

        public static async Task WaitForVisibleAsync(this IFrame frame, string selector, long timeout = 60000, bool isThrow = true, string msg = null)
        {
            await WaitForAsync(frame.Locator(selector), state: WaitForSelectorState.Visible, timeout: timeout, isThrow: isThrow, msg: msg);
        }
    }
}