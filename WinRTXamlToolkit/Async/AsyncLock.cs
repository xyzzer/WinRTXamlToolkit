using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
    /// <summary>
    /// Defines a critical section with a mutual-exclusion lock.
    /// </summary>
    public class AsyncLock
    {
        private readonly AsyncSemaphore _semaphore;
        private readonly Task<Releaser> _releaser;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLock" /> class.
        /// </summary>
        public AsyncLock()
        {
            _semaphore = new AsyncSemaphore(1);
            _releaser = Task.FromResult(new Releaser(this));
        }

        /// <summary>
        /// Waits for an open slot, then returns a disposable Releaser struct to be used in a using block marking the critical section.
        /// </summary>
        /// <returns></returns>
        public Task<Releaser> LockAsync()
        {
            var wait = _semaphore.WaitAsync();

            return wait.IsCompleted ?
                _releaser :
                wait.ContinueWith((_, state) => new Releaser((AsyncLock)state),
                    this, CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        /// <summary>
        /// Disposable Releaser to make it easy to use the AsyncLock in a scoped manner with a using block.
        /// </summary>
        public struct Releaser : IDisposable
        {
            private readonly AsyncLock _toRelease;

            internal Releaser(AsyncLock toRelease) { _toRelease = toRelease; }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_toRelease != null)
                    _toRelease._semaphore.Release();
            }
        }
    }
}
