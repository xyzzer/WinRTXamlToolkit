using Windows.UI.Xaml;
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
            Field.Loaded -= OnTextBoxLoaded;
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
            Field.Loaded += OnTextBoxLoaded;

            this.Validate();
        }

        private void OnTextBoxLoaded(object sender, RoutedEventArgs e)
        {
            this.Validate();
        }

        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            this.Validate();
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <returns></returns>
        protected override string GetFieldValue()
        {
            return Field.Text;
        }

        /// <summary>
        /// Gets the max length of the form field.
        /// </summary>
        /// <returns></returns>
        protected override int GetMaxLength()
        {
            return Field.MaxLength;
        }
    }
}
