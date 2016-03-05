namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class ResourceViewModel : BindableBase
    {
        public string Key { get;  }
        public object Resource { get; }

        public ResourceViewModel(string key, object resource)
        {
            this.Key = key;
            this.Resource = resource;
        }
    }
}
