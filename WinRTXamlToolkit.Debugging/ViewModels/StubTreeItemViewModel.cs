namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class StubTreeItemViewModel : TreeItemViewModel
    {
        private readonly string _displayName;
        private readonly string _description;

        public string Description { get { return _description; } }

        public StubTreeItemViewModel(
            VisualTreeViewModel treeModel,
            TreeItemViewModel parent,
            string displayName = "Loading...",
            string description = "Please wait while more content is loaded...")
            : base(treeModel, parent)
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