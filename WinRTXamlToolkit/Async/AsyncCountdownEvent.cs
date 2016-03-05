using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
    /// <summary>
    /// Represents a synchronization primitive that is signaled when its count reaches zero.
    /// </summary>
    public class AsyncCountdownEvent
    {
        private readonly AsyncManualResetEvent _amre = new AsyncManualResetEvent();
        private int _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCountdownEvent" /> class.
        /// </summary>
        /// <param name="initialCount">The initial count.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">initialCount</exception>
        public AsyncCountdownEvent(int initialCount)
        {
            if (initialCount <= 0)
            {
                throw new ArgumentOutOfRangeException("initialCount");
            }

            _count = initialCount;
        }

        /// <summary>
        /// Waits for the countdown completion signal.
        /// </summary>
        /// <returns></returns>
        public Task WaitAsync() { return _amre.WaitAsync(); }

        /// <summary>
        /// Registers a signal, decrementing the current count.
        /// </summary>
        public void Signal()
        {
            if (_count <= 0)
                throw new InvalidOperationException();

            int newCount = Interlocked.Decrement(ref _count);
            if (newCount == 0)
                _amre.Set();
            else if (newCount < 0)
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Registers a signal, then waits for countdown completion.
        /// </summary>
        /// <returns></returns>
        public Task SignalAndWait()
        {
            Signal();
            return WaitAsync();
        }
    }
}
