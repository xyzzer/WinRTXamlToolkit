using System.Collections.ObjectModel;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class DebugConsoleViewModel : BindableBase
    {
        #region Instance (singleton implementation)
        private static DebugConsoleViewModel _instance;
        public static DebugConsoleViewModel Instance
        {
            get { return _instance ?? (_instance = new DebugConsoleViewModel()); }
        }

        private DebugConsoleViewModel()
        {
        }
        #endregion

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
        private VisualTreeViewModel _visualTreeView = VisualTreeViewModel.Instance;
        public VisualTreeViewModel VisualTreeView
        {
            get { return _visualTreeView; }
            set { this.SetProperty(ref _visualTreeView, value); }
        }
        #endregion

        #region ToolWindows
        private ObservableCollection<ToolWindowViewModel> toolWindows = new ObservableCollection<ToolWindowViewModel>();
        /// <summary>
        /// Gets or sets the list of tool window view models.
        /// </summary>
        public ObservableCollection<ToolWindowViewModel> ToolWindows
        {
            get { return this.toolWindows; }
            set { this.SetProperty(ref this.toolWindows, value); }
        }
        #endregion
    }
}
