using System;
using System.Threading.Tasks;

namespace GoIoFish.Helpers
{
    public static class ExUtil
    {
        public static void SafeExec(Action action)
        {
            try
            {
                action();
            }
            catch
            {
                // ignored
            }
        }

        public static async Task SafeExecAsync(Func<Task> asyncAction)
        {
            try
            {
                await asyncAction();
            }
            catch
            {
                // ignored
            }
        }
    }
}