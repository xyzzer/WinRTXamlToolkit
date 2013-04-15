using System;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Tools
{
    /// <summary>
    /// Allows to mark a specific async block as one that can time out.
    /// </summary>
    public class TimeoutCheck : IDisposable
    {
        private readonly DispatcherTimer _timeoutTimer;

        /// <summary>
        /// Gets or sets a value indicating whether a TimeoutException
        /// should be thrown when timeout occurs.
        /// </summary>
        /// <value>
        ///   <c>true</c> if TimeoutException should be thrown on timeout; otherwise, <c>false</c>.
        /// </value>
        public bool ThrowOnTimeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether execution should break when timeout occurs.
        /// </summary>
        /// <value>
        ///   <c>true</c> if debugger should break on timeout; otherwise, <c>false</c>.
        /// </value>
        public bool BreakOnTimeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether debugger should get attached and break when timeout occurs.
        /// </summary>
        /// <value>
        ///   <c>true</c> if debugger should get atteched on timeout; otherwise, <c>false</c>.
        /// </value>
        public bool AttachOnTimeout { get; set; }

        /// <summary>
        /// If true - in release build configuration only the Timeout event will be raised when timeout occurs.
        /// </summary>
        public bool DebugOnly { get; set; }

        /// <summary>
        /// Gets or sets the subject of the timeout to use in reporting the timeout itself.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public object Subject { get; set; }

        /// <summary>
        /// Occurs when timeout occurs.
        /// </summary>
        public event EventHandler<object> Timeout;

        /// <summary>
        /// Gets or sets timeout interval.
        /// </summary>
        /// <value>
        /// The timeout interval.
        /// </value>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutCheck" /> class.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="timeoutInMilliseconds">The timeout in milliseconds.</param>
        /// <param name="autoStart">if set to <c>true</c> - timeout check will start automatically.</param>
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

        /// <summary>
        /// Starts a timeout check.
        /// </summary>
        public void Start()
        {
            _dispatcher = Window.Current.Dispatcher;
            _timeoutTimer.Start();
        }

        /// <summary>
        /// Stops the timeout check.
        /// </summary>
        /// <remarks>
        /// Typically called when an awaited operation completes before the timeout.
        /// </remarks>
        public void Stop()
        {
            if (_dispatcher.HasThreadAccess)
            {
                _timeoutTimer.Stop();
            }
            else
            {
#pragma warning disable 4014
                _dispatcher.RunAsync(CoreDispatcherPriority.High, Stop);
#pragma warning restore 4014
            }
        }

        ~TimeoutCheck()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
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
