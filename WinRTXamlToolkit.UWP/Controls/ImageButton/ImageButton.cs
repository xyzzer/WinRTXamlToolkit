using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Imaging;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// A Button control templated to use images for its states.
    /// Provides ImageSource properties for each of the button's states as well as mechanisms for generating missing images.
    /// </summary>
    [TemplatePart(Name = NormalStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = HoverStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = HoverStateRecycledNormalStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = HoverStateRecycledPressedStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = PressedStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = DisabledStateImageName, Type = typeof(Image))]
    public class ImageButton : Button
    {
        private const string NormalStateImageName = "PART_NormalStateImage";
        private const string HoverStateImageName = "PART_HoverStateImage";
        private const string HoverStateRecycledNormalStateImageName = "PART_HoverStateRecycledNormalStateImage";
        private const string HoverStateRecycledPressedStateImageName = "PART_HoverStateRecycledPressedStateImage";
        private const string PressedStateImageName = "PART_PressedStateImage";
        private const string DisabledStateImageName = "PART_DisabledStateImage";
        private Image _normalStateImage;
        private Image _hoverStateImage;
        private Image _hoverStateRecycledNormalStateImage;
        private Image _hoverStateRecycledPressedStateImage;
        private Image _pressedStateImage;
        private Image _disabledStateImage;
        private readonly TaskCompletionSource<bool> _waitForApplyTemplateTaskSource = new TaskCompletionSource<bool>(false);

        #region Stretch
        /// <summary>
        /// Stretch Dependency Property
        /// </summary>
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(
                "Stretch",
                typeof(Stretch),
                typeof(ImageButton),
                new PropertyMetadata(Stretch.None));

        /// <summary>
        /// Gets or sets the Stretch property. This dependency property 
        /// indicates how an Image should be stretched to fill the button.
        /// </summary>
        /// <remarks>
        /// A value of the Stretch enumeration specifies how the source image is
        /// applied if the Height and Width of the ImageButton are specified and are different
        /// than the source image's height and width. The default value is None.
        /// </remarks>
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
        #endregion
        
        #region RecyclePressedStateImageForHover
        /// <summary>
        /// RecyclePressedStateImageForHover Dependency Property
        /// </summary>
        public static readonly DependencyProperty RecyclePressedStateImageForHoverProperty =
            DependencyProperty.Register(
                "RecyclePressedStateImageForHover",
                typeof(bool),
                typeof(ImageButton),
                new PropertyMetadata(false, OnRecyclePressedStateImageForHoverChanged));

        /// <summary>
        /// Gets or sets the RecyclePressedStateImageForHover property. This dependency property 
        /// indicates whether the PressedStateImageSource should also be used for hover state with 0.5 opacity.
        /// </summary>
        public bool RecyclePressedStateImageForHover
        {
            get { return (bool)GetValue(RecyclePressedStateImageForHoverProperty); }
            set { SetValue(RecyclePressedStateImageForHoverProperty, value); }
        }

        /// <summary>
        /// Handles changes to the RecyclePressedStateImageForHover property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnRecyclePressedStateImageForHoverChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageButton)d;
            bool oldRecyclePressedStateImageForHover = (bool)e.OldValue;
            bool newRecyclePressedStateImageForHover = target.RecyclePressedStateImageForHover;
            target.OnRecyclePressedStateImageForHoverChanged(oldRecyclePressedStateImageForHover, newRecyclePressedStateImageForHover);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the RecyclePressedStateImageForHover property.
        /// </summary>
        /// <param name="oldRecyclePressedStateImageForHover">The old RecyclePressedStateImageForHover value</param>
        /// <param name="newRecyclePressedStateImageForHover">The new RecyclePressedStateImageForHover value</param>
        protected virtual void OnRecyclePressedStateImageForHoverChanged(
            bool oldRecyclePressedStateImageForHover, bool newRecyclePressedStateImageForHover)
        {
            UpdateRecycledHoverStateImages();
        }
        #endregion

        #region NormalStateImageSource
        /// <summary>
        /// NormalStateImageSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty NormalStateImageSourceProperty =
            DependencyProperty.Register(
                "NormalStateImageSource",
                typeof(ImageSource),
                typeof(ImageButton),
                new PropertyMetadata(null, OnNormalStateImageSourceChanged));

        /// <summary>
        /// Gets or sets the NormalStateImageSource property. This dependency property 
        /// indicates the ImageSource for the normal state.
        /// </summary>
        public ImageSource NormalStateImageSource
        {
            get { return (ImageSource)GetValue(NormalStateImageSourceProperty); }
            set { SetValue(NormalStateImageSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the NormalStateImageSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnNormalStateImageSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageButton)d;
            ImageSource oldNormalStateImageSource = (ImageSource)e.OldValue;
            ImageSource newNormalStateImageSource = target.NormalStateImageSource;
            target.OnNormalStateImageSourceChanged(oldNormalStateImageSource, newNormalStateImageSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the NormalStateImageSource property.
        /// </summary>
        /// <param name="oldNormalStateImageSource">The old NormalStateImageSource value</param>
        /// <param name="newNormalStateImageSource">The new NormalStateImageSource value</param>
        protected virtual void OnNormalStateImageSourceChanged(
            ImageSource oldNormalStateImageSource, ImageSource newNormalStateImageSource)
        {
            if (newNormalStateImageSource == null)
                Debugger.Break();

            UpdateNormalStateImage();
            UpdateHoverStateImage();
            UpdateRecycledHoverStateImages();
            UpdatePressedStateImage();
            UpdateDisabledStateImage();
        }
        #endregion

        #region HoverStateImageSource
        /// <summary>
        /// HoverStateImageSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty HoverStateImageSourceProperty =
            DependencyProperty.Register(
                "HoverStateImageSource",
                typeof(ImageSource),
                typeof(ImageButton),
                new PropertyMetadata(null, OnHoverStateImageSourceChanged));

        /// <summary>
        /// Gets or sets the HoverStateImageSource property. This dependency property 
        /// indicates the ImageSource to use when the pointer is over the button.
        /// </summary>
        public ImageSource HoverStateImageSource
        {
            get { return (ImageSource)GetValue(HoverStateImageSourceProperty); }
            set { SetValue(HoverStateImageSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the HoverStateImageSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnHoverStateImageSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageButton)d;
            ImageSource oldHoverStateImageSource = (ImageSource)e.OldValue;
            ImageSource newHoverStateImageSource = target.HoverStateImageSource;
            target.OnHoverStateImageSourceChanged(oldHoverStateImageSource, newHoverStateImageSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the HoverStateImageSource property.
        /// </summary>
        /// <param name="oldHoverStateImageSource">The old HoverStateImageSource value</param>
        /// <param name="newHoverStateImageSource">The new HoverStateImageSource value</param>
        protected virtual void OnHoverStateImageSourceChanged(
            ImageSource oldHoverStateImageSource, ImageSource newHoverStateImageSource)
        {
            UpdateHoverStateImage();
        }
        #endregion

        #region PressedStateImageSource
        /// <summary>
        /// PressedStateImageSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty PressedStateImageSourceProperty =
            DependencyProperty.Register(
                "PressedStateImageSource",
                typeof(ImageSource),
                typeof(ImageButton),
                new PropertyMetadata(null, OnPressedStateImageSourceChanged));

        /// <summary>
        /// Gets or sets the PressedStateImageSource property. This dependency property 
        /// indicates the ImageSource to use when the button is pressed.
        /// </summary>
        public ImageSource PressedStateImageSource
        {
            get { return (ImageSource)GetValue(PressedStateImageSourceProperty); }
            set { SetValue(PressedStateImageSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the PressedStateImageSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnPressedStateImageSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageButton)d;
            ImageSource oldPressedStateImageSource = (ImageSource)e.OldValue;
            ImageSource newPressedStateImageSource = target.PressedStateImageSource;
            target.OnPressedStateImageSourceChanged(oldPressedStateImageSource, newPressedStateImageSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the PressedStateImageSource property.
        /// </summary>
        /// <param name="oldPressedStateImageSource">The old PressedStateImageSource value</param>
        /// <param name="newPressedStateImageSource">The new PressedStateImageSource value</param>
        protected virtual void OnPressedStateImageSourceChanged(
            ImageSource oldPressedStateImageSource, ImageSource newPressedStateImageSource)
        {
            UpdatePressedStateImage();
            UpdateRecycledHoverStateImages();
        }
        #endregion

        #region DisabledStateImageSource
        /// <summary>
        /// DisabledStateImageSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty DisabledStateImageSourceProperty =
            DependencyProperty.Register(
                "DisabledStateImageSource",
                typeof(ImageSource),
                typeof(ImageButton),
                new PropertyMetadata(null, OnDisabledStateImageSourceChanged));

        /// <summary>
        /// Gets or sets the DisabledStateImageSource property. This dependency property 
        /// indicates the ImageSource to use when the button is Disabled.
        /// </summary>
        public ImageSource DisabledStateImageSource
        {
            get { return (ImageSource)GetValue(DisabledStateImageSourceProperty); }
            set { SetValue(DisabledStateImageSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DisabledStateImageSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnDisabledStateImageSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageButton)d;
            ImageSource oldDisabledStateImageSource = (ImageSource)e.OldValue;
            ImageSource newDisabledStateImageSource = target.DisabledStateImageSource;
            target.OnDisabledStateImageSourceChanged(oldDisabledStateImageSource, newDisabledStateImageSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the DisabledStateImageSource property.
        /// </summary>
        /// <param name="oldDisabledStateImageSource">The old DisabledStateImageSource value</param>
        /// <param name="newDisabledStateImageSource">The new DisabledStateImageSource value</param>
        protected virtual void OnDisabledStateImageSourceChanged(
            ImageSource oldDisabledStateImageSource, ImageSource newDisabledStateImageSource)
        {
            if (_disabledStateImage != null)
                _disabledStateImage.Source = newDisabledStateImageSource;
        }
        #endregion

        #region NormalStateImageUriSource
        /// <summary>
        /// NormalStateImageUriSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty NormalStateImageUriSourceProperty =
            DependencyProperty.Register(
                "NormalStateImageUriSource",
                typeof(Uri),
                typeof(ImageButton),
                new PropertyMetadata(null, OnNormalStateImageUriSourceChanged));

        /// <summary>
        /// Gets or sets the NormalStateImageUriSource property. This dependency property 
        /// indicates the uri to use for the normal image source.
        /// </summary>
        public Uri NormalStateImageUriSource
        {
            get { return (Uri)GetValue(NormalStateImageUriSourceProperty); }
            set { SetValue(NormalStateImageUriSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the NormalStateImageUriSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnNormalStateImageUriSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageButton)d;
            Uri oldNormalStateImageUriSource = (Uri)e.OldValue;
            Uri newNormalStateImageUriSource = target.NormalStateImageUriSource;
            target.OnNormalStateImageUriSourceChanged(oldNormalStateImageUriSource, newNormalStateImageUriSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the NormalStateImageUriSource property.
        /// </summary>
        /// <param name="oldNormalStateImageUriSource">The old NormalStateImageUriSource value</param>
        /// <param name="newNormalStateImageUriSource">The new NormalStateImageUriSource value</param>
        private void OnNormalStateImageUriSourceChanged(
            Uri oldNormalStateImageUriSource, Uri newNormalStateImageUriSource)
        {
            if (newNormalStateImageUriSource != null)
                this.NormalStateImageSource = new BitmapImage(newNormalStateImageUriSource);
            else
                this.NormalStateImageSource = null;
        }
        #endregion

        #region HoverStateImageUriSource
        /// <summary>
        /// HoverStateImageUriSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty HoverStateImageUriSourceProperty =
            DependencyProperty.Register(
                "HoverStateImageUriSource",
                typeof(Uri),
                typeof(ImageButton),
                new PropertyMetadata(null, OnHoverStateImageUriSourceChanged));

        /// <summary>
        /// Gets or sets the HoverStateImageUriSource property. This dependency property 
        /// indicates the uri to use for the normal image source.
        /// </summary>
        public Uri HoverStateImageUriSource
        {
            get { return (Uri)GetValue(HoverStateImageUriSourceProperty); }
            set { SetValue(HoverStateImageUriSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the HoverStateImageUriSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnHoverStateImageUriSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageButton)d;
            Uri oldHoverStateImageUriSource = (Uri)e.OldValue;
            Uri newHoverStateImageUriSource = target.HoverStateImageUriSource;
            target.OnHoverStateImageUriSourceChanged(oldHoverStateImageUriSource, newHoverStateImageUriSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the HoverStateImageUriSource property.
        /// </summary>
        /// <param name="oldHoverStateImageUriSource">The old HoverStateImageUriSource value</param>
        /// <param name="newHoverStateImageUriSource">The new HoverStateImageUriSource value</param>
        private void OnHoverStateImageUriSourceChanged(
            Uri oldHoverStateImageUriSource, Uri newHoverStateImageUriSource)
        {
            if (newHoverStateImageUriSource != null)
                this.HoverStateImageSource = new BitmapImage(newHoverStateImageUriSource);
            else
                this.HoverStateImageSource = null;
        }
        #endregion

        #region PressedStateImageUriSource
        /// <summary>
        /// PressedStateImageUriSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty PressedStateImageUriSourceProperty =
            DependencyProperty.Register(
                "PressedStateImageUriSource",
                typeof(Uri),
                typeof(ImageButton),
                new PropertyMetadata(null, OnPressedStateImageUriSourceChanged));

        /// <summary>
        /// Gets or sets the PressedStateImageUriSource property. This dependency property 
        /// indicates the uri to use for the normal image source.
        /// </summary>
        public Uri PressedStateImageUriSource
        {
            get { return (Uri)GetValue(PressedStateImageUriSourceProperty); }
            set { SetValue(PressedStateImageUriSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the PressedStateImageUriSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnPressedStateImageUriSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageButton)d;
            Uri oldPressedStateImageUriSource = (Uri)e.OldValue;
            Uri newPressedStateImageUriSource = target.PressedStateImageUriSource;
            target.OnPressedStateImageUriSourceChanged(oldPressedStateImageUriSource, newPressedStateImageUriSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the PressedStateImageUriSource property.
        /// </summary>
        /// <param name="oldPressedStateImageUriSource">The old PressedStateImageUriSource value</param>
        /// <param name="newPressedStateImageUriSource">The new PressedStateImageUriSource value</param>
        private void OnPressedStateImageUriSourceChanged(
            Uri oldPressedStateImageUriSource, Uri newPressedStateImageUriSource)
        {
            if (newPressedStateImageUriSource != null)
                this.PressedStateImageSource = new BitmapImage(newPressedStateImageUriSource);
            else
                this.PressedStateImageSource = null;
        }
        #endregion

        #region DisabledStateImageUriSource
        /// <summary>
        /// DisabledStateImageUriSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty DisabledStateImageUriSourceProperty =
            DependencyProperty.Register(
                "DisabledStateImageUriSource",
                typeof(Uri),
                typeof(ImageButton),
                new PropertyMetadata(null, OnDisabledStateImageUriSourceChanged));

        /// <summary>
        /// Gets or sets the DisabledStateImageUriSource property. This dependency property 
        /// indicates the uri to use for the normal image source.
        /// </summary>
        public Uri DisabledStateImageUriSource
        {
            get { return (Uri)GetValue(DisabledStateImageUriSourceProperty); }
            set { SetValue(DisabledStateImageUriSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DisabledStateImageUriSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnDisabledStateImageUriSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageButton)d;
            Uri oldDisabledStateImageUriSource = (Uri)e.OldValue;
            Uri newDisabledStateImageUriSource = target.DisabledStateImageUriSource;
            target.OnDisabledStateImageUriSourceChanged(oldDisabledStateImageUriSource, newDisabledStateImageUriSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the DisabledStateImageUriSource property.
        /// </summary>
        /// <param name="oldDisabledStateImageUriSource">The old DisabledStateImageUriSource value</param>
        /// <param name="newDisabledStateImageUriSource">The new DisabledStateImageUriSource value</param>
        private void OnDisabledStateImageUriSourceChanged(
            Uri oldDisabledStateImageUriSource, Uri newDisabledStateImageUriSource)
        {
            if (newDisabledStateImageUriSource != null)
                this.DisabledStateImageSource = new BitmapImage(newDisabledStateImageUriSource);
            else
                this.DisabledStateImageSource = null;
        }
        #endregion

        #region GenerateMissingImages
        /// <summary>
        /// GenerateMissingImages Dependency Property
        /// </summary>
        public static readonly DependencyProperty GenerateMissingImagesProperty =
            DependencyProperty.Register(
                "GenerateMissingImages",
                typeof(bool),
                typeof(ImageButton),
                new PropertyMetadata(false, OnGenerateMissingImagesChanged));

        /// <summary>
        /// Gets or sets the GenerateMissingImages property. This dependency property 
        /// indicates whether the missing images should be generated from the normal state image.
        /// </summary>
        public bool GenerateMissingImages
        {
            get { return (bool)GetValue(GenerateMissingImagesProperty); }
            set { SetValue(GenerateMissingImagesProperty, value); }
        }

        /// <summary>
        /// Handles changes to the GenerateMissingImages property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnGenerateMissingImagesChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageButton)d;
            bool oldGenerateMissingImages = (bool)e.OldValue;
            bool newGenerateMissingImages = target.GenerateMissingImages;
            target.OnGenerateMissingImagesChanged(oldGenerateMissingImages, newGenerateMissingImages);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the GenerateMissingImages property.
        /// </summary>
        /// <param name="oldGenerateMissingImages">The old GenerateMissingImages value</param>
        /// <param name="newGenerateMissingImages">The new GenerateMissingImages value</param>
        protected virtual void OnGenerateMissingImagesChanged(
            bool oldGenerateMissingImages, bool newGenerateMissingImages)
        {
            UpdateHoverStateImage();
            UpdatePressedStateImage();
            UpdateDisabledStateImage();
        }
        #endregion

        #region GeneratedHoverStateLightenAmount
        /// <summary>
        /// GeneratedHoverStateLightenAmount Dependency Property
        /// </summary>
        public static readonly DependencyProperty GeneratedHoverStateLightenAmountProperty =
            DependencyProperty.Register(
                "GeneratedHoverStateLightenAmount",
                typeof(double),
                typeof(ImageButton),
                new PropertyMetadata(0.25, OnGeneratedHoverStateLightenAmountChanged));

        /// <summary>
        /// Gets or sets the GeneratedHoverStateLightenAmount property. This dependency property 
        /// indicates the lightening amount to use when generating the hover state image.
        /// </summary>
        public double GeneratedHoverStateLightenAmount
        {
            get { return (double)GetValue(GeneratedHoverStateLightenAmountProperty); }
            set { SetValue(GeneratedHoverStateLightenAmountProperty, value); }
        }

        /// <summary>
        /// Handles changes to the GeneratedHoverStateLightenAmount property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnGeneratedHoverStateLightenAmountChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageButton)d;
            double oldGeneratedHoverStateLightenAmount = (double)e.OldValue;
            double newGeneratedHoverStateLightenAmount = target.GeneratedHoverStateLightenAmount;
            target.OnGeneratedHoverStateLightenAmountChanged(oldGeneratedHoverStateLightenAmount, newGeneratedHoverStateLightenAmount);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the GeneratedHoverStateLightenAmount property.
        /// </summary>
        /// <param name="oldGeneratedHoverStateLightenAmount">The old GeneratedHoverStateLightenAmount value</param>
        /// <param name="newGeneratedHoverStateLightenAmount">The new GeneratedHoverStateLightenAmount value</param>
        protected virtual void OnGeneratedHoverStateLightenAmountChanged(
            double oldGeneratedHoverStateLightenAmount, double newGeneratedHoverStateLightenAmount)
        {
            UpdateHoverStateImage();
        }
        #endregion

        #region GeneratedPressedStateLightenAmount
        /// <summary>
        /// GeneratedPressedStateLightenAmount Dependency Property
        /// </summary>
        public static readonly DependencyProperty GeneratedPressedStateLightenAmountProperty =
            DependencyProperty.Register(
                "GeneratedPressedStateLightenAmount",
                typeof(double),
                typeof(ImageButton),
                new PropertyMetadata(0.5, OnGeneratedPressedStateLightenAmountChanged));

        /// <summary>
        /// Gets or sets the GeneratedPressedStateLightenAmount property. This dependency property 
        /// indicates the lightening amount to use when generating the pressed state image.
        /// </summary>
        public double GeneratedPressedStateLightenAmount
        {
            get { return (double)GetValue(GeneratedPressedStateLightenAmountProperty); }
            set { SetValue(GeneratedPressedStateLightenAmountProperty, value); }
        }

        /// <summary>
        /// Handles changes to the GeneratedPressedStateLightenAmount property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnGeneratedPressedStateLightenAmountChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageButton)d;
            double oldGeneratedPressedStateLightenAmount = (double)e.OldValue;
            double newGeneratedPressedStateLightenAmount = target.GeneratedPressedStateLightenAmount;
            target.OnGeneratedPressedStateLightenAmountChanged(oldGeneratedPressedStateLightenAmount, newGeneratedPressedStateLightenAmount);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the GeneratedPressedStateLightenAmount property.
        /// </summary>
        /// <param name="oldGeneratedPressedStateLightenAmount">The old GeneratedPressedStateLightenAmount value</param>
        /// <param name="newGeneratedPressedStateLightenAmount">The new GeneratedPressedStateLightenAmount value</param>
        protected virtual void OnGeneratedPressedStateLightenAmountChanged(
            double oldGeneratedPressedStateLightenAmount, double newGeneratedPressedStateLightenAmount)
        {
            UpdatePressedStateImage();
        }
        #endregion

        #region GeneratedDisabledStateGrayscaleAmount
        /// <summary>
        /// GeneratedDisabledStateGrayscaleAmount Dependency Property
        /// </summary>
        public static readonly DependencyProperty GeneratedDisabledStateGrayscaleAmountProperty =
            DependencyProperty.Register(
                "GeneratedDisabledStateGrayscaleAmount",
                typeof(double),
                typeof(ImageButton),
                new PropertyMetadata(1.0, OnGeneratedDisabledStateGrayscaleAmountChanged));

        /// <summary>
        /// Gets or sets the GeneratedDisabledStateGrayscaleAmount property. This dependency property 
        /// indicates the grayscale amount to use when generating the disabled state image.
        /// </summary>
        public double GeneratedDisabledStateGrayscaleAmount
        {
            get { return (double)GetValue(GeneratedDisabledStateGrayscaleAmountProperty); }
            set { SetValue(GeneratedDisabledStateGrayscaleAmountProperty, value); }
        }

        /// <summary>
        /// Handles changes to the GeneratedDisabledStateGrayscaleAmount property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnGeneratedDisabledStateGrayscaleAmountChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageButton)d;
            double oldGeneratedDisabledStateGrayscaleAmount = (double)e.OldValue;
            double newGeneratedDisabledStateGrayscaleAmount = target.GeneratedDisabledStateGrayscaleAmount;
            target.OnGeneratedDisabledStateGrayscaleAmountChanged(oldGeneratedDisabledStateGrayscaleAmount, newGeneratedDisabledStateGrayscaleAmount);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the GeneratedDisabledStateGrayscaleAmount property.
        /// </summary>
        /// <param name="oldGeneratedDisabledStateGrayscaleAmount">The old GeneratedDisabledStateGrayscaleAmount value</param>
        /// <param name="newGeneratedDisabledStateGrayscaleAmount">The new GeneratedDisabledStateGrayscaleAmount value</param>
        protected virtual void OnGeneratedDisabledStateGrayscaleAmountChanged(
            double oldGeneratedDisabledStateGrayscaleAmount, double newGeneratedDisabledStateGrayscaleAmount)
        {
            UpdateDisabledStateImage();
        }
        #endregion

        #region GenerateHoverStateImage()
        private async void GenerateHoverStateImage()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            var wb = new WriteableBitmap(1, 1);
            await wb.FromBitmapImage((BitmapImage)NormalStateImageSource);
            await wb.WaitForLoadedAsync();
            wb.Lighten(GeneratedHoverStateLightenAmount);
            _hoverStateImage.Source = wb;
        } 
        #endregion

        #region GeneratePressedStateImage()
        private async void GeneratePressedStateImage()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            var wb = new WriteableBitmap(1, 1);
            await wb.FromBitmapImage((BitmapImage)NormalStateImageSource);
            await wb.WaitForLoadedAsync();
            wb.Lighten(GeneratedPressedStateLightenAmount);
            _pressedStateImage.Source = wb;
        } 
        #endregion

        #region GenerateDisabledStateImage()
        private async void GenerateDisabledStateImage()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            var wb = new WriteableBitmap(1, 1);
            await wb.FromBitmapImage((BitmapImage)NormalStateImageSource);
            await wb.WaitForLoadedAsync();
            wb.Grayscale(GeneratedDisabledStateGrayscaleAmount);
            _disabledStateImage.Source = wb;
        } 
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageButton" /> class.
        /// </summary>
        public ImageButton()
        {
            DefaultStyleKey = typeof (ImageButton);
        }

        #region OnApplyTemplate()
        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _normalStateImage = (Image)GetTemplateChild(NormalStateImageName);
            _hoverStateImage = (Image)GetTemplateChild(HoverStateImageName);
            _hoverStateRecycledNormalStateImage = (Image)GetTemplateChild(HoverStateRecycledNormalStateImageName);
            _hoverStateRecycledPressedStateImage = (Image)GetTemplateChild(HoverStateRecycledPressedStateImageName);
            _pressedStateImage = (Image)GetTemplateChild(PressedStateImageName);
            _disabledStateImage = (Image)GetTemplateChild(DisabledStateImageName);

            _waitForApplyTemplateTaskSource.SetResult(true);
            // No need to call these now, since completing the task above should unblock these
            //UpdateNormalStateImage();
            //UpdateHoverStateImage();
            //UpdateRecycledHoverStateImages();
            //UpdatePressedStateImage();
            //UpdateDisabledStateImage();
        }
        #endregion

        #region UpdateNormalStateImage()
        private async void UpdateNormalStateImage()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            await _waitForApplyTemplateTaskSource.Task;

            _normalStateImage.Source = NormalStateImageSource;
        }
        #endregion

        #region UpdateHoverStateImage()
        private async void UpdateHoverStateImage()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            await _waitForApplyTemplateTaskSource.Task;

            if (HoverStateImageSource != null)
            {
                _hoverStateImage.Source = HoverStateImageSource;
            }
            else if (
                GenerateMissingImages &&
                NormalStateImageSource != null)
            {
#pragma warning disable 4014
                GenerateHoverStateImage();
#pragma warning restore 4014
            }

            // If hover state is still not set - need to use normal state at least to avoid missing image
            if (_hoverStateImage.Source == null)
                _hoverStateImage.Source = NormalStateImageSource;
        } 
        #endregion

        #region UpdateRecycledHoverStateImages()
        private async void UpdateRecycledHoverStateImages()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            await _waitForApplyTemplateTaskSource.Task;

            if (RecyclePressedStateImageForHover &&
                NormalStateImageSource != null)
                _hoverStateRecycledNormalStateImage.Source = NormalStateImageSource;
            else
                _hoverStateRecycledNormalStateImage.Source = null;

            if (RecyclePressedStateImageForHover &&
                PressedStateImageSource != null)
                _hoverStateRecycledPressedStateImage.Source = PressedStateImageSource;
            else
                _hoverStateRecycledPressedStateImage.Source = null;
        } 
        #endregion

        #region UpdatePressedStateImage()
        private async void UpdatePressedStateImage()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            await _waitForApplyTemplateTaskSource.Task;

            if (PressedStateImageSource != null)
            {
                _pressedStateImage.Source = PressedStateImageSource;
            }
            else if (
                GenerateMissingImages &&
                NormalStateImageSource != null)
            {
#pragma warning disable 4014
                GeneratePressedStateImage();
#pragma warning restore 4014
            }
        } 
        #endregion

        #region UpdateDisabledStateImage()
        private async void UpdateDisabledStateImage()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            await _waitForApplyTemplateTaskSource.Task;

            if (DisabledStateImageSource != null)
            {
                _disabledStateImage.Source = DisabledStateImageSource;
            }
            else if (
                GenerateMissingImages &&
                NormalStateImageSource != null)
            {
#pragma warning disable 4014
                GenerateDisabledStateImage();
#pragma warning restore 4014
            }
        } 
        #endregion
    }
}
