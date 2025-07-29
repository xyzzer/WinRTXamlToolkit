using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WinRTXamlToolkit.StylesBrowser
{
    public static class StringUtilities
    {
        public static List<string> BreakCamelCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<string>();
            }

            // Use a regular expression to split CamelCased words
            var words = Regex.Split(input, @"(?<!^)(?=[A-Z])");

            // Return the results as a list
            return new List<string>(words);
        }
    }
}