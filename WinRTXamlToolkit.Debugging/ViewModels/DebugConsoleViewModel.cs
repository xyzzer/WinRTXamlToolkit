namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class DebugConsoleViewModel : BindableBase
    {
        #region Opacity
        private double _opacity = 1.0;
        public double Opacity
        {
            get { return _opacity; }
            set { this.SetProperty(ref _opacity, value); }
        }
        #endregion

        #region IsExpanded
        private bool _isExpanded = true;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { this.SetProperty(ref _isExpanded, value); }
        }
        #endregion

        #region VisualTreeView
        private VisualTreeViewModel _visualTreeView = new VisualTreeViewModel();
        public VisualTreeViewModel VisualTreeView
        {
            get { return _visualTreeView; }
            set { this.SetProperty(ref _visualTreeView, value); }
        }
        #endregion
    }
}
