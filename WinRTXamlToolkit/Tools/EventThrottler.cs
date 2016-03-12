using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Tools
{
    /// <summary>
    /// Defines an object that can throttle tasks in such a way that if a task is already being processed when new tasks are queued -
    /// only the last one of these tasks will be executed when the currently running task is done and the other ones will be skipped.
    /// Very useful in many UI scenarios.
    /// </summary>
    /// <remarks>
    /// Written by Servy as an answer to this question on Stack Overflow:
    /// http://stackoverflow.com/questions/20081996/is-there-such-a-synchronization-tool-as-single-item-sized-async-task-buffer
    /// </remarks>
    public class EventThrottler
    {
        private Func<Task> next;
        private bool isRunning;

        /// <summary>
        /// Runs the specified async action through the throttle.
        /// </summary>
        /// <param name="action">The async action.</param>
        public async void Run(Func<Task> action)
        {
            if (isRunning)
            {
                this.next = action;
            }
            else
            {
                isRunning = true;

                try
                {
                    await action();

                    while (this.next != null)
                    {
                        var nextCopy = this.next;
                        this.next = null;
                        await nextCopy();
                    }
                }
                finally
                {
                    isRunning = false;
                }
            }
        }
    }

    public class EventThrottler<TResult>
    {
        private Tuple<Func<Task<TResult>>, TaskCompletionSource<TResult>> next;
        private bool isRunning;

        /// <summary>
        /// Runs the specified async action through the throttle.
        /// </summary>
        /// <param name="action">The async action.</param>
        public async Task<TResult> RunAsync(Func<Task<TResult>> action, TResult skipResult)
        {
            if (this.next != null)
            {
                this.next.Item2.SetCanceled();
            }

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
