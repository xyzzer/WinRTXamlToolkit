using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    public class TextBoxFormatValidationHandler
    {
        private TextBox _textBox;

        internal void Detach()
        {
            _textBox = null;
            _textBox.TextChanged -= OnTextBoxTextChanged;
        }

        internal void Attach(TextBox textBox)
        {
            if (_textBox == textBox)
            {
                return;
            }

            if (_textBox != null)
            {
                this.Detach();
            }

            _textBox = textBox;
            _textBox.TextChanged += OnTextBoxTextChanged;

            this. Validate();
        }

        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            this.Validate();
        }

        internal void Validate()
        {
            var format = TextBoxValidationExtensions.GetFormat(_textBox);

            var expectNonEmpty = (format & ValidTextBoxFormats.NonEmpty) != 0;
            var isEmpty = string.IsNullOrWhiteSpace(_textBox.Text);

            if (expectNonEmpty && isEmpty)
            {
                MarkInvalid();
                return;
            }

            var expectNumber = (format & ValidTextBoxFormats.Numeric) != 0;

            if (expectNumber &&
                !isEmpty &&
                !IsNumeric())
            {
                MarkInvalid();
                return;
            }

            MarkValid();
        }

        private bool IsNumeric()
        {
            double number;
            return double.TryParse(_textBox.Text, out number);
        }

        protected virtual void MarkValid()
        {
            var brush = TextBoxValidationExtensions.GetValidBrush(_textBox);
            _textBox.Background = brush;
        }

        protected virtual void MarkInvalid()
        {
            var brush = TextBoxValidationExtensions.GetInvalidBrush(_textBox);
            _textBox.Background = brush;
        }
    }
}
