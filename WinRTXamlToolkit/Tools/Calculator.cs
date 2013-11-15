using System;
using System.Collections.Generic;
using System.Globalization;

namespace WinRTXamlToolkit.Tools
{
    public static class Calculator
    {
        private static Func<double, double, double> AddFunc = (d0, d1) => d0 + d1;
        private static Func<double, double, double> SubtractFunc = (d0, d1) => d0 - d1;
        private static Func<double, double, double> MultiplyFunc = (d0, d1) => d0 * d1;
        private static Func<double, double, double> DivideFunc = (d0, d1) => d0 / d1;
        private static Func<double, double, double> ModuloFunc = (d0, d1) => d0 % d1;
        private static Func<double, double, double> PowerFunc = (d0, d1) => Math.Pow(d0, d1);
        private static Func<double, double, double> NumberFunc = (d0, d1) => d0;

        private static bool IsLeftFirst(
            this Func<double, double, double> f1,
            Func<double, double, double> f2)
        {
            if ((f1 == AddFunc || f1 == SubtractFunc) &&
                (f2 != AddFunc && f2 != SubtractFunc) ||
                (f1 == MultiplyFunc || f1 == DivideFunc) && f2 == PowerFunc)
            {
                return false;
            }

            return true;
        }

        private struct Operation
        {
            private readonly double leftValue;
            public readonly Func<double, double, double> Function;
            public readonly int Parentheses;

            public double GetResult(double rightValue)
            {
                return Function(leftValue, rightValue);
            }

            public Operation(double leftValue, Func<double, double, double> function, int parentheses)
            {
                this.leftValue = leftValue;
                this.Function = function;
                this.Parentheses = parentheses;
            }
        }

        /// <summary>
        /// Parses an arithmetic problem
        /// </summary>
        /// <remarks>
        /// Supported symbols are +-*/%^().
        /// No error checking currently other than what double.Parse() used internally does.
        /// </remarks>
        /// <param name="expression">The string that specifies the expression.</param>
        /// <returns>Result of the calculation.</returns>
        public static double Calculate(string expression)
        {
            expression = expression.Replace(" ", "");
            var stack = new Stack<Operation>();

            int start = 0;
            int length = expression.Length;
            int parentheses = 0;
            bool valueStarted = false;

            // only support single character currency symbols, separators etc. for now
            char currencySymbol = CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol.Length == 1 ? CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol[0] : '$';
            char decimalSeparator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator.Length == 1 ? CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator[0] : '.';
            char groupSeparator = CultureInfo.CurrentUICulture.NumberFormat.NumberGroupSeparator.Length == 1 ? CultureInfo.CurrentUICulture.NumberFormat.NumberGroupSeparator[0] : '\'';

            for (int i = 0; i < length; i++)
            {
                var c = expression[i];

                if (c >= '0' && c <= '9' || // digit
                    c == decimalSeparator ||
                    c == groupSeparator ||
                    c == currencySymbol ||
                    c == '-' && i == start && (i + 1 < length && expression[i + 1] >= '0' && expression[i + 1] <= '9') // leading negation
                    )
                {
                    if (c != '-')
                    {
                        valueStarted = true;
                    }

                    if (i + 1 < length) // not end of string
                    {
                        // continue reading value
                        continue;
                    }
                }

                // value complete
                if (i + 1 == length)
                {
                    var value = valueStarted ? double.Parse(expression.Substring(start)) : 0;

                    while (stack.Count > 0)
                    {
                        value = stack.Pop().GetResult(value);
                    }

                    return value;
                }
                else
                {
                    var value = valueStarted ? double.Parse(expression.Substring(start, i - start)) : 0;
                    start = i + 1;
                    Func<double, double, double> func = null;

                    switch (c)
                    {
                        case '+':
                            func = AddFunc;
                            break;
                        case '-':
                            func = SubtractFunc;
                            break;
                        case '*':
                            func = MultiplyFunc;
                            break;
                        case '/':
                            func = DivideFunc;
                            break;
                        case '%':
                            func = ModuloFunc;
                            break;
                        case '^':
                            func = PowerFunc;
                            break;
                        case '(':
                            parentheses++;
                            break;
                        case ')':
                            while (stack.Peek().Parentheses == parentheses)
                            {
                                value = stack.Pop().GetResult(value);
                            }

                            parentheses--;

                            if (parentheses < 0)
                            {
                                throw new FormatException(string.Format("Closing unopened parenthesis at position {0} in {1}.", i, expression));
                            }

                            stack.Push(new Operation(value, NumberFunc, parentheses));
                            break;
                        default:
                            throw new FormatException(string.Format("{0} at position {1} is not an expected character in {2}", c, i, expression));
                    }

                    if (func != null)
                    {
                        if (i == length - 1 ||
                            expression[i+1] == '+' ||
                            expression[i+1] == '-' ||
                            expression[i+1] == '*' ||
                            expression[i+1] == '/' ||
                            expression[i+1] == '%' ||
                            expression[i+1] == '^' ||
                            !valueStarted && stack.Count == 0)
                        {
                            throw new FormatException(string.Format("{0} at position {1} is not an expected character in {2}", expression[i+1], i+1, expression));
                        }

                        while (
                            stack.Count > 0 &&
                            stack.Peek().Parentheses == parentheses &&
                            stack.Peek().Function.IsLeftFirst(func))
                        {
                            value = stack.Pop().GetResult(value);
                        }

                        stack.Push(new Operation(value, func, parentheses));
                    }

                    valueStarted = false;
                }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Parses an arithmetic problem
        /// </summary>
        /// <remarks>
        /// Supported symbols are +-*/%^().
        /// No error checking currently other than what double.TryParse() used internally does.
        /// </remarks>
        /// <param name="expression">The string that specifies the expression.</param>
        /// <param name="result">The result.</param>
        /// <returns>True if calculation/parsing succeeded.</returns>
        public static bool TryCalculate(string expression, out double result)
        {
            expression = expression.Replace(" ", "");
            var stack = new Stack<Operation>();

            int start = 0;
            int length = expression.Length;
            int parentheses = 0;
            bool valueStarted = false;

            // only support single character currency symbols, separators etc. for now
            char currencySymbol = CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol.Length == 1 ? CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol[0] : '$';
            char decimalSeparator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator.Length == 1 ? CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator[0] : '.';
            char groupSeparator = CultureInfo.CurrentUICulture.NumberFormat.NumberGroupSeparator.Length == 1 ? CultureInfo.CurrentUICulture.NumberFormat.NumberGroupSeparator[0] : '\'';

            for (int i = 0; i < length; i++)
            {
                var c = expression[i];

                if (c >= '0' && c <= '9' || // digit
                    c == decimalSeparator ||
                    c == groupSeparator ||
                    c == currencySymbol ||
                    c == '-' && i == start && (i + 1 < length && expression[i + 1] >= '0' && expression[i + 1] <= '9') // leading negation
                    )
                {
                    if (c != '-')
                    {
                        valueStarted = true;
                    }

                    if (i + 1 < length) // not end of string
                    {
                        // continue reading value
                        continue;
                    }
                }

                // value complete
                if (i + 1 == length)
                {
                    double value;

                    if (valueStarted)
                    {
                        if (!double.TryParse(expression.Substring(start), out value))
                        {
                            result = double.NaN;
                            return false;
                        }
                    }
                    else
                    {
                        value = 0;
                    }

                    while (stack.Count > 0)
                    {
                        value = stack.Pop().GetResult(value);
                    }

                    result = value;
                    return true;
                }
                else
                {
                    double value;

                    if (valueStarted)
                    {
                        if (!double.TryParse(expression.Substring(start, i - start), out value))
                        {
                            result = double.NaN;
                            return false;
                        }
                    }
                    else
                    {
                        value = 0;
                    }

                    start = i + 1;
                    Func<double, double, double> func = null;

                    switch (c)
                    {
                        case '+':
                            func = AddFunc;
                            break;
                        case '-':
                            func = SubtractFunc;
                            break;
                        case '*':
                            func = MultiplyFunc;
                            break;
                        case '/':
                            func = DivideFunc;
                            break;
                        case '%':
                            func = ModuloFunc;
                            break;
                        case '^':
                            func = PowerFunc;
                            break;
                        case '(':
                            parentheses++;
                            break;
                        case ')':
                            while (stack.Peek().Parentheses == parentheses)
                            {
                                value = stack.Pop().GetResult(value);
                            }

                            parentheses--;

                            if (parentheses < 0)
                            {
                                result = double.NaN;
                                return false;
                            }

                            stack.Push(new Operation(value, NumberFunc, parentheses));
                            break;
                        default:
                            result = double.NaN;
                            return false;
                    }

                    if (func != null)
                    {
                        if (i == length - 1 ||
                            expression[i+1] == '+' ||
                            expression[i+1] == '-' ||
                            expression[i+1] == '*' ||
                            expression[i+1] == '/' ||
                            expression[i+1] == '%' ||
                            expression[i+1] == '^' ||
                            !valueStarted && stack.Count == 0)
                        {
                            result = double.NaN;
                            return false;
                        }

                        while (
                            stack.Count > 0 &&
                            stack.Peek().Parentheses == parentheses &&
                            stack.Peek().Function.IsLeftFirst(func))
                        {
                            value = stack.Pop().GetResult(value);
                        }

                        stack.Push(new Operation(value, func, parentheses));
                    }

                    valueStarted = false;
                }
            }

            result = double.NaN;
            return false;
        }
    }
}
