using System;
using System.Windows.Input;

namespace WinRTXamlToolkit.Input
{
    /// <summary>
    /// TODO: Command to execute and associated key gesture.
    /// </summary>
    public class KeyCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the key gesture string associated with the command.
        /// </summary>
        public string KeyGestureString { get; set; }

        public string KeyGesture { get; set; }

        public bool IsFocusRequired { get; set; }

        #region Invoked event
        public event EventHandler Invoked;

        private void RaiseInvoked()
        {
            var handler = this.Invoked;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        } 
        #endregion

        #region ICommand implementation
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
        }

        public event EventHandler CanExecuteChanged; 
        #endregion
    }
}