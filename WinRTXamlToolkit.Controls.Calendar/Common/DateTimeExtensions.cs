using System;
using System.Globalization;

namespace WinRTXamlToolkit.Common
{
    public static class DateTimeExtensions
    {
        public static string ToLongDateString(this DateTime dateTime)
        {
            return ToLongDateString(dateTime, CultureInfo.CurrentUICulture);
        }

        public static string ToLongDateString(this DateTime dateTime, CultureInfo culture)
        {
            return dateTime.ToString(culture.DateTimeFormat.LongDatePattern, culture);
        }

        public static string ToShortDateString(this DateTime dateTime)
        {
            return ToShortDateString(dateTime, CultureInfo.CurrentUICulture);
        }

        public static string ToShortDateString(this DateTime dateTime, CultureInfo culture)
        {
            return dateTime.ToString(culture.DateTimeFormat.ShortDatePattern, culture);
        }

        public static string ToLongTimeString(this DateTime dateTime)
        {
            return ToLongTimeString(dateTime, CultureInfo.CurrentUICulture);
        }

        public static string ToLongTimeString(this DateTime dateTime, CultureInfo culture)
        {
            return dateTime.ToString(culture.DateTimeFormat.LongTimePattern, culture);
        }

        public static string ToShortTimeString(this DateTime dateTime)
        {
            return ToShortTimeString(dateTime, CultureInfo.CurrentUICulture);
        }

        public static string ToShortTimeString(this DateTime dateTime, CultureInfo culture)
        {
            return dateTime.ToString(culture.DateTimeFormat.ShortTimePattern, culture);
        }
    }
}
