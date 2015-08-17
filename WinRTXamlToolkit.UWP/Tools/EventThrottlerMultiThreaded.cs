using System;
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
    public class EventThrottlerMultiThreaded
    {
        private readonly object key = new object();
        private Func<Task> next;
        private bool isRunning;

        /// <summary>
        /// Runs the specified async action through the throttle.
        /// </summary>
        /// <param name="action">The async action.</param>
        public void Run(Func<Task> action)
        {
            bool shouldStartRunning = false;
            lock (key)
            {
                if (isRunning)
                    next = action;
                else
                {
                    isRunning = true;
                    shouldStartRunning = true;
                }
            }

            Action<Task> continuation = null;
            continuation = task =>
            {
                Func<Task> nextCopy = null;
                lock (key)
                {
                    if (next != null)
                    {
                        nextCopy = next;
                        next = null;
                    }
                    else
                    {
                        isRunning = false;
                    }
                }
                if (nextCopy != null)
                    nextCopy().ContinueWith(continuation, TaskScheduler.FromCurrentSynchronizationContext());
            };

            if (shouldStartRunning)
                action().ContinueWith(continuation, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
