using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls.Extensions.Forms
{
    public class FocusTracker
    {
        private const int DefaultFocusPollingInterval = 100;

        /// <summary>
        /// Gets or sets the focus polling interval in ms.
        /// </summary>
        /// <value>
        /// The focus polling interval.
        /// </value>
        public int FocusPollingInterval { get; set; }

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

        public FocusTracker()
        {
            FocusPollingInterval = DefaultFocusPollingInterval;

#pragma warning disable 4014
            RunFocusTrackingAsync();
#pragma warning restore 4014
        }

        private async Task RunFocusTrackingAsync()
        {
            var focusedElement = FocusManager.GetFocusedElement() as UIElement;
            var lastFocusedElement = focusedElement;

            while (true)
            {
                if (focusedElement != null)
                {
                    await focusedElement.WaitForLostFocusAsync();
                    focusedElement = FocusManager.GetFocusedElement() as UIElement;
                    RaiseFocusChanged(focusedElement);
                }
                else
                {
                    await Task.Delay(FocusPollingInterval);
                    focusedElement = FocusManager.GetFocusedElement() as UIElement;

                    if (lastFocusedElement != focusedElement)
                    {
                        RaiseFocusChanged(focusedElement);
                    }
                }

                lastFocusedElement = focusedElement;
            }
        }
    }
}
