using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
    /// <summary>
    /// Notifies one or more waiting awaiters that an event has occurred
    /// </summary>
    public class AsyncAutoResetEvent
    {
        private readonly static Task CompletedTask = Task.FromResult(true);
        private readonly Queue<TaskCompletionSource<bool>> _waits = new Queue<TaskCompletionSource<bool>>();
        private bool _signaled;

        /// <summary>
        /// Initializes a new instance of the AsyncAutoResetEvent class,
        /// specifying whether the wait handle is initially signaled.
        /// </summary>
        /// <param name="initialState">true to set the initial state to signaled; false to set it to nonsignaled.</param>
        public AsyncAutoResetEvent(bool initialState = false)
        {
            _signaled = initialState;
        }

        /// <summary>
        /// Blocks the current task until the current AsyncAutoResetEvent receives a signal.
        /// </summary>
        /// <returns></returns>
        public Task WaitAsync()
        {
            lock (_waits)
            {
                if (_signaled)
                {
                    _signaled = false;
                    return CompletedTask;
                }

                var tcs = new TaskCompletionSource<bool>();
                _waits.Enqueue(tcs);
                return tcs.Task;
            }
        }

        /// <summary>
        /// Sets the state of the event to nonsignaled, causing task returned by the next WaitAsync call to block.
        /// </summary>
        public void Reset()
        {
            _signaled = false;
        }

        /// <summary>
        /// Sets the state of the event to signaled, allowing one or more waiting threads to proceed.
        /// </summary>
        public void Set()
        {
            TaskCompletionSource<bool> toRelease = null;

            lock (_waits)
            {
                if (_waits.Count > 0)
                    toRelease = _waits.Dequeue();
                else if (!_signaled)
                    _signaled = true;
            }

            if (toRelease != null)
                toRelease.SetResult(true);
        }
    }
}
