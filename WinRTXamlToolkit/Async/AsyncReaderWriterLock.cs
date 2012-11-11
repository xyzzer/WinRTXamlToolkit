using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
    /// <summary>
    /// Defines a lock that supports single writers and multiple readers.
    /// </summary>
    /// <remarks>
    /// See <a href="http://blogs.msdn.com/b/pfxteam/archive/2012/02/12/building-async-coordination-primitives-part-7-asyncreaderwriterlock.aspx">
    /// http://blogs.msdn.com/b/pfxteam/archive/2012/02/12/building-async-coordination-primitives-part-7-asyncreaderwriterlock.aspx </a>
    /// </remarks>
    public class AsyncReaderWriterLock
    {
        private readonly Task<Releaser> _readerReleaser;
        private readonly Task<Releaser> _writerReleaser;
        private readonly Queue<TaskCompletionSource<Releaser>> _waitingWriters =
            new Queue<TaskCompletionSource<Releaser>>();
        private TaskCompletionSource<Releaser> _waitingReader =
            new TaskCompletionSource<Releaser>();
        private int _readersWaiting;
        private int _status;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncReaderWriterLock" /> class.
        /// </summary>
        public AsyncReaderWriterLock()
        {
            _readerReleaser = Task.FromResult(new Releaser(this, false));
            _writerReleaser = Task.FromResult(new Releaser(this, true));
        }

        /// <summary>
        /// ReaderLockAsync is used when a new reader wants in.
        /// </summary>
        /// <returns></returns>
        public Task<Releaser> ReaderLockAsync()
        {
            lock (_waitingWriters)
            {
                if (_status >= 0 && _waitingWriters.Count == 0)
                {
                    ++_status;
                    return _readerReleaser;
                }

                ++_readersWaiting;

                return _waitingReader.Task.ContinueWith(t => t.Result);
            }
        }

        /// <summary>
        /// WriterLockAsync is used when a new writer wants in.
        /// </summary>
        /// <returns></returns>
        public Task<Releaser> WriterLockAsync()
        {
            lock (_waitingWriters)
            {
                if (_status == 0)
                {
                    _status = -1;
                    return _writerReleaser;
                }

                var waiter = new TaskCompletionSource<Releaser>();
                _waitingWriters.Enqueue(waiter);

                return waiter.Task;
            }
        }

        /// <summary>
        /// Called when an active reader completes its work.
        /// </summary>
        private void ReaderRelease()
        {
            TaskCompletionSource<Releaser> toWake = null;

            lock (_waitingWriters)
            {
                --_status;
                if (_status == 0 && _waitingWriters.Count > 0)
                {
                    _status = -1;
                    toWake = _waitingWriters.Dequeue();
                }
            }

            if (toWake != null)
                toWake.SetResult(new Releaser(this, true));
        }

        /// <summary>
        /// Called when an active writer completes its work.
        /// </summary>
        private void WriterRelease()
        {
            TaskCompletionSource<Releaser> toWake = null;
            bool toWakeIsWriter = false;

            lock (_waitingWriters)
            {
                if (_waitingWriters.Count > 0)
                {
                    toWake = _waitingWriters.Dequeue();
                    toWakeIsWriter = true;
                }
                else if (_readersWaiting > 0)
                {
                    toWake = _waitingReader;
                    _status = _readersWaiting;
                    _readersWaiting = 0;
                    _waitingReader = new TaskCompletionSource<Releaser>();
                }
                else _status = 0;
            }

            if (toWake != null)
                toWake.SetResult(new Releaser(this, toWakeIsWriter));
        }

        /// <summary>
        /// Disposable Releaser to make it easy to use AsyncReaderWriterLock in a scoped manner with a using block.
        /// </summary>
        public struct Releaser : IDisposable
        {
            private readonly AsyncReaderWriterLock _toRelease;
            private readonly bool _writer;

            internal Releaser(AsyncReaderWriterLock toRelease, bool writer)
            {
                _toRelease = toRelease;
                _writer = writer;
            }

            public void Dispose()
            {
                if (_toRelease != null)
                {
                    if (_writer) _toRelease.WriterRelease();
                    else _toRelease.ReaderRelease();
                }
            }
        }
    }
}
