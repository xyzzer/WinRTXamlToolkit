using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Tools
{
    /// <summary>
    /// A timer that raises the Tick events on a background thread,
    /// similar to ThreadPoolTimer, but with API compatible with
    /// DispatcherTimer.
    /// </summary>
    /// <remarks>
    /// It was written in response to this SO question: Controlled non UI timer in metro app (.NET)
    /// (http://stackoverflow.com/questions/10493253/controlled-non-ui-timer-in-metro-app-net).
    /// The purpose was to have ticks on a background thread and have
    /// the same API as the DispatcherTimer. It also has an option to self adjust a bit
    /// to have an average frequency closer to what you would expect given the configured Interval.
    /// That said I was not aware of the ThreadPoolTimer,
    /// so it is possible it makes more sense to use that one.
    /// It is a bit strange that the two timers we get have different APIs though,
    /// so perhaps that is one case where using the BackgroundTimer makes a little bit of sense.
    /// </remarks>
    public class BackgroundTimer : IDisposable
    {
        private ManualResetEvent _stopRequestEvent;
        private ManualResetEvent _stoppedEvent;

        /// <summary>
        /// Occurs when the timer interval has elapsed.
        /// </summary>
        public event EventHandler<object> Tick;

        #region Interval
        private TimeSpan _interval;
        /// <summary>
        /// Gets or sets the period of time between timer ticks.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
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
        /// <summary>
        /// Gets or sets a value indicating whether delays between Tick events should be adjusted to
        /// have Tick intervals averaging the given Interval property
        /// instead of being minimum of Interval property value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if delays should be adjusted; otherwise, <c>false</c>.
        /// </value>
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
        /// <summary>
        /// Gets or sets a value that indicates whether the timer is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundTimer" /> class.
        /// </summary>
        public BackgroundTimer()
        {
            _stopRequestEvent = new ManualResetEvent(false);
            _stoppedEvent = new ManualResetEvent(false);
        }

        /// <summary>
        /// Starts the BackgroundTimer.
        /// </summary>
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

        /// <summary>
        /// Stops the BackgroundTimer. Waits for it to stop before returning.
        /// </summary>
        public void Stop()
        {
            if (!_isEnabled)
            {
                return;
            }

            _isEnabled = false;
            _stoppedEvent.Reset();
            _stopRequestEvent.Set();
            _stoppedEvent.WaitOne();
        }

        /// <summary>
        /// Stops the BackgroundTimer - non blocking.
        /// </summary>
        public void StopNonBlocking()
        {
            if (!_isEnabled)
            {
                return;
            }

            _isEnabled = false;
            _stopRequestEvent.Set();
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

                if (effectiveInterval > TimeSpan.Zero)
                {
                    _stopRequestEvent.WaitOne(effectiveInterval);
                }

                if (_isEnabled &&
                    Tick != null)
                {
                    Tick(this, null);
                }

                tickCount++;
            }

            _stoppedEvent.Set();
        }

        public void Dispose()
        {
            if (_stopRequestEvent != null)
            {
                _stopRequestEvent.Dispose();
                _stopRequestEvent = null;
            }

            if (_stoppedEvent != null)
            {
                _stoppedEvent.Dispose();
                _stoppedEvent = null;
            }
        }

        ~BackgroundTimer()
        {
            Dispose();
        }
    }
}
