using System;
using System.ComponentModel;
using WinRTXamlToolkit.AwaitableUI;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Debug = System.Diagnostics.Debug;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Attached properties that extend the Image control class.
    /// </summary>
    public static class ImageExtensions
    {
        #region FadeInOnLoaded
        /// <summary>
        /// FadeInOnLoaded Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty FadeInOnLoadedProperty =
            DependencyProperty.RegisterAttached(
                "FadeInOnLoaded",
                typeof(bool),
                typeof(ImageExtensions),
                new PropertyMetadata(false, OnFadeInOnLoadedChanged));

        /// <summary>
        /// Gets the FadeInOnLoaded property. This dependency property 
        /// indicates whether the image should be transparent and fade in into view only when loaded.
        /// </summary>
        public static bool GetFadeInOnLoaded(DependencyObject d)
        {
            return (bool)d.GetValue(FadeInOnLoadedProperty);
        }

        /// <summary>
        /// Sets the FadeInOnLoaded property. This dependency property 
        /// indicates whether the image should be transparent and fade in into view only when loaded.
        /// </summary>
        public static void SetFadeInOnLoaded(DependencyObject d, bool value)
        {
            d.SetValue(FadeInOnLoadedProperty, value);
        }

        /// <summary>
        /// Handles changes to the FadeInOnLoaded property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnFadeInOnLoadedChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool newFadeInOnLoaded = (bool)d.GetValue(FadeInOnLoadedProperty);
            var image = (Image)d;

            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            if (newFadeInOnLoaded)
            {
                var handler = new FadeInOnLoadedHandler((Image)d);
                SetFadeInOnLoadedHandler(d, handler);

#pragma warning disable 4014
                image.Dispatcher.RunAsync(
                    CoreDispatcherPriority.High,
                    () => image.SetBinding(
                        SourceProperty,
                        new Binding
                        {
                            Path = new PropertyPath("Source"),
                            Source = image
                        }));
#pragma warning restore 4014
            }
            else
            {
                var handler = GetFadeInOnLoadedHandler(d);
                SetFadeInOnLoadedHandler(d, null);
                handler.Detach();
                image.SetValue(
                    ImageExtensions.SourceProperty,
                    null);
            }
        }
        #endregion

        #region FadeInOnLoadedHandler
        /// <summary>
        /// FadeInOnLoadedHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty FadeInOnLoadedHandlerProperty =
            DependencyProperty.RegisterAttached(
                "FadeInOnLoadedHandler",
                typeof(FadeInOnLoadedHandler),
                typeof(ImageExtensions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the FadeInOnLoadedHandler property. This dependency property 
        /// indicates the handler for the FadeInOnLoaded property.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static FadeInOnLoadedHandler GetFadeInOnLoadedHandler(DependencyObject d)
        {
            return (FadeInOnLoadedHandler)d.GetValue(FadeInOnLoadedHandlerProperty);
        }

        /// <summary>
        /// Sets the FadeInOnLoadedHandler property. This dependency property 
        /// indicates the handler for the FadeInOnLoaded property.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetFadeInOnLoadedHandler(DependencyObject d, FadeInOnLoadedHandler value)
        {
            d.SetValue(FadeInOnLoadedHandlerProperty, value);
        }
        #endregion

        #region Source
        /// <summary>
        /// Source Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached(
                "Source",
                typeof(object),
                typeof(ImageExtensions),
                new PropertyMetadata(null, OnSourceChanged));

        /// <summary>
        /// Gets the Source property. This dependency property 
        /// indicates the Image.Source that supports property change handling.
        /// </summary>
        public static object GetSource(DependencyObject d)
        {
            return (object)d.GetValue(SourceProperty);
        }

        /// <summary>
        /// Sets the Source property. This dependency property 
        /// indicates the Image.Source that supports property change handling.
        /// </summary>
        public static void SetSource(DependencyObject d, object value)
        {
            d.SetValue(SourceProperty, value);
        }

        /// <summary>
        /// Handles changes to the Source property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool fadeInOnLoaded = GetFadeInOnLoaded(d);

            if (fadeInOnLoaded)
            {
                var handler = GetFadeInOnLoadedHandler(d);

                if (handler != null)
                    handler.Detach();

                SetFadeInOnLoadedHandler(d, new FadeInOnLoadedHandler((Image)d));
            }
        }
        #endregion

        #region CustomSource
        /// <summary>
        /// CustomSource Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty CustomSourceProperty =
            DependencyProperty.RegisterAttached(
                "CustomSource",
                typeof(string),
                typeof(ImageExtensions),
                new PropertyMetadata(null, OnCustomSourceChanged));

        /// <summary>
        /// Gets the CustomSource property. This dependency property 
        /// indicates the location of the image file to be used as a source of the Image.
        /// </summary>
        /// <remarks>
        /// This property can be used to use custom loading and tracing code for the Source,
        /// though it currently requires modifying the toolkit code.
        /// </remarks>
        public static string GetCustomSource(DependencyObject d)
        {
            return (string)d.GetValue(CustomSourceProperty);
        }

        /// <summary>
        /// Sets the CustomSource property. This dependency property 
        /// indicates the location of the image file to be used as a source of the Image.
        /// </summary>
        /// <remarks>
        /// This property can be used to use custom loading and tracing code for the Source,
        /// though it currently requires modifying the toolkit code.
        /// </remarks>
        public static void SetCustomSource(DependencyObject d, string value)
        {
            d.SetValue(CustomSourceProperty, value);
        }

        /// <summary>
        /// Handles changes to the CustomSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static async void OnCustomSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string newCustomSource = (string)d.GetValue(CustomSourceProperty);

            Debug.Assert(d is Image);

            var image = (Image)d;

            //DC.Trace("ImageCreate: " + newCustomSource);
            //var bi = await BitmapImageLoadExtensions.LoadAsync(
            //    Package.Current.InstalledLocation, newCustomSource);
            //image.Source = bi;
            image.Source = new BitmapImage(new Uri("ms-appx:///" + newCustomSource));

            await image.WaitForUnloadedAsync();
            //DC.Trace("ImageDispose: " + newCustomSource);
            image.Source = null;

            GC.Collect();
        }
        #endregion
    }

    /// <summary>
    /// Handles fade in animations on mage controls.
    /// </summary>
    public class FadeInOnLoadedHandler
    {
        private Image _image;
        private BitmapImage _source;
        private double _targetOpacity;

        /// <summary>
        /// Initializes a new instance of the <see cref="FadeInOnLoadedHandler" /> class.
        /// </summary>
        /// <param name="image">The image.</param>
        public FadeInOnLoadedHandler(Image image)
        {
            Attach(image);
        }

        /// <summary>
        /// Attaches the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        private void Attach(Image image)
        {
            _image = image;
            _source = image.Source as BitmapImage;

            if (_source != null)
            {
                if (_source.PixelWidth > 0)
                {
                    image.Opacity = 1;
                    _image = null;
                    _source = null;
                    return;
                }

                _source.ImageOpened += OnSourceImageOpened;
                _source.ImageFailed += OnSourceImageFailed;
            }

            image.Unloaded += OnImageUnloaded;

            _targetOpacity = image.Opacity == 0.0 ? 1.0 : image.Opacity;
            image.Opacity = 0;
        }

        private void OnSourceImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var source = (BitmapImage)sender;
            Debug.WriteLine("Failed: " + source.UriSource);
            source.ImageOpened -= OnSourceImageOpened;
            source.ImageFailed -= OnSourceImageFailed;
        }

        private void OnSourceImageOpened(object sender, RoutedEventArgs e)
        {
            var source = (BitmapImage)sender;

            source.ImageOpened -= OnSourceImageOpened;
            source.ImageFailed -= OnSourceImageFailed;

#pragma warning disable 4014
            _image.FadeInCustom(TimeSpan.FromSeconds(1), null, _targetOpacity);
#pragma warning restore 4014
        }

        private void OnImageUnloaded(object sender, RoutedEventArgs e)
        {
            Detach();
        }

        internal void Detach()
        {
            if (_source != null)
            {
                _source.ImageOpened -= OnSourceImageOpened;
                _source.ImageFailed -= OnSourceImageFailed;
            }

            if (_image != null)
            {
                _image.Unloaded -= OnImageUnloaded;
                _image.CleanUpPreviousFadeStoryboard();
                _image.Opacity = _targetOpacity;
                _image = null;
            }
        }
    }
}
