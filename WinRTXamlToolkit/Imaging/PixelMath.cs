namespace WinRTXamlToolkit.Imaging
{
    /// <summary>
    /// Contains basic pixel processing helper methods for double- and byte-type pixel color components.
    /// </summary>
    public static class PixelMath
    {
        #region Min(), Max() - 3-4 parameters
        /// <summary>
        /// Returns the maximum of 3 numbers.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <returns></returns>
        public static double Max(double v1, double v2, double v3)
        {
            return v1 > v2 ? (v1 > v3 ? v1 : v3) : (v2 > v3 ? v2 : v3);
        }

        /// <summary>
        /// Returns the maximum of 4 numbers.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <returns></returns>
        public static double Max(double v1, double v2, double v3, double v4)
        {
            return v1 > v2 ? (v1 > v3 ? (v1 > v4 ? v1 : v4) : (v3 > v4 ? v3 : v4)) : (v2 > v3 ? (v2 > v4 ? v2 : v4) : (v3 > v4 ? v3 : v4));
        }

        /// <summary>
        /// Returns the minimum of 3 numbers.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <returns></returns>
        public static double Min(double v1, double v2, double v3)
        {
            return v1 < v2 ? (v1 < v3 ? v1 : v3) : (v2 < v3 ? v2 : v3);
        }

        /// <summary>
        /// Returns the minimum of 4 numbers.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <returns></returns>
        public static double Min(double v1, double v2, double v3, double v4)
        {
            return v1 < v2 ? (v1 < v3 ? (v1 < v4 ? v1 : v4) : (v3 < v4 ? v3 : v4)) : (v2 < v3 ? (v2 < v4 ? v2 : v4) : (v3 < v4 ? v3 : v4));
        }

        /// <summary>
        /// Returns the maximum of 3 numbers.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <returns></returns>
        public static byte Max(byte v1, byte v2, byte v3)
        {
            return v1 > v2 ? (v1 > v3 ? v1 : v3) : (v2 > v3 ? v2 : v3);
        }

        /// <summary>
        /// Returns the maximum of 4 numbers.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <returns></returns>
        public static byte Max(byte v1, byte v2, byte v3, byte v4)
        {
            return v1 > v2 ? (v1 > v3 ? (v1 > v4 ? v1 : v4) : (v3 > v4 ? v3 : v4)) : (v2 > v3 ? (v2 > v4 ? v2 : v4) : (v3 > v4 ? v3 : v4));
        }

        /// <summary>
        /// Returns the minimum of 3 numbers.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <returns></returns>
        public static byte Min(byte v1, byte v2, byte v3)
        {
            return v1 < v2 ? (v1 < v3 ? v1 : v3) : (v2 < v3 ? v2 : v3);
        }

        /// <summary>
        /// Returns the minimum of 4 numbers.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <returns></returns>
        public static byte Min(byte v1, byte v2, byte v3, byte v4)
        {
            return v1 < v2 ? (v1 < v3 ? (v1 < v4 ? v1 : v4) : (v3 < v4 ? v3 : v4)) : (v2 < v3 ? (v2 < v4 ? v2 : v4) : (v3 < v4 ? v3 : v4));
        } 
        #endregion

        #region Clamp()
        /// <summary>
        /// Clamps the specified value to the given inclusive min..max range.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static double Clamp(this double value, double min, double max)
        {
            return value < min ? min : (value > max ? max : value);
        }

        /// <summary>
        /// Clamps the specified value to the given inclusive min..max range.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static byte Clamp(this byte value, byte min, byte max)
        {
            return value < min ? min : (value > max ? max : value);
        } 
        #endregion
    }
}
