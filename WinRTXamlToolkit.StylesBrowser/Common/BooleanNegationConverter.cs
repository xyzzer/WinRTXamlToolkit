﻿using System;
using Microsoft.UI.Xaml.Data;

namespace WinRTXamlToolkit.StylesBrowser.Common
{
    /// <summary>
    /// Value converter that translates true to false and vice versa.
    /// </summary>
    public sealed class BooleanNegationConverter : IValueConverter
    {
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property, specified by a helper structure that wraps the type name.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object. This method is called only in <c>TwoWay</c> bindings. 
        /// </summary>
        /// <param name="value">The target data being passed to the source..</param>
        /// <param name="targetType">The type of the target property, specified by a helper structure that wraps the type name.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }
    }
}
