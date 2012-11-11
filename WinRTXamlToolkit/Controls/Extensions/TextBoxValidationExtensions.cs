using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Defines basic formats accepted by TextBox Text.
    /// </summary>
    [Flags]
    public enum ValidTextBoxFormats
    {
        /// <summary>
        /// Any content is valid.
        /// </summary>
        Any = 0,
        /// <summary>
        /// Text property can't be empty.
        /// </summary>
        NonEmpty = 1,
        /// <summary>
        /// Text property needs to be numeric (parse to a double).
        /// </summary>
        Numeric = 2,
        /// <summary>
        /// Text property needs to be numeric (parse to a double) and non-empty.
        /// </summary>
        NonEmptyNumeric = 3
    }

    /// <summary>
    /// Attached properties for handling basic validation of TextBox Text.
    /// </summary>
    public static class TextBoxValidationExtensions
    {
        #region Format
        /// <summary>
        /// Format Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.RegisterAttached(
                "Format",
                typeof(ValidTextBoxFormats),
                typeof(TextBoxValidationExtensions),
                new PropertyMetadata(ValidTextBoxFormats.Any, OnFormatChanged));

        /// <summary>
        /// Gets the Format property. This dependency property 
        /// indicates the formats to validate for.
        /// </summary>
        public static ValidTextBoxFormats GetFormat(DependencyObject d)
        {
            return (ValidTextBoxFormats)d.GetValue(FormatProperty);
        }

        /// <summary>
        /// Sets the Format property. This dependency property 
        /// indicates the formats to validate for.
        /// </summary>
        public static void SetFormat(DependencyObject d, ValidTextBoxFormats value)
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
            ValidTextBoxFormats oldFormat = (ValidTextBoxFormats)e.OldValue;
            ValidTextBoxFormats newFormat = (ValidTextBoxFormats)d.GetValue(FormatProperty);

            SetupAndValidate((TextBox)d);
        }
        #endregion

        #region FormatValidationHandler
        /// <summary>
        /// FormatValidationHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty FormatValidationHandlerProperty =
            DependencyProperty.RegisterAttached(
                "FormatValidationHandler",
                typeof(TextBoxFormatValidationHandler),
                typeof(TextBoxValidationExtensions),
                new PropertyMetadata(null, OnFormatValidationHandlerChanged));

        /// <summary>
        /// Gets the FormatValidationHandler property. This dependency property 
        /// indicates the handler that checks the format in the TextBox.
        /// </summary>
        public static TextBoxFormatValidationHandler GetFormatValidationHandler(DependencyObject d)
        {
            return (TextBoxFormatValidationHandler)d.GetValue(FormatValidationHandlerProperty);
        }

        /// <summary>
        /// Sets the FormatValidationHandler property. This dependency property 
        /// indicates the handler that checks the format in the TextBox.
        /// </summary>
        public static void SetFormatValidationHandler(DependencyObject d, TextBoxFormatValidationHandler value)
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
            TextBoxFormatValidationHandler oldFormatValidationHandler = (TextBoxFormatValidationHandler)e.OldValue;
            TextBoxFormatValidationHandler newFormatValidationHandler = (TextBoxFormatValidationHandler)d.GetValue(FormatValidationHandlerProperty);

            if (oldFormatValidationHandler != null)
            {
                oldFormatValidationHandler.Detach();
            }

            newFormatValidationHandler.Attach((TextBox)d);
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
                typeof(TextBoxValidationExtensions),
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
            Brush oldValidBrush = (Brush)e.OldValue;
            Brush newValidBrush = (Brush)d.GetValue(ValidBrushProperty);
            SetupAndValidate((TextBox)d);
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
                typeof(TextBoxValidationExtensions),
                new PropertyMetadata(new SolidColorBrush(Colors.Pink), OnInvalidBrushChanged));

        /// <summary>
        /// Gets the InvalidBrush property. This dependency property 
        /// indicates the brush to use to highlight a TextBox with invalid text.
        /// </summary>
        public static Brush GetInvalidBrush(DependencyObject d)
        {
            return (Brush)d.GetValue(InvalidBrushProperty);
        }

        /// <summary>
        /// Sets the InvalidBrush property. This dependency property 
        /// indicates the brush to use to highlight a TextBox with invalid text.
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
            Brush oldInvalidBrush = (Brush)e.OldValue;
            Brush newInvalidBrush = (Brush)d.GetValue(InvalidBrushProperty);
            SetupAndValidate((TextBox)d);
        }
        #endregion

        private static void SetupAndValidate(TextBox textBox)
        {
            //if (DesignMode.DesignModeEnabled)
            //{
            //    return;
            //}

            TextBoxFormatValidationHandler handler = null;

            if ((handler = GetFormatValidationHandler(textBox)) == null)
            {
                handler = new TextBoxFormatValidationHandler();
                SetFormatValidationHandler(textBox, handler);
            }
            else
            {
                handler.Validate();
            }
        }
    }
}
