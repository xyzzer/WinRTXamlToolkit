using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Debugging.WinRTProxy
{
    /// <summary>
    /// This control isn't used anywhere, but having it here triggers generation of the XamlTypeInfo.g.cs file
    /// which is necessary to enable loading XAML types from referenced libraries.
    /// </summary>
    public sealed partial class ImportantUserControl : UserControl
    {
        public ImportantUserControl()
        {
            this.InitializeComponent();
        }
    }
}
