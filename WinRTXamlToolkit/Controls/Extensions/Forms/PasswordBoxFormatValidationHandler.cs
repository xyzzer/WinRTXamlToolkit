using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Handles validation of PasswordBox.Text whenever TextChanged property is raised.
    /// </summary>
    public class PasswordBoxFormatValidationHandler : FieldValidationHandler<PasswordBox>
    {
        internal void Detach()
        {
            Field.PasswordChanged -= OnPasswordChanged;
            Field = null;
        }

        internal void Attach(PasswordBox passwordBox)
        {
            if (Field == passwordBox)
            {
                return;
            }

            if (Field != null)
            {
                this.Detach();
            }

            Field = passwordBox;
            Field.PasswordChanged += OnPasswordChanged;

            this. Validate();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Validate();
        }

        protected override string GetFieldValue()
        {
            return Field.Password;
        }

        protected override int GetMaxLength()
        {
            return Field.MaxLength;
        }
    }
}
