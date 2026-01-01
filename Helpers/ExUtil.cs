using System;
using System.Threading.Tasks;

namespace GoIoFish.Helpers
{
    public static class ExUtil
    {
        public static void SafeExec(Action action, bool isThrow = false, string msg = null)
        {
            SafeExecAsync(() =>
            {
                action();
                return Task.CompletedTask;
            }, isThrow, msg).Wait();
        }

        public static async Task SafeExecAsync(Func<Task> asyncAction, bool isThrow = false, string msg = null)
        {
            try
            {
                await asyncAction();
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