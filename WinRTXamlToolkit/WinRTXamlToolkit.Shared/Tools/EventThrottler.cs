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
    public class EventThrottler
    {
        private Func<Task> next;
        private bool isRunning;

        public async void Run(Func<Task> action)
        {
            if (isRunning)
                next = action;
            else
            {
                isRunning = true;
                try
                {
                    await action();
                    while (next != null)
                    {
                        var nextCopy = next;
                        next = null;
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
}
