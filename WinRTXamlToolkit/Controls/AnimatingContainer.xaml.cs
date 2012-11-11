using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// A UserControl that applies animation to its content.
    /// </summary>
    [ContentProperty(Name="RotatingContent")]
    public sealed partial class AnimatingContainer : UserControl
    {
        #region RotatingContent
        /// <summary>
        /// RotatingContent Dependency Property
        /// </summary>
        public static readonly DependencyProperty RotatingContentProperty =
            DependencyProperty.Register(
                "RotatingContent",
                typeof(object),
                typeof(AnimatingContainer),
                new PropertyMetadata(null, OnRotatingContentChanged));

        /// <summary>
        /// Gets or sets the RotatingContent property. This dependency property 
        /// indicates the RotatingContent of this control.
        /// </summary>
        public object RotatingContent
        {
            get { return (object)GetValue(RotatingContentProperty); }
            set { SetValue(RotatingContentProperty, value); }
        }

        /// <summary>
        /// Handles changes to the RotatingContent property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnRotatingContentChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (AnimatingContainer)d;
            object oldRotatingContent = (object)e.OldValue;
            object newRotatingContent = target.RotatingContent;
            target.OnRotatingContentChanged(oldRotatingContent, newRotatingContent);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the RotatingContent property.
        /// </summary>
        /// <param name="oldRotatingContent">The old RotatingContent value</param>
        /// <param name="newRotatingContent">The new RotatingContent value</param>
        private void OnRotatingContentChanged(
            object oldRotatingContent, object newRotatingContent)
        {
            ContentContainer.Content = newRotatingContent;
        }
        #endregion

        #region RadiusX
        /// <summary>
        /// RadiusX Dependency Property
        /// </summary>
        public static readonly DependencyProperty RadiusXProperty =
            DependencyProperty.Register(
                "RadiusX",
                typeof(double),
                typeof(AnimatingContainer),
                new PropertyMetadata(0d, OnRadiusXChanged));

        /// <summary>
        /// Gets or sets the RadiusX property. This dependency property 
        /// indicates the rotation RadiusX.
        /// </summary>
        public double RadiusX
        {
            get { return (double)GetValue(RadiusXProperty); }
            set { SetValue(RadiusXProperty, value); }
        }

        /// <summary>
        /// Handles changes to the RadiusX property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnRadiusXChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (AnimatingContainer)d;
            double oldRadiusX = (double)e.OldValue;
            double newRadiusX = target.RadiusX;
            target.OnRadiusXChanged(oldRadiusX, newRadiusX);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the RadiusX property.
        /// </summary>
        /// <param name="oldRadiusX">The old RadiusX value</param>
        /// <param name="newRadiusX">The new RadiusX value</param>
        private void OnRadiusXChanged(
            double oldRadiusX, double newRadiusX)
        {
            UpdateRadius();
        }
        #endregion

        #region RadiusY
        /// <summary>
        /// RadiusY Dependency Property
        /// </summary>
        public static readonly DependencyProperty RadiusYProperty =
            DependencyProperty.Register(
                "RadiusY",
                typeof(double),
                typeof(AnimatingContainer),
                new PropertyMetadata(0d, OnRadiusYChanged));

        /// <summary>
        /// Gets or sets the RadiusY property. This dependency property 
        /// indicates the rotation RadiusY.
        /// </summary>
        public double RadiusY
        {
            get { return (double)GetValue(RadiusYProperty); }
            set { SetValue(RadiusYProperty, value); }
        }

        /// <summary>
        /// Handles changes to the RadiusY property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnRadiusYChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (AnimatingContainer)d;
            double oldRadiusY = (double)e.OldValue;
            double newRadiusY = target.RadiusY;
            target.OnRadiusYChanged(oldRadiusY, newRadiusY);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the RadiusY property.
        /// </summary>
        /// <param name="oldRadiusY">The old RadiusY value</param>
        /// <param name="newRadiusY">The new RadiusY value</param>
        private void OnRadiusYChanged(
            double oldRadiusY, double newRadiusY)
        {
            this.UpdateRadius();
        }
        #endregion

        #region Duration
        /// <summary>
        /// Duration Dependency Property
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(
                "Duration",
                typeof(string),
                typeof(AnimatingContainer),
                new PropertyMetadata("0:0:4", OnDurationChanged));

        /// <summary>
        /// Gets or sets the Duration property. This dependency property 
        /// indicates the duration of the animation.
        /// </summary>
        public string Duration
        {
            get { return (string)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Duration property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnDurationChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (AnimatingContainer)d;
            string oldDuration = (string)e.OldValue;
            string newDuration = target.Duration;
            target.OnDurationChanged(oldDuration, newDuration);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Duration property.
        /// </summary>
        /// <param name="oldDuration">The old Duration value</param>
        /// <param name="newDuration">The new Duration value</param>
        private void OnDurationChanged(
            string oldDuration, string newDuration)
        {
            UpdateDuration();
        }
        #endregion

        #region PulseScale
        /// <summary>
        /// PulseScale Dependency Property
        /// </summary>
        public static readonly DependencyProperty PulseScaleProperty =
            DependencyProperty.Register(
                "PulseScale",
                typeof(double),
                typeof(AnimatingContainer),
                new PropertyMetadata(1d, OnPulseScaleChanged));

        /// <summary>
        /// Gets or sets the PulseScale property. This dependency property 
        /// indicates the rotation PulseScale.
        /// </summary>
        public double PulseScale
        {
            get { return (double)GetValue(PulseScaleProperty); }
            set { SetValue(PulseScaleProperty, value); }
        }

        /// <summary>
        /// Handles changes to the PulseScale property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnPulseScaleChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (AnimatingContainer)d;
            double oldPulseScale = (double)e.OldValue;
            double newPulseScale = target.PulseScale;
            target.OnPulseScaleChanged(oldPulseScale, newPulseScale);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the PulseScale property.
        /// </summary>
        /// <param name="oldPulseScale">The old PulseScale value</param>
        /// <param name="newPulseScale">The new PulseScale value</param>
        private void OnPulseScaleChanged(
            double oldPulseScale, double newPulseScale)
        {
            this.UpdateRadius();
        }
        #endregion

        #region AutoPlay
        /// <summary>
        /// AutoPlay Dependency Property
        /// </summary>
        public static readonly DependencyProperty AutoPlayProperty =
            DependencyProperty.Register(
                "AutoPlay",
                typeof(bool),
                typeof(AnimatingContainer),
                new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the AutoPlay property. This dependency property 
        /// indicates whether animation playback will begin automatically when the control loads.
        /// </summary>
        public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }
            set { SetValue(AutoPlayProperty, value); }
        }
        #endregion

        #region IsAnimating
        /// <summary>
        /// IsAnimating Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsAnimatingProperty =
            DependencyProperty.Register(
                "IsAnimating",
                typeof(bool),
                typeof(AnimatingContainer),
                new PropertyMetadata(false, OnIsAnimatingChanged));

        /// <summary>
        /// Gets or sets the IsAnimating property. This dependency property 
        /// indicates whether the animation is playing.
        /// </summary>
        public bool IsAnimating
        {
            get { return (bool)GetValue(IsAnimatingProperty); }
            set { SetValue(IsAnimatingProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsAnimating property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnIsAnimatingChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (AnimatingContainer)d;
            bool oldIsAnimating = (bool)e.OldValue;
            bool newIsAnimating = target.IsAnimating;
            target.OnIsAnimatingChanged(oldIsAnimating, newIsAnimating);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the IsAnimating property.
        /// </summary>
        /// <param name="oldIsAnimating">The old IsAnimating value</param>
        /// <param name="newIsAnimating">The new IsAnimating value</param>
        private void OnIsAnimatingChanged(
            bool oldIsAnimating, bool newIsAnimating)
        {
            if (newIsAnimating)
                BeginAnimations();
            else
                StopAnimations();
        }
        #endregion

        public AnimatingContainer()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        public void Animate()
        {
            IsAnimating = true;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (AutoPlay)
                BeginAnimations();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            StopAnimations();
        }

        private void BeginAnimations()
        {
            UpdateDuration();
            UpdateRadius();
            RotationStoryboard.Begin();
            PulsingStoryboard.Begin();
        }

        private void StopAnimations()
        {
            RotationStoryboard.Stop();
            PulsingStoryboard.Stop();
        }

        private void UpdateDuration()
        {
            TimeSpan duration = TimeSpan.Parse(Duration);
            this.KeyRightX.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration.TotalSeconds / 4));
            this.KeyRightY.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration.TotalSeconds / 4));
            this.KeyBottomX.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration.TotalSeconds / 2));
            this.KeyBottomY.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration.TotalSeconds / 2));
            this.KeyLeftX.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration.TotalSeconds * 3 / 4));
            this.KeyLeftY.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration.TotalSeconds * 3 / 4));
            this.KeyTopX.KeyTime = KeyTime.FromTimeSpan(duration);
            this.KeyTopY.KeyTime = KeyTime.FromTimeSpan(duration);
            this.PulseKeyX.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration.TotalSeconds / 2));
            this.PulseKeyY.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration.TotalSeconds / 2));
        }

        private void UpdateRadius()
        {
            this.KeyRightX.Value = RadiusX;
            this.KeyRightY.Value = RadiusY;
            this.KeyBottomY.Value = 2 * RadiusY;
            this.KeyLeftX.Value = -RadiusX;
            this.KeyLeftY.Value = RadiusY;
            this.PulseKeyX.Value = PulseScale;
            this.PulseKeyY.Value = PulseScale;
        }
    }
}