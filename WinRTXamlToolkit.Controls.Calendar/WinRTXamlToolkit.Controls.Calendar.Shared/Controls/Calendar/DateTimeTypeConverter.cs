//TODO: See if this needs to be worked around
//// (c) Copyright Microsoft Corporation.
//// This source is subject to the Microsoft Public License (Ms-PL).
//// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//// All other rights reserved.

//using System;
//using System.ComponentModel;
//using System.Globalization;

//namespace WinRTXamlToolkit.Controls
//{
//    /// <summary>
//    /// Provides a converter to convert <see cref="T:System.DateTime" /> objects
//    /// to and from other representations.
//    /// </summary>
//    /// <QualityBand>Mature</QualityBand>
//    public class DateTimeTypeConverter : TypeConverter
//    {
//        /// <summary>
//        /// Initializes a new instance of the
//        /// <see cref="T:WinRTXamlToolkit.Controls.DateTimeTypeConverter" />
//        /// class.
//        /// </summary>
//        public DateTimeTypeConverter()
//        {
//        }

//        /// <summary>
//        /// Gets a value indicating whether it is possible to convert the
//        /// specified type to a <see cref="T:System.DateTime" /> with the
//        /// specified format context.
//        /// </summary>
//        /// <param name="context">
//        /// The format context that is used to convert the specified type.
//        /// </param>
//        /// <param name="sourceType">The type to convert.</param>
//        /// <returns>
//        /// True if the conversion is possible; otherwise, false.
//        /// </returns>
//        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
//        {
//            return sourceType == typeof(string);
//        }

//        /// <summary>
//        /// Converts the specified object to a <see cref="T:System.DateTime" />
//        /// with the specified culture with the specified format context.
//        /// </summary>
//        /// <param name="context">
//        /// The format context that is used to convert the specified type.
//        /// </param>
//        /// <param name="culture">The culture to use for the result.</param>
//        /// <param name="value">The value to convert.</param>
//        /// <returns>
//        /// A <see cref="T:System.DateTime" /> object that represents
//        /// <paramref name="value" />.
//        /// </returns>
//        /// <exception cref="System.FormatException">
//        /// The conversion cannot be performed.
//        /// </exception>
//        /// <exception cref="System.ArgumentNullException">
//        /// The culture is null.
//        /// </exception>
//        /// <exception cref="System.ArgumentNullException">
//        /// The value is null.
//        /// </exception>
//        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
//        {
//            if (culture == null)
//            {
//                throw new ArgumentNullException("culture");
//            }

//            if (value == null)
//            {
//                throw new ArgumentNullException("value");
//            }

//            DateTimeFormatInfo info = (DateTimeFormatInfo) culture.GetFormat(typeof(DateTimeFormatInfo));
//            return DateTime.ParseExact(value.ToString(), info.ShortDatePattern, culture);
//        }

//        /// <summary>
//        /// Gets a value indicating whether it is possible to convert a
//        /// <see cref="T:System.DateTime" /> to the specified type within the
//        /// specified format context.
//        /// </summary>
//        /// <param name="context">
//        /// The format context that is used to convert.
//        /// </param>
//        /// <param name="destinationType">The type to convert to.</param>
//        /// <returns>
//        /// True if the conversion is possible; otherwise, false.
//        /// </returns>
//        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
//        {
//            return destinationType == typeof(string);
//        }

//        /// <summary>
//        /// Converts a <see cref="T:System.DateTime" /> with the specified
//        /// culture to the specified object with the specified format context.
//        /// </summary>
//        /// <param name="context">
//        /// The format context that is used to convert to the specified type.
//        /// </param>
//        /// <param name="culture">
//        /// The culture to use for the converting date.
//        /// </param>
//        /// <param name="value">The date to convert.</param>
//        /// <param name="destinationType">The type to convert to.</param>
//        /// <returns>
//        /// An object of the specified type that represents
//        /// <paramref name="value" />.
//        /// </returns>
//        /// <exception cref="System.ArgumentNullException">
//        /// The culture is a null reference (Nothing in Visual Basic).
//        /// </exception>
//        /// <exception cref="System.ArgumentNullException">
//        /// The destinationType is a null reference (Nothing in Visual Basic).
//        /// </exception>
//        /// <exception cref="System.NotSupportedException">
//        /// The value is not a DateTime or the destinationType is not a string.
//        /// </exception>
//        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
//        {
//            if (destinationType == null)
//            {
//                throw new ArgumentNullException("destinationType");
//            }

//            if (culture == null)
//            {
//                throw new ArgumentNullException("culture");
//            }

//            DateTime? date = value as DateTime?;
//            if (!date.HasValue || destinationType != typeof(string))
//            {
//                throw new NotSupportedException();
//            }
//            else
//            {
//                DateTimeFormatInfo info = (DateTimeFormatInfo) culture.GetFormat(typeof(DateTimeFormatInfo));
//                return date.Value.ToString(info.ShortDatePattern, culture);
//            }
//        }
//    }
//}