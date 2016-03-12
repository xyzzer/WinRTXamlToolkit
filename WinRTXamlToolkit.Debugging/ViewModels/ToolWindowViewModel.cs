using System;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class ToolWindowViewModel : BindableBase
    {
        public event EventHandler Removed;

        #region Remove()
        internal virtual void Remove()
        {
            DebugConsoleViewModel.Instance.ToolWindows.Remove(this);
            this.Removed?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
