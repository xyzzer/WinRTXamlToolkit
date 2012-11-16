using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WinRTXamlToolkit.AwaitableUI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Extension methods for the ScrollViewer class.
    /// </summary>
    public static class ScrollViewerExtensions
    {
        private static readonly TimeSpan DefaultAnimatedScrollDuration = TimeSpan.FromSeconds(1.5);
        private static readonly EasingFunctionBase DefaultEasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };

        #region AnimatedScrollHandler
        /// <summary>
        /// AnimatedScrollHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty AnimatedScrollHandlerProperty =
            DependencyProperty.RegisterAttached(
                "AnimatedScrollHandler",
                typeof(ScrollViewerAnimatedScrollHandler),
                typeof(ScrollViewerExtensions),
                new PropertyMetadata(null, OnAnimatedScrollHandlerChanged));

        /// <summary>
        /// Gets the AnimatedScrollHandler property. This dependency property 
        /// indicates the handler object that handles animated scrolling of the ScrollViewer.
        /// </summary>
        public static ScrollViewerAnimatedScrollHandler GetAnimatedScrollHandler(DependencyObject d)
        {
            return (ScrollViewerAnimatedScrollHandler)d.GetValue(AnimatedScrollHandlerProperty);
        }

        /// <summary>
        /// Sets the AnimatedScrollHandler property. This dependency property 
        /// indicates the handler object that handles animated scrolling of the ScrollViewer.
        /// </summary>
        public static void SetAnimatedScrollHandler(DependencyObject d, ScrollViewerAnimatedScrollHandler value)
        {
            d.SetValue(AnimatedScrollHandlerProperty, value);
        }

        /// <summary>
        /// Handles changes to the AnimatedScrollHandler property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnAnimatedScrollHandlerChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScrollViewerAnimatedScrollHandler oldAnimatedScrollHandler = (ScrollViewerAnimatedScrollHandler)e.OldValue;
            ScrollViewerAnimatedScrollHandler newAnimatedScrollHandler = (ScrollViewerAnimatedScrollHandler)d.GetValue(AnimatedScrollHandlerProperty);

            var scrollViewer = d as ScrollViewer;

            Debug.Assert(scrollViewer != null);

            if (oldAnimatedScrollHandler != null)
                oldAnimatedScrollHandler.Detach();

            if (newAnimatedScrollHandler != null)
                newAnimatedScrollHandler.Attach(scrollViewer);
        }
        #endregion

        #region ScrollToHorizontalOffsetWithAnimation()
        /// <summary>
        /// Scrolls to the specified offset using an animation instead of
        /// immediately jumping to that offset as with ScrollToHorizontalOffset().
        /// </summary>
        /// <remarks>
        /// Note that calling ScrollToHorizontalOffset() does not update HorizontalOffset immediately,
        /// so it is important to wait for it to change before calling this method.
        /// </remarks>
        /// <param name="scrollViewer"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static async Task ScrollToHorizontalOffsetWithAnimation(
            this ScrollViewer scrollViewer,
            double offset)
        {
            await scrollViewer.ScrollToHorizontalOffsetWithAnimation(offset, DefaultAnimatedScrollDuration);
        }

        /// <summary>
        /// Scrolls to the specified offset using an animation instead of
        /// immediately jumping to that offset as with ScrollToHorizontalOffset().
        /// </summary>
        /// <remarks>
        /// Note that calling ScrollToHorizontalOffset() does not update HorizontalOffset immediately,
        /// so it is important to wait for it to change before calling this method.
        /// </remarks>
        /// <param name="scrollViewer"></param>
        /// <param name="offset"></param>
        /// <param name="durationInSeconds"></param>
        /// <returns></returns>
        public static async Task ScrollToHorizontalOffsetWithAnimation(
            this ScrollViewer scrollViewer,
            double offset,
            double durationInSeconds)
        {
            await scrollViewer.ScrollToHorizontalOffsetWithAnimation(
                offset,
                TimeSpan.FromSeconds(durationInSeconds),
                DefaultEasingFunction);
        }

        /// <summary>
        /// Scrolls to the specified offset using an animation instead of
        /// immediately jumping to that offset as with ScrollToHorizontalOffset().
        /// </summary>
        /// <remarks>
        /// Note that calling ScrollToHorizontalOffset() does not update HorizontalOffset immediately,
        /// so it is important to wait for it to change before calling this method.
        /// </remarks>
        /// <param name="scrollViewer"></param>
        /// <param name="offset"></param>
        /// <param name="durationInSeconds"></param>
        /// <param name="easingFunction"></param>
        /// <returns></returns>
        public static async Task ScrollToHorizontalOffsetWithAnimation(
            this ScrollViewer scrollViewer,
            double offset,
            double durationInSeconds,
            EasingFunctionBase easingFunction)
        {
            await scrollViewer.ScrollToHorizontalOffsetWithAnimation(
                offset,
                TimeSpan.FromSeconds(durationInSeconds),
                easingFunction);
        }

        /// <summary>
        /// Scrolls to the specified offset using an animation instead of
        /// immediately jumping to that offset as with ScrollToHorizontalOffset().
        /// </summary>
        /// <remarks>
        /// Note that calling ScrollToHorizontalOffset() does not update HorizontalOffset immediately,
        /// so it is important to wait for it to change before calling this method.
        /// </remarks>
        /// <param name="scrollViewer"></param>
        /// <param name="offset"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static async Task ScrollToHorizontalOffsetWithAnimation(
            this ScrollViewer scrollViewer,
            double offset,
            TimeSpan duration)
        {
            await scrollViewer.ScrollToHorizontalOffsetWithAnimation(
                offset,
                duration,
                DefaultEasingFunction);
        }

        /// <summary>
        /// Scrolls to the specified offset using an animation instead of
        /// immediately jumping to that offset as with ScrollToHorizontalOffset().
        /// </summary>
        /// <remarks>
        /// Note that calling ScrollToHorizontalOffset() does not update HorizontalOffset immediately,
        /// so it is important to wait for it to change before calling this method.
        /// </remarks>
        /// <param name="scrollViewer"></param>
        /// <param name="offset"></param>
        /// <param name="duration"></param>
        /// <param name="easingFunction"></param>
        /// <returns></returns>
        public static async Task ScrollToHorizontalOffsetWithAnimation(
            this ScrollViewer scrollViewer,
            double offset,
            TimeSpan duration,
            EasingFunctionBase easingFunction)
        {
            var handler = GetAnimatedScrollHandler(scrollViewer);

            if (handler == null)
            {
                handler = new ScrollViewerAnimatedScrollHandler();
                SetAnimatedScrollHandler(scrollViewer, handler);
            }

            await handler.ScrollToHorizontalOffsetWithAnimation(
                offset, duration, easingFunction);
        }
        #endregion

        #region ScrollToVerticalOffsetWithAnimation()
        /// <summary>
        /// Scrolls to the specified offset using an animation instead of
        /// immediately jumping to that offset as with ScrollToVerticalOffset().
        /// </summary>
        /// <remarks>
        /// Note that calling ScrollToVerticalOffset() does not update VerticalOffset immediately,
        /// so it is important to wait for it to change before calling this method.
        /// </remarks>
        /// <param name="scrollViewer"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static async Task ScrollToVerticalOffsetWithAnimation(
            this ScrollViewer scrollViewer,
            double offset)
        {
            await scrollViewer.ScrollToVerticalOffsetWithAnimation(offset, DefaultAnimatedScrollDuration);
        }

        /// <summary>
        /// Scrolls to the specified offset using an animation instead of
        /// immediately jumping to that offset as with ScrollToVerticalOffset().
        /// </summary>
        /// <remarks>
        /// Note that calling ScrollToVerticalOffset() does not update VerticalOffset immediately,
        /// so it is important to wait for it to change before calling this method.
        /// </remarks>
        /// <param name="scrollViewer"></param>
        /// <param name="offset"></param>
        /// <param name="durationInSeconds"></param>
        /// <returns></returns>
        public static async Task ScrollToVerticalOffsetWithAnimation(
            this ScrollViewer scrollViewer,
            double offset,
            double durationInSeconds)
        {
            await scrollViewer.ScrollToVerticalOffsetWithAnimation(
                offset,
                TimeSpan.FromSeconds(durationInSeconds),
                DefaultEasingFunction);
        }

        /// <summary>
        /// Scrolls to the specified offset using an animation instead of
        /// immediately jumping to that offset as with ScrollToVerticalOffset().
        /// </summary>
        /// <remarks>
        /// Note that calling ScrollToVerticalOffset() does not update VerticalOffset immediately,
        /// so it is important to wait for it to change before calling this method.
        /// </remarks>
        /// <param name="scrollViewer"></param>
        /// <param name="offset"></param>
        /// <param name="durationInSeconds"></param>
        /// <param name="easingFunction"></param>
        /// <returns></returns>
        public static async Task ScrollToVerticalOffsetWithAnimation(
            this ScrollViewer scrollViewer,
            double offset,
            double durationInSeconds,
            EasingFunctionBase easingFunction)
        {
            await scrollViewer.ScrollToVerticalOffsetWithAnimation(
                offset,
                TimeSpan.FromSeconds(durationInSeconds),
                easingFunction);
        }

        /// <summary>
        /// Scrolls to the specified offset using an animation instead of
        /// immediately jumping to that offset as with ScrollToVerticalOffset().
        /// </summary>
        /// <remarks>
        /// Note that calling ScrollToVerticalOffset() does not update VerticalOffset immediately,
        /// so it is important to wait for it to change before calling this method.
        /// </remarks>
        /// <param name="scrollViewer"></param>
        /// <param name="offset"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static async Task ScrollToVerticalOffsetWithAnimation(
            this ScrollViewer scrollViewer,
            double offset,
            TimeSpan duration)
        {
            await scrollViewer.ScrollToVerticalOffsetWithAnimation(
                offset,
                duration,
                DefaultEasingFunction);
        }

        /// <summary>
        /// Scrolls to the specified offset using an animation instead of
        /// immediately jumping to that offset as with ScrollToVerticalOffset().
        /// </summary>
        /// <remarks>
        /// Note that calling ScrollToVerticalOffset() does not update VerticalOffset immediately,
        /// so it is important to wait for it to change before calling this method.
        /// </remarks>
        /// <param name="scrollViewer"></param>
        /// <param name="offset"></param>
        /// <param name="duration"></param>
        /// <param name="easingFunction"></param>
        /// <returns></returns>
        public static async Task ScrollToVerticalOffsetWithAnimation(
            this ScrollViewer scrollViewer,
            double offset,
            TimeSpan duration,
            EasingFunctionBase easingFunction)
        {
            var handler = GetAnimatedScrollHandler(scrollViewer);

            if (handler == null)
            {
                handler = new ScrollViewerAnimatedScrollHandler();
                SetAnimatedScrollHandler(scrollViewer, handler);
            }

            await handler.ScrollToVerticalOffsetWithAnimation(
                offset, duration, easingFunction);
        }
        #endregion
    }

    public class ScrollViewerAnimatedScrollHandler : FrameworkElement
    {
        private ScrollViewer _scrollViewer;

        // Sliders are used as animation targets due to problems with custom property animation
        private Slider _sliderHorizontal;
        private Slider _sliderVertical;

        #region CTOR
        public ScrollViewerAnimatedScrollHandler()
        {
        }
        #endregion

        public void Attach(ScrollViewer scrollViewer)
        {
            _scrollViewer = scrollViewer;
            _sliderHorizontal = new Slider();
            _sliderHorizontal.SmallChange = 0.0000000001;
            _sliderHorizontal.Minimum = double.MinValue;
            _sliderHorizontal.Maximum = double.MaxValue;
            _sliderHorizontal.StepFrequency = 0.0000000001;
            _sliderHorizontal.ValueChanged += OnHorizontalOffsetChanged;
            _sliderVertical = new Slider();
            _sliderVertical.SmallChange = 0.0000000001;
            _sliderVertical.Minimum = double.MinValue;
            _sliderVertical.Maximum = double.MaxValue;
            _sliderVertical.StepFrequency = 0.0000000001;
            _sliderVertical.ValueChanged += OnVerticalOffsetChanged;
        }

        public void Detach()
        {
            _scrollViewer = null;

            if (_sliderHorizontal != null)
            {
                _sliderHorizontal.ValueChanged -= OnHorizontalOffsetChanged;
                _sliderHorizontal = null;
            }

            if (_sliderVertical != null)
            {
                _sliderVertical.ValueChanged -= OnHorizontalOffsetChanged;
                _sliderVertical = null;
            }
        }

        #region OnHorizontalOffsetChanged()
        private void OnHorizontalOffsetChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollToHorizontalOffset(e.NewValue);
            }
        }
        #endregion

        #region OnVerticalOffsetChanged()
        private void OnVerticalOffsetChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollToVerticalOffset(e.NewValue);
            }
        }
        #endregion

        #region ScrollToHorizontalOffsetWithAnimation()
        internal async Task ScrollToHorizontalOffsetWithAnimation(
            double offset,
            TimeSpan duration,
            EasingFunctionBase easingFunction)
        {
            var sb = new Storyboard();
            var da = new DoubleAnimation();
            da.EnableDependentAnimation = true;
            da.From = _scrollViewer.HorizontalOffset;
            da.To = offset;
            da.EasingFunction = easingFunction;
            da.Duration = duration;
            sb.Children.Add(da);
            Storyboard.SetTarget(sb, _sliderHorizontal);
            Storyboard.SetTargetProperty(da, "Value");
            await sb.BeginAsync();
        }
        #endregion

        #region ScrollToVerticalOffsetWithAnimation()
        internal async Task ScrollToVerticalOffsetWithAnimation(
            double offset,
            TimeSpan duration,
            EasingFunctionBase easingFunction)
        {
            var sb = new Storyboard();
            var da = new DoubleAnimation();
            da.EnableDependentAnimation = true;
            da.From = _scrollViewer.VerticalOffset;
            da.To = offset;
            da.EasingFunction = easingFunction;
            da.Duration = duration;
            sb.Children.Add(da);
            Storyboard.SetTarget(sb, _sliderVertical);
            Storyboard.SetTargetProperty(da, "Value");
            await sb.BeginAsync();
        }
        #endregion
    }
}
