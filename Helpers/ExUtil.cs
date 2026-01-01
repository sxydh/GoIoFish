using System;
using System.Threading.Tasks;

namespace GoIoFish.Helpers
{
    public static class ExUtil
    {
        public static void SafeExec(Action action, bool isThrow = false, string msg = null)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                if (isThrow)
                {
                    throw new Exception(msg ?? e.Message, e);
                }
            }
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

        public static async Task<T> SafeExecAsync<T>(Func<Task<T>> asyncAction, bool isThrow = false, string msg = null)
        {
            try
            {
                return await asyncAction();
            }
            catch (Exception e)
            {
                if (isThrow)
                {
                    throw new Exception(msg ?? e.Message, e);
                }
            }

            return default;
        }
    }
}