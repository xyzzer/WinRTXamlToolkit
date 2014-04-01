using System;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Implements extensions of the <see cref="ManipulationInertiaStartingRoutedEventArgs"/> class that defines the argument of the <see cref="ManipulationInertiaStarting"/> event.
    /// </summary>
    public static class ManipulationInertiaStartingRoutedEventArgsExtensions
    {
        /// <summary>
        /// Defines the default deceleration of a flick determined by testing the framework.
        /// </summary>
        public const double DefaultDeceleration = 0.0036234234;

        public static void SetDesiredDisplacementX(this ManipulationInertiaStartingRoutedEventArgs e, double desiredDisplacementX)
        {
            var vx = e.Velocities.Linear.X;
            var vy = e.Velocities.Linear.Y;

            if (vx == 0)
            {
                e.TranslationBehavior.DesiredDisplacement = desiredDisplacementX;
                return;
                //throw new InvalidOperationException("Can't adjust inertia angle - desired displacement X is unsupported.");
            }

            var v = Math.Sqrt(vx * vx + vy * vy);

            e.TranslationBehavior.DesiredDisplacement = desiredDisplacementX * v / vx;
        }

        public static void SetDesiredDisplacementY(this ManipulationInertiaStartingRoutedEventArgs e, double desiredDisplacementY)
        {
            var vx = e.Velocities.Linear.X;
            var vy = e.Velocities.Linear.Y;

            if (vy == 0)
            {
                e.TranslationBehavior.DesiredDisplacement = desiredDisplacementY;
                return;
                throw new InvalidOperationException("Can't adjust inertia angle - desired displacement Y is unsupported.");
            }

            var v = Math.Sqrt(vx * vx + vy * vy);

            e.TranslationBehavior.DesiredDisplacement = desiredDisplacementY * v / vy;
        }

        /// <summary>
        /// Gets the expected duration of a flick as measured in seconds.
        /// A flick starts on <see cref="ManipulationInertiaStarting"/> event and ends on <see cref="ManipulationCompleted"/> event.
        /// </summary>
        /// <remarks>
        /// Note that the duration is estimated and may vary by a few percent based on testing.
        /// </remarks>
        /// <param name="e">The event argument from the <see cref="ManipulationInertiaStarting"/> event.</param>
        /// <returns>The expected duration of the flick as measured in seconds.</returns>
        public static double GetExpectedDisplacementDuration(this ManipulationInertiaStartingRoutedEventArgs e)
        {
            var d = e.TranslationBehavior.DesiredDisplacement;
            double a;

            var vx = e.Velocities.Linear.X;
            var vy = e.Velocities.Linear.Y;
            var v = Math.Sqrt(vx * vx + vy * vy);

            if (double.IsNaN(d))
            {
                a = e.TranslationBehavior.DesiredDeceleration;

                if (double.IsNaN(a))
                {
                    a = DefaultDeceleration;
                }
            }
            else
            {
                a = v * v / (2 * d);
            }

            return (v / a) * 0.001;
        }

        /// <summary>
        /// Gets the expected displacement at the end of the flick.
        /// A flick starts on <see cref="ManipulationInertiaStarting"/> event and ends on <see cref="ManipulationCompleted"/> event.
        /// The displacement is the length of the vector from e.Cumulative.Translation at the beginning of the flick
        /// to the e.Cumulative.Translation at the end of the flick.
        /// </summary>
        /// <param name="e">The event argument from the <see cref="ManipulationInertiaStarting"/> event.</param>
        /// <returns>The displacement of the flick.</returns>
        public static double GetExpectedDisplacement(this ManipulationInertiaStartingRoutedEventArgs e)
        {
            var d = e.TranslationBehavior.DesiredDisplacement;

            if (double.IsNaN(d))
            {
                var a = e.TranslationBehavior.DesiredDeceleration;

                if (double.IsNaN(a))
                {
                    a = DefaultDeceleration;
                }

                var vx = e.Velocities.Linear.X;
                var vy = e.Velocities.Linear.Y;
                var v = Math.Sqrt(vx * vx + vy * vy);

                d = v * v / (2 * a);
            }

            return d;
        }

        /// <summary>
        /// Gets the X component of the expected displacement vector at the end of the flick.
        /// A flick starts on <see cref="ManipulationInertiaStarting"/> event and ends on <see cref="ManipulationCompleted"/> event.
        /// The displacement is the vector from e.Cumulative.Translation at the beginning of the flick
        /// to the e.Cumulative.Translation at the end of the flick.
        /// </summary>
        /// <param name="e">The event argument from the <see cref="ManipulationInertiaStarting"/> event.</param>
        /// <returns>The X component of the displacement of the flick.</returns>
        public static double GetExpectedDisplacementX(this ManipulationInertiaStartingRoutedEventArgs e)
        {
            var vx = e.Velocities.Linear.X;

            if (vx == 0)
            {
                return 0;
            }

            var vy = e.Velocities.Linear.Y;
            var v = Math.Sqrt(vx * vx + vy * vy);
            var d = e.TranslationBehavior.DesiredDisplacement;

            if (double.IsNaN(d))
            {
                var a = e.TranslationBehavior.DesiredDeceleration;

                if (double.IsNaN(a))
                {
                    a = DefaultDeceleration;
                }

                d = v * v / (2 * a);
            }

            var dx = d * vx / v;

            return dx;
        }

        /// <summary>
        /// Gets the Y component of the expected displacement vector at the end of the flick.
        /// A flick starts on <see cref="ManipulationInertiaStarting"/> event and ends on <see cref="ManipulationCompleted"/> event.
        /// The displacement is the vector from e.Cumulative.Translation at the beginning of the flick
        /// to the e.Cumulative.Translation at the end of the flick.
        /// </summary>
        /// <param name="e">The event argument from the <see cref="ManipulationInertiaStarting"/> event.</param>
        /// <returns>The Y component of the displacement of the flick.</returns>
        public static double GetExpectedDisplacementY(this ManipulationInertiaStartingRoutedEventArgs e)
        {
            var vy = e.Velocities.Linear.Y;

            if (vy == 0)
            {
                return 0;
            }

            var vx = e.Velocities.Linear.X;
            var v = Math.Sqrt(vx * vx + vy * vy);
            var d = e.TranslationBehavior.DesiredDisplacement;

            if (double.IsNaN(d))
            {
                var a = e.TranslationBehavior.DesiredDeceleration;

                if (double.IsNaN(a))
                {
                    a = DefaultDeceleration;
                }

                d = v * v / (2 * a);
            }

            var dy = d * vy / v;

            return dy;
        }
    }
}
