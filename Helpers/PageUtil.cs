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

        public static async Task WaitForAsync(this ILocator locator, WaitForSelectorState state = WaitForSelectorState.Attached, long timeout = 60000, bool isThrow = true, string msg = null)
        {
            await ExUtil.SafeExecAsync(
                async () => await locator.WaitForAsync(new LocatorWaitForOptions { State = state, Timeout = timeout }),
                isThrow, msg);
        }
    }
}