using System;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Tools
{
    public class EventThrottler<TResult>
    {
        private Tuple<Func<Task<TResult>>, TaskCompletionSource<TResult>> next;
        private bool isRunning;

        /// <summary>
        /// Runs the specified async action through the throttle.
        /// </summary>
        /// <param name="action">The async action.</param>
        /// <param name="skipResult">Result to return if a task gets skipped.</param>
        public async Task<TResult> RunAsync(Func<Task<TResult>> action, TResult skipResult)
        {
            // If there's anything queued up - cancel it so we can replace it.
            this.next?.Item2.SetCanceled();

            var tcs = new TaskCompletionSource<TResult>();
            this.next = new Tuple<Func<Task<TResult>>, TaskCompletionSource<TResult>>(action, tcs);

#pragma warning disable 4014
            this.EnsureRunningAsync();
#pragma warning restore 4014

            try
            {
                return await tcs.Task;
            }
            catch (TaskCanceledException)
            {
                return skipResult;
            }
        }

        private async Task EnsureRunningAsync()
        {
            if (!isRunning)
            {
                isRunning = true;

                try
                {
                    while (this.next != null)
                    {
                        var nextCopy = this.next;
                        this.next = null;
                        var task = nextCopy.Item1();
                        await task;

                        if (!nextCopy.Item2.Task.IsCanceled)
                        {
                            nextCopy.Item2.SetResult(task.Result);
                        }
                    }
                }
                finally
                {
                    isRunning = false;
                }
            }
        }
    }
}