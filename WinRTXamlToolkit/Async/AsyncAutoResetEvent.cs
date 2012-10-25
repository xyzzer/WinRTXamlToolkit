using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
    public class AsyncAutoResetEvent
    {
        private static readonly object SyncObject = new object();
        private TaskCompletionSource<bool> _taskCompletionSource;

        /// <summary>
        /// Initializes a new instance of the AsyncAutoResetEvent class,
        /// specifying whether the wait handle is initially signaled.
        /// </summary>
        /// <param name="initialState">true to set the initial state to signaled; false to set it to nonsignaled.</param>
        public AsyncAutoResetEvent(bool initialState = false)
        {
            _taskCompletionSource = new TaskCompletionSource<bool>();

            if (initialState)
            {
                _taskCompletionSource.SetResult(true);
            }
        }

        /// <summary>
        /// Sets the state of the event to signaled, allowing one or more waiting threads to proceed.
        /// </summary>
        public void Set()
        {
            lock (SyncObject)
            {
                _taskCompletionSource.TrySetResult(true);
            }
        }

        /// <summary>
        /// Sets the state of the event to nonsignaled, causing threads to block.
        /// </summary>
        public void Reset()
        {
            lock (SyncObject)
            {
                _taskCompletionSource = new TaskCompletionSource<bool>();
            }
        }

        /// <summary>
        /// Blocks the current thread until the current AsyncAutoResetEvent receives a signal.
        /// </summary>
        /// <returns></returns>
        public async Task WaitAsync()
        {
            await _taskCompletionSource.Task;

            Reset();
        }
    }


    // Stephen Toub's version
    //public class AsyncAutoResetEvent
    //{
    //    private readonly static Task CompletedTask = Task.FromResult(true);
    //    private readonly Queue<TaskCompletionSource<bool>> _waits = new Queue<TaskCompletionSource<bool>>();
    //    private bool _signaled; 

    //    public Task WaitAsync()
    //    {
    //        lock (_waits)
    //        {
    //            if (_signaled)
    //            {
    //                _signaled = false;
    //                return CompletedTask;
    //            }

    //            var tcs = new TaskCompletionSource<bool>();
    //            _waits.Enqueue(tcs);
    //            return tcs.Task;
    //        }
    //    }

    //    public void Set()
    //    {
    //        TaskCompletionSource<bool> toRelease = null;

    //        lock (_waits)
    //        {
    //            if (_waits.Count > 0)
    //                toRelease = _waits.Dequeue();
    //            else if (!_signaled)
    //                _signaled = true;
    //        }

    //        if (toRelease != null)
    //            toRelease.SetResult(true); 
    //    }
    //}
}
