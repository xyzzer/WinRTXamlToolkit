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

        //public string KeyGesture { get; set; }

        //public bool IsFocusRequired { get; set; }

        #region Invoked event
        /// <summary>
        /// Occurs when the command is invoked.
        /// </summary>
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
        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged; 
        #endregion
    }
}