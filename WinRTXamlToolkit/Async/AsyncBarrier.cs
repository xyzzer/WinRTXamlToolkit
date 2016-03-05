using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
    /// <summary>
    /// Enables multiple tasks to cooperatively work on an algorithm in parallel through multiple phases.
    /// </summary>
    public class AsyncBarrier
    {
        private readonly int _participantCount;
        private int _remainingParticipants;
        private ConcurrentStack<TaskCompletionSource<bool>> _waiters;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncBarrier" /> class.
        /// </summary>
        /// <param name="participantCount">The participant count.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">participantCount</exception>
        public AsyncBarrier(int participantCount)
        {
            if (participantCount <= 0) throw new ArgumentOutOfRangeException("participantCount");
            _remainingParticipants = _participantCount = participantCount;
            _waiters = new ConcurrentStack<TaskCompletionSource<bool>>();
        }

        /// <summary>
        /// Signals that a participant has reached the barrier and waits for all other participants to reach the barrier as well.
        /// </summary>
        /// <returns></returns>
        public Task SignalAndWait()
        {
            var tcs = new TaskCompletionSource<bool>();
            _waiters.Push(tcs);
            if (Interlocked.Decrement(ref _remainingParticipants) == 0)
            {
                _remainingParticipants = _participantCount;
                var waiters = _waiters;
                _waiters = new ConcurrentStack<TaskCompletionSource<bool>>();
                Parallel.ForEach(waiters, w => w.SetResult(true));
            }
            return tcs.Task;
        } 
    }
}
