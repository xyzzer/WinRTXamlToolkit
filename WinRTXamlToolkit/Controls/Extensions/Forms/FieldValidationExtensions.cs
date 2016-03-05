using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Attached properties for handling basic validation of field values.
    /// </summary>
    public static class FieldValidationExtensions
    {
        #region Format
        /// <summary>
        /// Format Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.RegisterAttached(
                "Format",
                typeof(ValidationChecks),
                typeof(FieldValidationExtensions),
                new PropertyMetadata(ValidationChecks.Any, OnFormatChanged));

        /// <summary>
        /// Gets the Format property. This dependency property 
        /// indicates the formats to validate for.
        /// </summary>
        public static ValidationChecks GetFormat(DependencyObject d)
        {
            return (ValidationChecks)d.GetValue(FormatProperty);
        }

        /// <summary>
        /// Sets the Format property. This dependency property 
        /// indicates the formats to validate for.
        /// </summary>
        public static void SetFormat(DependencyObject d, ValidationChecks value)
        {
            d.SetValue(FormatProperty, value);
        }

        /// <summary>
        /// Handles changes to the Format property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnFormatChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetupAndValidate(d);
        }
        #endregion

        #region Pattern
        /// <summary>
        /// Pattern Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty PatternProperty =
            DependencyProperty.RegisterAttached(
                "Pattern",
                typeof(string),
                typeof(FieldValidationExtensions),
                new PropertyMetadata(null, OnPatternChanged));

        /// <summary>
        /// Gets the Pattern property. This dependency property 
        /// indicates the regex pattern that the Text property needs to match.
        /// </summary>
        public static string GetPattern(DependencyObject d)
        {
            return (string)d.GetValue(PatternProperty);
        }

        /// <summary>
        /// Sets the Pattern property. This dependency property 
        /// indicates the regex pattern that the Text property needs to match.
        /// </summary>
        public static void SetPattern(DependencyObject d, string value)
        {
            d.SetValue(PatternProperty, value);
        }

        /// <summary>
        /// Handles changes to the Pattern property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnPatternChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetupAndValidate(d);
        }
        #endregion

        #region IsValid
        /// <summary>
        /// IsValid Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.RegisterAttached(
                "IsValid",
                typeof(bool),
                typeof(FieldValidationExtensions),
                new PropertyMetadata(true));

        /// <summary>
        /// Gets the IsValid property. This dependency property 
        /// indicates whether the field value is valid.
        /// </summary>
        public static bool GetIsValid(DependencyObject d)
        {
            return (bool)d.GetValue(IsValidProperty);
        }

        /// <summary>
        /// Sets the IsValid property. This dependency property 
        /// indicates whether the field value is valid.
        /// </summary>
        public static void SetIsValid(DependencyObject d, bool value)
        {
            d.SetValue(IsValidProperty, value);
        }
        #endregion

        #region NonEmptyErrorMessage
        /// <summary>
        /// NonEmptyErrorMessage Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty NonEmptyErrorMessageProperty =
            DependencyProperty.RegisterAttached(
                "NonEmptyErrorMessage",
                typeof(string),
                typeof(FieldValidationExtensions),
                new PropertyMetadata("Field can't be empty.", OnNonEmptyErrorMessageChanged));

        /// <summary>
        /// Gets the NonEmptyErrorMessage property. This dependency property 
        /// indicates the error message to use when the field is marked non-empty and is empty.
        /// </summary>
        public static string GetNonEmptyErrorMessage(DependencyObject d)
        {
            return (string)d.GetValue(NonEmptyErrorMessageProperty);
        }

        /// <summary>
        /// Sets the NonEmptyErrorMessage property. This dependency property 
        /// indicates the error message to use when the field is marked non-empty and is empty.
        /// </summary>
        public static void SetNonEmptyErrorMessage(DependencyObject d, string value)
        {
            d.SetValue(NonEmptyErrorMessageProperty, value);
        }

        /// <summary>
        /// Handles changes to the NonEmptyErrorMessage property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnNonEmptyErrorMessageChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetupAndValidate(d);
        }
        #endregion

        #region NumericErrorMessage
        /// <summary>
        /// NumericErrorMessage Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty NumericErrorMessageProperty =
            DependencyProperty.RegisterAttached(
                "NumericErrorMessage",
                typeof(string),
                typeof(FieldValidationExtensions),
                new PropertyMetadata("Field value needs to be numeric.", OnNumericErrorMessageChanged));

        /// <summary>
        /// Gets the NumericErrorMessage property. This dependency property 
        /// indicates the error message to use for a field marked numeric with non-numeric content.
        /// </summary>
        public static string GetNumericErrorMessage(DependencyObject d)
        {
            return (string)d.GetValue(NumericErrorMessageProperty);
        }

        /// <summary>
        /// Sets the NumericErrorMessage property. This dependency property 
        /// indicates the error message to use for a field marked numeric with non-numeric content.
        /// </summary>
        public static void SetNumericErrorMessage(DependencyObject d, string value)
        {
            d.SetValue(NumericErrorMessageProperty, value);
        }

        /// <summary>
        /// Handles changes to the NumericErrorMessage property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnNumericErrorMessageChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetupAndValidate(d);
        }
        #endregion

        #region SpecificLengthErrorMessage
        /// <summary>
        /// SpecificLengthErrorMessage Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty SpecificLengthErrorMessageProperty =
            DependencyProperty.RegisterAttached(
                "SpecificLengthErrorMessage",
                typeof(string),
                typeof(FieldValidationExtensions),
                new PropertyMetadata("Field needs to have {0} characters.", OnSpecificLengthErrorMessageChanged));

        /// <summary>
        /// Gets the SpecificLengthErrorMessage property. This dependency property 
        /// indicates the error message to use for a field with an incorrect number of characters.
        /// </summary>
        public static string GetSpecificLengthErrorMessage(DependencyObject d)
        {
            return (string)d.GetValue(SpecificLengthErrorMessageProperty);
        }

        /// <summary>
        /// Sets the SpecificLengthErrorMessage property. This dependency property 
        /// indicates the error message to use for a field with an incorrect number of characters.
        /// </summary>
        public static void SetSpecificLengthErrorMessage(DependencyObject d, string value)
        {
            d.SetValue(SpecificLengthErrorMessageProperty, value);
        }

        /// <summary>
        /// Handles changes to the SpecificLengthErrorMessage property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnSpecificLengthErrorMessageChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetupAndValidate(d);
        }
        #endregion

        #region PatternErrorMessage
        /// <summary>
        /// PatternErrorMessage Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty PatternErrorMessageProperty =
            DependencyProperty.RegisterAttached(
                "PatternErrorMessage",
                typeof(string),
                typeof(FieldValidationExtensions),
                new PropertyMetadata("The field needs to match pattern \"{0}\".", OnPatternErrorMessageChanged));

        /// <summary>
        /// Gets the PatternErrorMessage property. This dependency property 
        /// indicates the error message to use when the field text doesn't match requested pattern.
        /// </summary>
        public static string GetPatternErrorMessage(DependencyObject d)
        {
            return (string)d.GetValue(PatternErrorMessageProperty);
        }

        /// <summary>
        /// Sets the PatternErrorMessage property. This dependency property 
        /// indicates the error message to use when the field text doesn't match requested pattern.
        /// </summary>
        public static void SetPatternErrorMessage(DependencyObject d, string value)
        {
            d.SetValue(PatternErrorMessageProperty, value);
        }

        /// <summary>
        /// Handles changes to the PatternErrorMessage property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnPatternErrorMessageChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetupAndValidate(d);
        }
        #endregion

        #region MinLengthErrorMessage
        /// <summary>
        /// MinLengthErrorMessage Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinLengthErrorMessageProperty =
            DependencyProperty.RegisterAttached(
                "MinLengthErrorMessage",
                typeof(string),
                typeof(FieldValidationExtensions),
                new PropertyMetadata("Field needs to have at least {0} characters.", OnMinLengthErrorMessageChanged));

        /// <summary>
        /// Gets the MinLengthErrorMessage property. This dependency property 
        /// indicates the error message to use for field that doesn't match minimum length requirement.
        /// </summary>
        public static string GetMinLengthErrorMessage(DependencyObject d)
        {
            return (string)d.GetValue(MinLengthErrorMessageProperty);
        }

        /// <summary>
        /// Sets the MinLengthErrorMessage property. This dependency property 
        /// indicates the error message to use for field that doesn't match minimum length requirement.
        /// </summary>
        public static void SetMinLengthErrorMessage(DependencyObject d, string value)
        {
            d.SetValue(MinLengthErrorMessageProperty, value);
        }

        /// <summary>
        /// Handles changes to the MinLengthErrorMessage property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnMinLengthErrorMessageChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string oldMinLengthErrorMessage = (string)e.OldValue;
            string newMinLengthErrorMessage = (string)d.GetValue(MinLengthErrorMessageProperty);
        }
        #endregion

        #region DefaultErrorMessage
        /// <summary>
        /// DefaultErrorMessage Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty DefaultErrorMessageProperty =
            DependencyProperty.RegisterAttached(
                "DefaultErrorMessage",
                typeof(string),
                typeof(FieldValidationExtensions),
                new PropertyMetadata("Invalid field value", OnDefaultErrorMessageChanged));

        /// <summary>
        /// Gets the DefaultErrorMessage property. This dependency property 
        /// indicates the field value is invalid.
        /// </summary>
        public static string GetDefaultErrorMessage(DependencyObject d)
        {
            return (string)d.GetValue(DefaultErrorMessageProperty);
        }

        /// <summary>
        /// Sets the DefaultErrorMessage property. This dependency property 
        /// indicates the field value is invalid.
        /// </summary>
        public static void SetDefaultErrorMessage(DependencyObject d, string value)
        {
            d.SetValue(DefaultErrorMessageProperty, value);
        }

        /// <summary>
        /// Handles changes to the DefaultErrorMessage property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnDefaultErrorMessageChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string oldDefaultErrorMessage = (string)e.OldValue;
            string newDefaultErrorMessage = (string)d.GetValue(DefaultErrorMessageProperty);
        }
        #endregion

        #region ValidationMessage
        /// <summary>
        /// ValidationMessage Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValidationMessageProperty =
            DependencyProperty.RegisterAttached(
                "ValidationMessage",
                typeof(string),
                typeof(FieldValidationExtensions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the ValidationMessage property. This dependency property 
        /// indicates the validation error message if the value is not valid.
        /// </summary>
        public static string GetValidationMessage(DependencyObject d)
        {
            return (string)d.GetValue(ValidationMessageProperty);
        }

        /// <summary>
        /// Sets the ValidationMessage property. This dependency property 
        /// indicates the validation error message if the value is not valid.
        /// </summary>
        public static void SetValidationMessage(DependencyObject d, string value)
        {
            d.SetValue(ValidationMessageProperty, value);
        }
        #endregion

        #region ValidationMessageVisibility
        /// <summary>
        /// ValidationMessageVisibility Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValidationMessageVisibilityProperty =
            DependencyProperty.RegisterAttached(
                "ValidationMessageVisibility",
                typeof(Visibility),
                typeof(FieldValidationExtensions),
                new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Gets the ValidationMessageVisibility property. This dependency property 
        /// indicates the visibility of the validation message.
        /// </summary>
        public static Visibility GetValidationMessageVisibility(DependencyObject d)
        {
            return (Visibility)d.GetValue(ValidationMessageVisibilityProperty);
        }

        /// <summary>
        /// Sets the ValidationMessageVisibility property. This dependency property 
        /// indicates the visibility of the validation message.
        /// </summary>
        public static void SetValidationMessageVisibility(DependencyObject d, Visibility value)
        {
            d.SetValue(ValidationMessageVisibilityProperty, value);
        }
        #endregion

        #region FormatValidationHandler
        /// <summary>
        /// FormatValidationHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty FormatValidationHandlerProperty =
            DependencyProperty.RegisterAttached(
                "FormatValidationHandler",
                typeof(object),
                typeof(FieldValidationExtensions),
                new PropertyMetadata(null, OnFormatValidationHandlerChanged));

        /// <summary>
        /// Gets the FormatValidationHandler property. This dependency property 
        /// indicates the handler that checks field format.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static object GetFormatValidationHandler(DependencyObject d)
        {
            return (object)d.GetValue(FormatValidationHandlerProperty);
        }

        /// <summary>
        /// Sets the FormatValidationHandler property. This dependency property 
        /// indicates the handler that checks field format.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetFormatValidationHandler(DependencyObject d, object value)
        {
            d.SetValue(FormatValidationHandlerProperty, value);
        }

        /// <summary>
        /// Handles changes to the FormatValidationHandler property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnFormatValidationHandlerChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = d as TextBox;
            PasswordBox passwordBox = d as PasswordBox;

            if (textBox != null)
            {
                TextBoxFormatValidationHandler oldFormatValidationHandler = (TextBoxFormatValidationHandler)e.OldValue;
                TextBoxFormatValidationHandler newFormatValidationHandler = (TextBoxFormatValidationHandler)textBox.GetValue(FormatValidationHandlerProperty);

                if (oldFormatValidationHandler != null)
                {
                    oldFormatValidationHandler.Detach();
                }

                newFormatValidationHandler.Attach(textBox);
            }
            else if (passwordBox != null)
            {
                PasswordBoxFormatValidationHandler oldFormatValidationHandler = (PasswordBoxFormatValidationHandler)e.OldValue;
                PasswordBoxFormatValidationHandler newFormatValidationHandler = (PasswordBoxFormatValidationHandler)passwordBox.GetValue(FormatValidationHandlerProperty);

                if (oldFormatValidationHandler != null)
                {
                    oldFormatValidationHandler.Detach();
                }

                newFormatValidationHandler.Attach(passwordBox);
            }
        }
        #endregion

        #region ValidBrush
        /// <summary>
        /// ValidBrush Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValidBrushProperty =
            DependencyProperty.RegisterAttached(
                "ValidBrush",
                typeof(Brush),
                typeof(FieldValidationExtensions),
                new PropertyMetadata(new SolidColorBrush(Colors.White), OnValidBrushChanged));

        /// <summary>
        /// Gets the ValidBrush property. This dependency property 
        /// indicates the brush to use to highlight a successfully validated field.
        /// </summary>
        public static Brush GetValidBrush(DependencyObject d)
        {
            return (Brush)d.GetValue(ValidBrushProperty);
        }

        /// <summary>
        /// Sets the ValidBrush property. This dependency property 
        /// indicates the brush to use to highlight a successfully validated field.
        /// </summary>
        public static void SetValidBrush(DependencyObject d, Brush value)
        {
            d.SetValue(ValidBrushProperty, value);
        }

        /// <summary>
        /// Handles changes to the ValidBrush property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnValidBrushChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetupAndValidate(d);
        }
        #endregion

        #region InvalidBrush
        /// <summary>
        /// InvalidBrush Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty InvalidBrushProperty =
            DependencyProperty.RegisterAttached(
                "InvalidBrush",
                typeof(Brush),
                typeof(FieldValidationExtensions),
                new PropertyMetadata(new SolidColorBrush(Colors.Pink), OnInvalidBrushChanged));

        /// <summary>
        /// Gets the InvalidBrush property. This dependency property 
        /// indicates the brush to use to highlight a field with invalid value.
        /// </summary>
        public static Brush GetInvalidBrush(DependencyObject d)
        {
            return (Brush)d.GetValue(InvalidBrushProperty);
        }

        /// <summary>
        /// Sets the InvalidBrush property. This dependency property 
        /// indicates the brush to use to highlight a field with invalid value.
        /// </summary>
        public static void SetInvalidBrush(DependencyObject d, Brush value)
        {
            d.SetValue(InvalidBrushProperty, value);
        }

        /// <summary>
        /// Handles changes to the InvalidBrush property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnInvalidBrushChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetupAndValidate(d);
        }
        #endregion

        #region MinLength
        /// <summary>
        /// MinLength Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinLengthProperty =
            DependencyProperty.RegisterAttached(
                "MinLength",
                typeof(int),
                typeof(FieldValidationExtensions),
                new PropertyMetadata(8, OnMinLengthChanged));

        /// <summary>
        /// Gets the MinLength property. This dependency property 
        /// indicates minimum expected field value length if MinLength format is set.
        /// </summary>
        /// <remarks>
        /// The default value is 8 which matches a typical requirement for minimum password length.
        /// Specify another value if 8 is not the minimum value you expect when you set ValidationChecks.MinLength.
        /// </remarks>
        public static int GetMinLength(DependencyObject d)
        {
            return (int)d.GetValue(MinLengthProperty);
        }

        /// <summary>
        /// Sets the MinLength property. This dependency property 
        /// indicates minimum expected field value length if MinLength format is set.
        /// </summary>
        public static void SetMinLength(DependencyObject d, int value)
        {
            d.SetValue(MinLengthProperty, value);
        }

        /// <summary>
        /// Handles changes to the MinLength property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnMinLengthChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int oldMinLength = (int)e.OldValue;
            int newMinLength = (int)d.GetValue(MinLengthProperty);
        }
        #endregion

        private static void SetupAndValidate(DependencyObject dependencyObject)
        {
            //if (DesignMode.DesignModeEnabled)
            //{
            //    return;
            //}

            TextBox textBox = dependencyObject as TextBox;
            PasswordBox passwordBox = dependencyObject as PasswordBox;

            if (textBox != null)
            {
                TextBoxFormatValidationHandler handler;

                if ((handler = GetFormatValidationHandler(textBox) as TextBoxFormatValidationHandler) == null)
                {
                    handler = new TextBoxFormatValidationHandler();
                    SetFormatValidationHandler(textBox, handler);
                }
                else
                {
                    handler.Validate();
                }
            }
            else if (passwordBox != null)
            {
                PasswordBoxFormatValidationHandler handler;

                if ((handler = GetFormatValidationHandler(passwordBox) as PasswordBoxFormatValidationHandler) == null)
                {
                    handler = new PasswordBoxFormatValidationHandler();
                    SetFormatValidationHandler(passwordBox, handler);
                }
                else
                {
                    handler.Validate();
                }
            }
        }
    }
}
