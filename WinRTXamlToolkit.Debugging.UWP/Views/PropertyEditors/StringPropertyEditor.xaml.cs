using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Debugging.Views.PropertyEditors
{
    public sealed partial class StringPropertyEditor : UserControl
    {
        public StringPropertyEditor()
        {
            this.InitializeComponent();
        }

        private void EditTextBox_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            this.EditTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void EditTextBox_OnPaste(object sender, TextControlPasteEventArgs e)
        {
            this.EditTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
    }
}
