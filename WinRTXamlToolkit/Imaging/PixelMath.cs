namespace WinRTXamlToolkit.Imaging
{
    /// <summary>
    /// Contains basic pixel processing helper methods for double- and byte-type pixel color components.
    /// </summary>
    public static class PixelMath
    {
        #region Min(), Max() - 3-4 parameters
        public static double Max(double v1, double v2, double v3)
        {
            return v1 > v2 ? (v1 > v3 ? v1 : v3) : (v2 > v3 ? v2 : v3);
        }

        public static double Max(double v1, double v2, double v3, double v4)
        {
            return v1 > v2 ? (v1 > v3 ? (v1 > v4 ? v1 : v4) : (v3 > v4 ? v3 : v4)) : (v2 > v3 ? (v2 > v4 ? v2 : v4) : (v3 > v4 ? v3 : v4));
        }

        public static double Min(double v1, double v2, double v3)
        {
            return v1 < v2 ? (v1 < v3 ? v1 : v3) : (v2 < v3 ? v2 : v3);
        }

        public static double Min(double v1, double v2, double v3, double v4)
        {
            return v1 < v2 ? (v1 < v3 ? (v1 < v4 ? v1 : v4) : (v3 < v4 ? v3 : v4)) : (v2 < v3 ? (v2 < v4 ? v2 : v4) : (v3 < v4 ? v3 : v4));
        }

        public static byte Max(byte v1, byte v2, byte v3)
        {
            return v1 > v2 ? (v1 > v3 ? v1 : v3) : (v2 > v3 ? v2 : v3);
        }

        public static byte Max(byte v1, byte v2, byte v3, byte v4)
        {
            return v1 > v2 ? (v1 > v3 ? (v1 > v4 ? v1 : v4) : (v3 > v4 ? v3 : v4)) : (v2 > v3 ? (v2 > v4 ? v2 : v4) : (v3 > v4 ? v3 : v4));
        }

        public static byte Min(byte v1, byte v2, byte v3)
        {
            return v1 < v2 ? (v1 < v3 ? v1 : v3) : (v2 < v3 ? v2 : v3);
        }

        public static byte Min(byte v1, byte v2, byte v3, byte v4)
        {
            return v1 < v2 ? (v1 < v3 ? (v1 < v4 ? v1 : v4) : (v3 < v4 ? v3 : v4)) : (v2 < v3 ? (v2 < v4 ? v2 : v4) : (v3 < v4 ? v3 : v4));
        } 
        #endregion

        #region Clamp()
        public static double Clamp(this double value, double min, double max)
        {
            return value < min ? min : (value > max ? max : value);
        }

        public static byte Clamp(this byte value, byte min, byte max)
        {
            return value < min ? min : (value > max ? max : value);
        } 
        #endregion
    }
}
