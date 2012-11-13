using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Animation used in WipeTransition that reveals the page using a clip rectangle animation.
    /// </summary>
    public class WipeAnimation : PageTransitionAnimation
    {
        #region Direction
        /// <summary>
        /// Direction Dependency Property
        /// </summary>
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(
                "Direction",
                typeof(DirectionOfMotion),
                typeof(WipeAnimation),
                new PropertyMetadata(DirectionOfMotion.RightToLeft));

        /// <summary>
        /// Gets or sets the Direction property. This dependency property 
        /// indicates the slide direction.
        /// </summary>
        public DirectionOfMotion Direction
        {
            get { return (DirectionOfMotion)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }
        #endregion

        /// <summary>
        /// Gets the generated animation.
        /// </summary>
        /// <value>
        /// The animation.
        /// </value>
        protected override Storyboard Animation
        {
            get
            {
                // NOTE: There seem to be problems with WinRT when reusing same Storyboard for multiple elements, so we need to always get a new storyboard.
                var sb = new Storyboard();
                var da = new DoubleAnimation();
                da.EasingFunction = this.EasingFunction;
                da.Duration = this.Duration;
                sb.Children.Add(da);

                return sb;
            }
        }

        private Slider _slider;
        private FrameworkElement _fe;

        protected override void ApplyTargetProperties(DependencyObject target, Storyboard animation)
        {
            _fe = (FrameworkElement)target;

            if (_fe.Clip == null)
            {
                _fe.Clip = new RectangleGeometry();
            }

            if (this.Mode == AnimationMode.Out)
            {
                _fe.Clip.Rect = new Rect(0, 0, _fe.ActualWidth, _fe.ActualHeight);
            }
            else
            {
                switch (this.Direction)
                {
                    case DirectionOfMotion.TopToBottom:
                        _fe.Clip.Rect = new Rect(0, 0, _fe.ActualWidth, 0);
                        break;
                    case DirectionOfMotion.BottomToTop:
                        _fe.Clip.Rect = new Rect(0, _fe.ActualHeight, _fe.ActualWidth, 0);
                        break;
                    case DirectionOfMotion.LeftToRight:
                        _fe.Clip.Rect = new Rect(0, 0, 0, _fe.ActualHeight);
                        break;
                    case DirectionOfMotion.RightToLeft:
                        _fe.Clip.Rect = new Rect(_fe.ActualHeight, 0, 0, _fe.ActualHeight);
                        break;
                }
            }

            var da = (DoubleAnimation)animation.Children[0];
            da.EnableDependentAnimation = true;

            // Slider is used as animation targets due to problems with custom property animation
            if (_slider == null)
            {
                _slider = new Slider();
                _slider.SmallChange = 0.0000000001;
                _slider.Minimum = double.MinValue;
                _slider.Maximum = double.MaxValue;
                _slider.StepFrequency = 0.0000000001;
                _slider.ValueChanged += OnSliderValueChanged;
            }

            Storyboard.SetTarget(animation, _slider);
            Storyboard.SetTargetProperty(da, "Value");

            da.From = 0;
            da.To = 1.0;
        }

        private void OnSliderValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var amount = _slider.Value;

            if (this.Mode == AnimationMode.Out)
            {
                switch (this.Direction)
                {
                    case DirectionOfMotion.TopToBottom:
                        _fe.Clip.Rect = new Rect(0, amount * _fe.ActualHeight, _fe.ActualWidth, (1 - amount) * _fe.ActualHeight);
                        break;
                    case DirectionOfMotion.BottomToTop:
                        _fe.Clip.Rect = new Rect(0, 0, _fe.ActualWidth, amount * _fe.ActualHeight);
                        break;
                    case DirectionOfMotion.LeftToRight:
                        _fe.Clip.Rect = new Rect(amount * _fe.ActualWidth, 0, (1 - amount) * _fe.ActualWidth, _fe.ActualHeight);
                        break;
                    case DirectionOfMotion.RightToLeft:
                        _fe.Clip.Rect = new Rect(0, 0, amount * _fe.ActualWidth, _fe.ActualHeight);
                        break;
                }
            }
            else
            {
                switch (this.Direction)
                {
                    case DirectionOfMotion.TopToBottom:
                        _fe.Clip.Rect = new Rect(0, 0, _fe.ActualWidth, amount * _fe.ActualHeight);
                        break;
                    case DirectionOfMotion.BottomToTop:
                        _fe.Clip.Rect = new Rect(0, (1 - amount) * _fe.ActualHeight, _fe.ActualWidth, amount * _fe.ActualHeight);
                        break;
                    case DirectionOfMotion.LeftToRight:
                        _fe.Clip.Rect = new Rect(0, 0, amount * _fe.ActualWidth, _fe.ActualHeight);
                        break;
                    case DirectionOfMotion.RightToLeft:
                        _fe.Clip.Rect = new Rect((1 - amount) * _fe.ActualWidth, 0, amount * _fe.ActualWidth, _fe.ActualHeight);
                        break;
                }
            }
        }

        internal override void CleanupAnimation(DependencyObject target, Storyboard animation)
        {
            base.CleanupAnimation(target, animation);
            _slider.ValueChanged -= OnSliderValueChanged;
            _slider = null;
            _fe = null;
        }
    }
}
