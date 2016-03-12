namespace WinRTXamlToolkit.Debugging.ViewModels
{
    /// <summary>
    /// Base class of tree items in the VisualTreeViewModel.
    /// </summary>
    /// <remarks>
    /// Base class for DependencyObjectViewModel and StubTreeItemViewModel.
    /// </remarks>
    public class VisualTreeItemViewModel : TreeItemViewModel
    {
        public VisualTreeViewModel VisualTreeViewModel => (VisualTreeViewModel)this.TreeViewModel;

        public VisualTreeItemViewModel(
            VisualTreeViewModel treeViewViewModel,
            VisualTreeItemViewModel parent) : base(treeViewViewModel, parent)
        {
        }
    }
}