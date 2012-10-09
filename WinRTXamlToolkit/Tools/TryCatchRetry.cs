using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Tools
{
    public static class TryCatchRetry
    {
        public static void Run<T>(
            Action action,
            int retries = 1,
            bool throwOnFail = false)
            where T : Exception
        {
            int attempts = 0;

            while (true)
            {
                attempts++;

                try
                {
                    action.Invoke();
                    return;
                }
                catch (T exception)
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }

                    Debug.WriteLine(exception);

                    if (attempts > retries)
                    {
                        if (throwOnFail)
                        {
                            throw;
                        }

                        return;
                    }
                }
            }
        }

        public static async Task RunWithDelayAsync<T>(
            Task task,
            TimeSpan delay,
            int retries = 1,
            bool throwOnFail = false)
            where T : Exception
        {
            int attempts = 0;

            while (true)
            {
                attempts++;

                try
                {
                    await task;
                    return;
                }
                catch (T exception)
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }

                    Debug.WriteLine(exception);

                    if (attempts > retries)
                    {
                        if (throwOnFail)
                        {
                            throw;
                        }

                        return;
                    }
                }

                await Task.Delay(delay);
            }
        }

        public static async Task<TResult> RunWithDelayAsync<TException, TResult>(
            Task<TResult> task,
            TimeSpan delay,
            int retries = 1,
            bool throwOnFail = false)
            where TException : Exception
        {
            int attempts = 0;

            while (true)
            {
                attempts++;

                try
                {
                    return await task;
                }
                catch (TException exception)
                {
                    //if (Debugger.IsAttached)
                    //{
                    //    Debugger.Break();
                    //}

                    Debug.WriteLine(exception);

                    if (attempts > retries)
                    {
                        if (throwOnFail)
                        {
                            throw;
                        }

                        return default(TResult);
                    }
                }

                await Task.Delay(delay);
            }
        }
    }
}
