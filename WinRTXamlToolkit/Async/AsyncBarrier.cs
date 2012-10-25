using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
    public class AsyncBarrier
    {
        private readonly int _participantCount;
        private int _remainingParticipants;
        private ConcurrentStack<TaskCompletionSource<bool>> _waiters;

        public AsyncBarrier(int participantCount)
        {
            if (participantCount <= 0) throw new ArgumentOutOfRangeException("participantCount");
            _remainingParticipants = _participantCount = participantCount;
            _waiters = new ConcurrentStack<TaskCompletionSource<bool>>();
        }

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
