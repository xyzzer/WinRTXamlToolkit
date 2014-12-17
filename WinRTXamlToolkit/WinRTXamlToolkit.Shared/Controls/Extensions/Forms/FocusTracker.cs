using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls.Extensions.Forms
{
    public class FocusTracker
    {
        private UIElement focusedElement;
        private const int DefaultFocusPollingInterval = 100;
        private DispatcherTimer timer;

        #region FocusPollingInterval
        private int focusPollingInterval;

        /// <summary>
        /// Gets or sets the focus polling interval in ms.
        /// </summary>
        /// <value>
        /// The focus polling interval.
        /// </value>
        public int FocusPollingInterval
        {
            get
            {
                return focusPollingInterval;
            }
            set
            {
                if (focusPollingInterval != value)
                {
                    focusPollingInterval = value;

                    if (this.timer != null)
                    {
                        this.timer.Interval = TimeSpan.FromMilliseconds(focusPollingInterval);
                    }
                }
            }
        }
        #endregion

        #region FocusChanged event
        /// <summary>
        /// FocusChanged event property.
        /// </summary>
        public event EventHandler<UIElement> FocusChanged;

        /// <summary>
        /// Raises FocusChanged event.
        /// </summary>
        private void RaiseFocusChanged(UIElement e)
        {
            var handler = this.FocusChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region CTOR
        public FocusTracker()
        {
            FocusPollingInterval = DefaultFocusPollingInterval;

#pragma warning disable 4014
            RunFocusTrackingAsync();
#pragma warning restore 4014
        }
        #endregion

        #region RunFocusTrackingAsync()
        private async Task RunFocusTrackingAsync()
        {
            this.UpdateFocusedElement();
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(FocusPollingInterval);
            this.timer.Tick += this.OnTick;
            this.timer.Start();
        }
        #endregion

        #region OnLostFocus()
        private void OnLostFocus(object sender, RoutedEventArgs args)
        {
            this.UpdateFocusedElement();
        }
        #endregion

        #region OnTick()
        private void OnTick(object sender, object o)
        {
            this.UpdateFocusedElement();
        }
        #endregion

        #region UpdateFocusedElement()
        private void UpdateFocusedElement()
        {
            var newFocusedElement = FocusManager.GetFocusedElement() as UIElement;

            if (this.focusedElement == newFocusedElement)
            {
                return;
            }

            if (this.focusedElement != null)
            {
                this.focusedElement.LostFocus -= this.OnLostFocus;
            }

            this.focusedElement = newFocusedElement;
            RaiseFocusChanged(this.focusedElement);

            if (this.focusedElement != null)
            {
                this.focusedElement.LostFocus += this.OnLostFocus;
            }
        }
        #endregion
    }
}
