using System;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Tools
{
    public class TimeoutCheck : IDisposable
    {
        private readonly DispatcherTimer _timeoutTimer;
        public bool ThrowOnTimeout { get; set; }
        public bool BreakOnTimeout { get; set; }
        public bool AttachOnTimeout { get; set; }

        /// <summary>
        /// If true - in release build configuration only the Timeout event will be raised when timeout occurs.
        /// </summary>
        public bool DebugOnly { get; set; }
        public object Subject { get; set; }
        public event EventHandler<object> Timeout;

        public TimeSpan Interval
        {
            get
            {
                return _timeoutTimer.Interval;
            }
            set
            {
                _timeoutTimer.Interval = value;
            }
        }

        public TimeoutCheck(object subject, int timeoutInMilliseconds = 1000, bool autoStart = true)
        {
            Debug.Assert(subject != null);

            this.DebugOnly = true;
            this.BreakOnTimeout = true;
#if DEBUG
            this.AttachOnTimeout = true;
#endif
            this.Subject = subject;
            _timeoutTimer = new DispatcherTimer();
            _timeoutTimer.Tick += OnTimeout;
            this.Interval = TimeSpan.FromMilliseconds(timeoutInMilliseconds);

            if (autoStart)
            {
                Start();
            }
        }

        private void OnTimeout(object sender, object o)
        {
            var timeoutHandler = Timeout;

            if (timeoutHandler != null)
            {
                timeoutHandler(this, this.Subject);
            }

            if (!this.DebugOnly
#if DEBUG
                || true
#endif
                )
            {
                if (this.BreakOnTimeout && Debugger.IsAttached)
                {
                    Debugger.Break();
                }
                else if (this.AttachOnTimeout)
                {
                    Debugger.Launch();
                }
                else if (this.ThrowOnTimeout)
                {
                    throw new TimeoutException("Timeout occured for " + this.Subject);
                }
            }
        }

        private CoreDispatcher _dispatcher;
        public void Start()
        {
            _dispatcher = Window.Current.Dispatcher;
            _timeoutTimer.Start();
        }

        public void Stop()
        {
            if (_dispatcher.HasThreadAccess)
            {
                _timeoutTimer.Stop();
            }
            else
            {
                _dispatcher.RunAsync(CoreDispatcherPriority.High, Stop);
            }
        }

        ~TimeoutCheck()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(Boolean disposing)
        {
            Stop();
        }
    }
}
