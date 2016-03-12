namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class StubVisualTreeItemViewModel : VisualTreeItemViewModel
    {
        private readonly string _displayName;
        private readonly string _description;

        public string Description { get { return _description; } }

        public StubVisualTreeItemViewModel(
            VisualTreeViewModel treeViewModel,
            VisualTreeItemViewModel parent,
            string displayName = "Loading...",
            string description = "Please wait while more content is loaded...")
            : base(treeViewModel, parent)
        {
            _displayName = displayName;
            _description = description;
        }

        public override string ToString()
        {
            return _displayName;
        }
    }
}