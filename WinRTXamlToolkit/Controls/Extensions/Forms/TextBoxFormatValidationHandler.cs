using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Handles validation of TextBox.Text whenever TextChanged property is raised.
    /// </summary>
    public class TextBoxFormatValidationHandler : FieldValidationHandler<TextBox>
    {
        internal void Detach()
        {
            Field.TextChanged -= OnTextBoxTextChanged;
            Field = null;
        }

        internal void Attach(TextBox textBox)
        {
            if (Field == textBox)
            {
                return;
            }

            if (Field != null)
            {
                this.Detach();
            }

            Field = textBox;
            Field.TextChanged += OnTextBoxTextChanged;

            this. Validate();
        }

        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            this.Validate();
        }

        protected override string GetFieldValue()
        {
            return Field.Text;
        }

        protected override int GetMaxLength()
        {
            return Field.MaxLength;
        }
    }
}
