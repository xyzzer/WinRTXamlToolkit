using System;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Primitive control - TextBox with UpPressed and DownPressed events for use in a NumericUpDown
    /// to make up and down keys work to increment and decrement the values.
    /// </summary>
    public class UpDownTextBox : TextBox
    {
        #region UpPressed event
        /// <summary>
        /// UpPressed event property.
        /// </summary>
        public event EventHandler UpPressed;

        /// <summary>
        /// Raises UpPressed event.
        /// </summary>
        private void RaiseUpPressed()
        {
            var handler = this.UpPressed;

            if (handler != null)
            {
                var args = EventArgs.Empty;
                handler(this, args);
            }
        }
        #endregion

        #region DownPressed event
        /// <summary>
        /// DownPressed event property.
        /// </summary>
        public event EventHandler DownPressed;

        /// <summary>
        /// Raises DownPressed event.
        /// </summary>
        private void RaiseDownPressed()
        {
            var handler = this.DownPressed;

            if (handler != null)
            {
                var args = EventArgs.Empty;
                handler(this, args);
            }
        }
        #endregion

        /// <summary>
        /// Called before the KeyDown event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            // Overriding OnKeyDown seems to be the only way to prevent selection to change when you press these keys.
            if (e.Key == VirtualKey.Up)
            {
                this.RaiseUpPressed();
                e.Handled = true;
                return;
            }

            if (e.Key == VirtualKey.Down)
            {
                this.RaiseDownPressed();
                e.Handled = true;
                return;
            }

            base.OnKeyDown(e);
        }
    }
}