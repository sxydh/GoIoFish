using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace GoIoFish.Helpers
{
    public static class PageUtil
    {
        public static async Task GotoAsync(this IPage page, string url, long timeout = 60000, bool isThrow = true, string msg = null)
        {
            try
            {
                await page.GotoAsync(url, new PageGotoOptions { Timeout = timeout });
            }
            catch (Exception e)
            {
                if (isThrow)
                {
                    throw new Exception(msg ?? e.Message, e);
                }
            }
        }
    }
}