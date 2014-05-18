using System;

namespace WinRTXamlToolkit.Tools
{
    /// <summary>
    /// Extension methods that help simplify some math operations.
    /// </summary>
    public static class MathEx
    {
        /// <summary>
        /// Returns the minimum of this and right value.
        /// </summary>
        /// <param name="lv">The left value.</param>
        /// <param name="rv">The right value.</param>
        /// <returns>The minimum of left and right value.</returns>
        public static double Min(this double lv, double rv)
        {
            return (lv < rv) ? lv : rv;
        }

        /// <summary>
        /// Returns the maximum of this and right value.
        /// </summary>
        /// <param name="lv">The left value.</param>
        /// <param name="rv">The right value.</param>
        /// <returns>The maximum of left and right value.</returns>
        public static double Max(this double lv, double rv)
        {
            return (lv > rv) ? lv : rv;
        }

        /// <summary>
        /// Returns the distance between this and right value.
        /// </summary>
        /// <param name="lv">The left value.</param>
        /// <param name="rv">The right value.</param>
        /// <returns>The distance between the left and right value.</returns>
        public static double Distance(this double lv, double rv)
        {
            return Math.Abs(lv - rv);
        }

        /// <summary>
        /// Returns linear interpolation between start and end values at position specified as 0..1 range value.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="end">The end value.</param>
        /// <param name="progress">The progress between start and end values
        /// where for progress value of 0 - the start value is returned and for 1 - the right value is returned.</param>
        /// <returns>Linear interpolation between start and end.</returns>
        public static double Lerp(double start, double end, double progress)
        {
            return start * (1 - progress) + end * progress;
        }

        /// <summary>
        /// Returns the value limited by the range of min..max.
        /// </summary>
        /// <remarks>
        /// If either min or max are double.NaN - they are not limiting the range.
        /// </remarks>
        /// <param name="value">The starting value.</param>
        /// <param name="min">The range minimum.</param>
        /// <param name="max">The range maximum.</param>
        /// <returns>The value limited by the range of min..max.</returns>
        public static double Clamp(this double @value, double min, double max)
        {
            if (!double.IsNaN(min) && @value < min)
                @value = min;
            else if (!double.IsNaN(max) && @value > max)
                @value = max;

            return @value;
        }
    }
}
