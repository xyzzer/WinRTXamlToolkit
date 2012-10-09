using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Tools
{
    public class BackgroundTimer
    {
        private readonly AutoResetEvent _stopRequestEvent;
        private readonly AutoResetEvent _stoppedEvent;

        public event EventHandler<object> Tick;

        #region Interval
        private TimeSpan _interval;
        public TimeSpan Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                if (IsEnabled)
                {
                    Stop();
                    _interval = value;
                    Start();
                }
                else
                {
                    _interval = value;
                }
            }
        }
        #endregion

        #region AdjustDelays
        private bool _adjustDelays = true;
        public bool AdjustDelays
        {
            get
            {
                return _adjustDelays;
            }
            set
            {
                if (IsEnabled)
                {
                    Stop();
                    _adjustDelays = value;
                    Start();
                }
                else
                {
                    _adjustDelays = value;
                }
            }
        }
        #endregion

        #region IsEnabled
        private bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled == value)
                    return;

                if (value)
                    Start();
                else
                    Stop();
            }
        }
        #endregion

        public BackgroundTimer()
        {
            _stopRequestEvent = new AutoResetEvent(false);
            _stoppedEvent = new AutoResetEvent(false);
        }

        public void Start()
        {
            if (_isEnabled)
            {
                return;
            }

            _isEnabled = true;
            _stopRequestEvent.Reset();

            if (_adjustDelays)
                Task.Run((Action)RunAdjusted);
            else
                Task.Run((Action)Run);
        }

        public void Stop()
        {
            if (!_isEnabled)
            {
                return;
            }

            _isEnabled = false;
            _stopRequestEvent.Set();
            _stoppedEvent.WaitOne();
        }

        private void Run()
        {
            while (_isEnabled)
            {
                _stopRequestEvent.WaitOne(_interval);

                if (_isEnabled &&
                    Tick != null)
                {
                    Tick(this, null);
                }
            }

            _stoppedEvent.Set();
        }

        private void RunAdjusted()
        {
            var start = DateTime.Now;
            long tickCount = 0;

            while (_isEnabled)
            {
                var waitStart = DateTime.Now;
                var timeRunning = waitStart - start;
                var effectiveInterval = TimeSpan.FromSeconds(_interval.TotalSeconds * (tickCount + 1)) - timeRunning;

                _stopRequestEvent.WaitOne(effectiveInterval);

                if (_isEnabled &&
                    Tick != null)
                {
                    Tick(this, null);
                }

                tickCount++;
            }

            _stoppedEvent.Set();
        }
    }
}
