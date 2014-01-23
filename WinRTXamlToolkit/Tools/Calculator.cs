using System;
using System.Collections.Generic;
using System.Globalization;

namespace WinRTXamlToolkit.Tools
{
    /// <summary>
    /// Implements Calculate and TryCalculate methods to evaluate simple arithmetic expressions.
    /// </summary>
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
        /// </remarks>
        /// <param name="expression">The string that specifies the expression.</param>
        /// <returns>Result of the calculation.</returns>
        public static double Calculate(string expression)
        {
            double result;

            TryCalculate(expression, out result, true);

            // Note - if TryCalculate failed - it would throw an exception, so we don't need to check the result.

            return result;
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
            return TryCalculate(expression, out result, false);
        }

        /// <summary>
        /// Parses an arithmetic problem
        /// </summary>
        /// <remarks>
        /// Supported symbols are +-*/%^().
        /// </remarks>
        /// <param name="expression">The string that specifies the expression.</param>
        /// <param name="result">The result.</param>
        /// <param name="throwOnError">If true and syntex error is found - throws exception. Otherwise - method returns false and result is NaN.</param>
        /// <returns>True if calculation/parsing succeeded.</returns>
        private static bool TryCalculate(string expression, out double result, bool throwOnError)
        {
            if (expression == null)
            {
                if (throwOnError)
                {
                    throw new ArgumentNullException(expression);
                }

                result = double.NaN;
                return false;
            }

            expression = expression.Replace(" ", "");
            var stack = new Stack<Operation>();

            int start = 0;
            int length = expression.Length;

            if (length == 0)
            {
                if (throwOnError)
                {
                    throw new ArgumentException("Empty expression can't be calculated.");
                }

                result = double.NaN;
                return false;
            }

            int parentheses = 0;
            bool valueStarted = false;

            // only support single character currency symbols, separators etc. for now
            char currencySymbol = CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol.Length == 1 ? CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol[0] : '$';
            char decimalSeparator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator.Length == 1 ? CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator[0] : '.';
            char groupSeparator = CultureInfo.CurrentUICulture.NumberFormat.NumberGroupSeparator.Length == 1 ? CultureInfo.CurrentUICulture.NumberFormat.NumberGroupSeparator[0] : '\'';

            for (int i = 0; i < length; i++)
            {
                var c = expression[i];

                // Number
                if (c >= '0' && c <= '9' || // digit
                    c == 'E' ||
                    c == 'e' ||
                    c == decimalSeparator ||
                    c == groupSeparator ||
                    c == currencySymbol ||
                    c == '-' && i == start && (i + 1 < length && expression[i + 1] >= '0' && expression[i + 1] <= '9') // leading negation
                    )
                {
                    if (c == 'E' || c == 'e')
                    {
                        valueStarted = true;

                        if (i + 1 == length)
                        {
                            if (throwOnError)
                            {
                                throw new FormatException(string.Format("{0} at position {1} is not an expected character at end of {2}", c, i, expression));
                            }

                            result = double.NaN;
                            return false;
                        }

                        //i++;

                        //c = expression[i];

                        //if (c >= '0' && c <= '9' ||
                        //    c == '-' && i + 1 < length)
                        //{
                        //    continue;
                        //}

                        //if (throwOnError)
                        //{
                        //    throw new FormatException(string.Format("{0} at position {1} is not an expected character in {2}", c, i, expression));
                        //}

                        //result = double.NaN;
                        //return false;

                        continue;
                    }

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
                else if (stack.Count == 0 && !valueStarted && c != '(')
                {
                    if (throwOnError)
                    {
                        throw new FormatException(string.Format("{0} at position {1} is not an expected character in {2}", c, i, expression));
                    }

                    result = double.NaN;
                    return false;
                }

                // value complete
                if (i + 1 == length)
                {
                    double value;

                    if (valueStarted)
                    {
                        var token =
                            c == ')' && parentheses == 1
                                ? expression.Substring(start, i - start)
                                : expression.Substring(start);

                        if (!double.TryParse(token, out value))
                        {
                            if (throwOnError)
                            {
                                // need to retry parsing to throw with meaningful syntax error
                                double.Parse(token);
                            }

                            result = double.NaN;
                            return false;
                        }
                    }
                    else
                    {
                        if (stack.Count == 0)
                        {
                            if (throwOnError)
                            {
                                // need to retry parsing to throw with meaningful syntax error
                                throw new FormatException(string.Format("{0} at position {1} is not an expected character in {2}", c, i, expression));
                            }

                            result = double.NaN;
                            return false;
                        }

                        value = 0;
                    }

                    while (stack.Count > 0)
                    {
                        value = stack.Pop().GetResult(value);
                    }

                    if (parentheses != 0 && !(c == ')' && parentheses == 1))
                    {
                        if (throwOnError)
                        {
                            // need to retry parsing to throw with meaningful syntax error
                            throw new FormatException(string.Format("Expression missing a closing parenthesis character - {0} ", expression));
                        }

                        result = double.NaN;
                        return false;
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
                            if (throwOnError)
                            {
                                // need to retry parsing to throw with meaningful syntax error
                                double.Parse(expression.Substring(start, i - start));
                            }

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
                            if (stack.Count > 0 && stack.Peek().Function == NumberFunc ||
                                valueStarted)
                            {
                                if (throwOnError)
                                {
                                    throw new FormatException(string.Format("{0} at position {1} is not an expected character in {2}", c, i, expression));
                                }

                                result = double.NaN;
                                return false;
                            }

                            parentheses++;
                            break;
                        case ')':
                            while (
                                stack.Count > 0 &&
                                stack.Peek().Parentheses == parentheses)
                            {
                                value = stack.Pop().GetResult(value);
                            }

                            parentheses--;

                            if (parentheses < 0)
                            {
                                if (throwOnError)
                                {            
                                    throw new FormatException(string.Format("Closing unopened parenthesis at position {0} in {1}.", i, expression));
                                }

                                result = double.NaN;
                                return false;
                            }

                            if (i + 1 == length)
                            {
                                result = value;
                                return true;
                            }

                            stack.Push(new Operation(value, NumberFunc, parentheses));
                            break;
                        default:
                            if (throwOnError)
                            {
                                throw new FormatException(string.Format("{0} at position {1} is not an expected character in {2}", c, i, expression));
                            }

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
                            if (throwOnError)
                            {
                                throw new FormatException(string.Format("{0} at position {1} is not an expected character in {2}", expression[i+1], i+1, expression));
                            }

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

            if (throwOnError)
            {
                throw new InvalidOperationException();
            }

            result = double.NaN;
            return false;
        }
    }
}
