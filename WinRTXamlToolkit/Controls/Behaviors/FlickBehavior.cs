using System;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Interactivity;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls.Behaviors
{
    /// <summary>
    /// When applied to an element placed in a Canvas - allows the element to be dragged or flicked.
    /// </summary>
    /// <remarks>
    /// Works by changing the Canvas.Top/Left properties.
    /// Changes Manipulation mode of the associated element to translate with inertia.
    /// </remarks>
    public class FlickBehavior : Behavior<FrameworkElement>
    {
        private Canvas _canvas;
        private Point _startPosition;

        /// <summary>
        /// Called after the AssociatedObject is loaded (added to visual tree).
        /// </summary>
        /// <exception cref="System.InvalidOperationException">FlickBehavior can only be used on elements hosted inside of a Canvas.</exception>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnLoaded()
        {
            _canvas = this.AssociatedObject.GetFirstAncestorOfType<Canvas>();

            if (_canvas == null)
            {
                throw new InvalidOperationException("FlickBehavior can only be used on elements hosted inside of a Canvas.");
            }

            this.AssociatedObject.ManipulationMode =
                ManipulationModes.TranslateX |
                ManipulationModes.TranslateY |
                ManipulationModes.TranslateInertia;
            this.AssociatedObject.ManipulationStarting += OnAssociatedObjectManipulationStarting;
            this.AssociatedObject.ManipulationDelta += OnAssociatedObjectManipulationDelta;
        }

        /// <summary>
        /// Called after the AssociatedObject is unloaded (removed from visual tree).
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnUnloaded()
        {
            this.AssociatedObject.ManipulationStarting -= OnAssociatedObjectManipulationStarting;
            this.AssociatedObject.ManipulationDelta -= OnAssociatedObjectManipulationDelta;
            _canvas = null;
        }

        private void OnAssociatedObjectManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            _startPosition = new Point(
                Canvas.GetLeft(this.AssociatedObject),
                Canvas.GetTop(this.AssociatedObject));
        }

        private void OnAssociatedObjectManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs manipulationDeltaRoutedEventArgs)
        {
            var dx = manipulationDeltaRoutedEventArgs.Cumulative.Translation.X;
            var dy = manipulationDeltaRoutedEventArgs.Cumulative.Translation.Y;

            var x = _startPosition.X + dx;
            var y = _startPosition.Y + dy;

            if (manipulationDeltaRoutedEventArgs.IsInertial)
            {
                while (x < 0 ||
                       x > _canvas.ActualWidth - this.AssociatedObject.ActualWidth)
                {
                    if (x < 0)
                        x = -x;
                    if (x > _canvas.ActualWidth - this.AssociatedObject.ActualWidth)
                        x = 2 *
                            (_canvas.ActualWidth - this.AssociatedObject.ActualWidth) -
                            x;
                }

                while (y < 0 ||
                       y > _canvas.ActualHeight - this.AssociatedObject.ActualHeight)
                {
                    if (y < 0)
                        y = -y;
                    if (y > _canvas.ActualHeight - this.AssociatedObject.ActualHeight)
                        y = 2 * (_canvas.ActualHeight - this.AssociatedObject.ActualHeight) -
                            y;
                }
            }
            else
            {
                if (x < 0)
                    x = 0;
                if (x > _canvas.ActualWidth - this.AssociatedObject.ActualWidth)
                    x = _canvas.ActualWidth - this.AssociatedObject.ActualWidth;
                if (y < 0)
                    y = 0;
                if (y > _canvas.ActualHeight - this.AssociatedObject.ActualHeight)
                    y = _canvas.ActualHeight - this.AssociatedObject.ActualHeight;
            }

            Canvas.SetLeft(this.AssociatedObject, x);
            Canvas.SetTop(this.AssociatedObject, y);
        }
    }
}
