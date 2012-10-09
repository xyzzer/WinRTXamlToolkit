using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace WinRTXamlToolkit.Controls
{
    [ContentProperty(Name="RotatingContent")]
    public sealed partial class RotatingContainer : UserControl
    {
        #region RotatingContent
        /// <summary>
        /// RotatingContent Dependency Property
        /// </summary>
        public static readonly DependencyProperty RotatingContentProperty =
            DependencyProperty.Register(
                "RotatingContent",
                typeof(object),
                typeof(RotatingContainer),
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
            var target = (RotatingContainer)d;
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
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
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
                typeof(RotatingContainer),
                new PropertyMetadata(1d, OnRadiusXChanged));

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
            var target = (RotatingContainer)d;
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
                typeof(RotatingContainer),
                new PropertyMetadata(1d, OnRadiusYChanged));

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
            var target = (RotatingContainer)d;
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
                typeof(RotatingContainer),
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
            var target = (RotatingContainer)d;
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
            var duration = TimeSpan.Parse(newDuration);
            this.KeyRightX.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration.TotalSeconds / 4));
            this.KeyRightY.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration.TotalSeconds / 4));
            this.KeyBottomX.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration.TotalSeconds / 2));
            this.KeyBottomY.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration.TotalSeconds / 2));
            this.KeyLeftX.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration.TotalSeconds * 3 / 4));
            this.KeyLeftY.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration.TotalSeconds * 3 / 4));
            this.KeyTopX.KeyTime = KeyTime.FromTimeSpan(duration);
            this.KeyTopY.KeyTime = KeyTime.FromTimeSpan(duration);
        }
        #endregion

        private void UpdateRadius()
        {
            this.KeyRightX.Value = RadiusX;
            this.KeyRightY.Value = RadiusY;
            this.KeyBottomY.Value = 2 * RadiusY;
            this.KeyLeftX.Value = -RadiusX;
            this.KeyLeftY.Value = RadiusY;
        }

        public RotatingContainer()
        {
            InitializeComponent();
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                this.Loaded += new RoutedEventHandler(RotatingContainer_Loaded);
        }

        void RotatingContainer_Loaded(object sender, RoutedEventArgs e)
        {
            RotationStoryboard.Begin();
        }
    }
}