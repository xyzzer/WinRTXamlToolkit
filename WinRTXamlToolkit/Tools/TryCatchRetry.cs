using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;

namespace WinRTXamlToolkit.Tools
{
    /// <summary>
    /// Helper methods for running a specified action or task multiple times ignoring exceptions,
    /// until a retry count is reached.
    /// </summary>
    public static class TryCatchRetry
    {
        /// <summary>
        /// Runs the specified action until it succeeds or max number of retries is reached.
        /// </summary>
        /// <typeparam name="T">Type of expected exception.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="retries">The number of retries.</param>
        /// <param name="throwOnFail">if set to <c>true</c> - throws upon reaching the given max number of retries.</param>
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

        /// <summary>
        /// Runs the specified task until it succeeds or max number of retries is reached,
        /// with a delay in between retries.
        /// </summary>
        /// <typeparam name="T">Type of expected exception.</typeparam>
        /// <param name="task">The task.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="retries">The number of retries.</param>
        /// <param name="throwOnFail">if set to <c>true</c> - throws upon reaching the given max number of retries.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Runs the specified task until it succeeds or max number of retries is reached,
        /// with a delay in between retries and specific return type.
        /// </summary>
        /// <typeparam name="TException">Type of expected exception.</typeparam>
        /// <typeparam name="TResult">Returned result type.</typeparam>
        /// <param name="task">The task.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="retries">The number of retries.</param>
        /// <param name="throwOnFail">if set to <c>true</c> - throws upon reaching the given max number of retries.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Runs the specified operation until it succeeds or max number of retries is reached,
        /// with a delay in between retries and specific return type.
        /// </summary>
        /// <typeparam name="TException">Type of expected exception.</typeparam>
        /// <typeparam name="TResult">Returned result type.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="retries">The number of retries.</param>
        /// <param name="throwOnFail">if set to <c>true</c> - throws upon reaching the given max number of retries.</param>
        /// <returns></returns>
        public static async Task<TResult> RunWithDelayAsync<TException, TResult>(
            IAsyncOperation<TResult> operation,
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
                    return await operation;
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
