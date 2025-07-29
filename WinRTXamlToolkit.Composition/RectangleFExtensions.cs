using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace WinRTXamlToolkit.Composition
{
    public static class RectangleFExtensions
    {
        /// <summary>
        /// Returns a dilated version of the specified rectangle by expanding it by amount in each direction.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static RectangleF Dilated(this RectangleF rect, float amount)
        {
            rect.Left -= amount;
            rect.Top -= amount;
            rect.Right += amount;
            rect.Bottom += amount;

            return rect;
        }

        /// <summary>
        /// Returns an eroded version of the specified rectangle by shrinking it by amount from each direction.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static RectangleF Eroded(this RectangleF rect, float amount)
        {
            rect.Left += amount;
            rect.Top += amount;
            rect.Right -= amount;
            rect.Bottom -= amount;

            return rect;
        }
    }
}
