using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
    public class AsyncCountdownEvent
    {
        private readonly AsyncManualResetEvent _amre = new AsyncManualResetEvent();
        private int _count;

        public AsyncCountdownEvent(int initialCount)
        {
            if (initialCount <= 0)
                throw new ArgumentOutOfRangeException("initialCount");
            _count = initialCount;
        }

        public Task WaitAsync() { return _amre.WaitAsync(); }

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

        public Task SignalAndWait()
        {
            Signal();
            return WaitAsync();
        }
    }
}
