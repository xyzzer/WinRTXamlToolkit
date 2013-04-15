using System;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Imaging;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// A ToggleButton control templated to use images for its states.
    /// Provides ImageSource properties for each of the button's states as well as mechanisms for generating missing images.
    /// </summary>
    [TemplatePart(Name = NormalStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = HoverStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = HoverStateRecycledNormalStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = HoverStateRecycledPressedStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = PressedStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = DisabledStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = CheckedStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = CheckedHoverStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = CheckedHoverStateRecycledCheckedStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = CheckedHoverStateRecycledCheckedPressedStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = CheckedPressedStateImageName, Type = typeof(Image))]
    [TemplatePart(Name = CheckedDisabledStateImageName, Type = typeof(Image))]
    public class ImageToggleButton : ToggleButton
    {
        private const string NormalStateImageName = "PART_NormalStateImage";
        private const string HoverStateImageName = "PART_HoverStateImage";
        private const string HoverStateRecycledNormalStateImageName = "PART_HoverStateRecycledNormalStateImage";
        private const string HoverStateRecycledPressedStateImageName = "PART_HoverStateRecycledPressedStateImage";
        private const string PressedStateImageName = "PART_PressedStateImage";
        private const string DisabledStateImageName = "PART_DisabledStateImage";
        private const string CheckedStateImageName = "PART_CheckedStateImage";
        private const string CheckedHoverStateImageName = "PART_CheckedHoverStateImage";
        private const string CheckedHoverStateRecycledCheckedStateImageName = "PART_CheckedHoverStateRecycledCheckedStateImage";
        private const string CheckedHoverStateRecycledCheckedPressedStateImageName = "PART_CheckedHoverStateRecycledCheckedPressedStateImage";
        private const string CheckedPressedStateImageName = "PART_CheckedPressedStateImage";
        private const string CheckedDisabledStateImageName = "PART_CheckedDisabledStateImage";
        private Image _normalStateImage;
        private Image _hoverStateImage;
        private Image _hoverStateRecycledCheckedStateImage;
        private Image _hoverStateRecycledCheckedPressedStateImage;
        private Image _pressedStateImage;
        private Image _disabledStateImage;
        private Image _checkedStateImage;
        private Image _checkedHoverStateImage;
        private Image _checkedHoverStateRecycledCheckedStateImage;
        private Image _checkedHoverStateRecycledCheckedPressedStateImage;
        private Image _checkedPressedStateImage;
        private Image _checkedDisabledStateImage;

        #region Stretch
        /// <summary>
        /// Stretch Dependency Property
        /// </summary>
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(
                "Stretch",
                typeof(Stretch),
                typeof(ImageToggleButton),
                new PropertyMetadata(Stretch.None));

        /// <summary>
        /// Gets or sets the Stretch property. This dependency property 
        /// indicates how an Image should be stretched to fill the button.
        /// </summary>
        /// <remarks>
        /// A value of the Stretch enumeration specifies how the source image is
        /// applied if the Height and Width of the ImageToggleButton are specified and are different
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
                typeof(ImageToggleButton),
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
            var target = (ImageToggleButton)d;
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

        #region RecycleUncheckedStateImagesForCheckedStates
        /// <summary>
        /// RecycleUncheckedStateImagesForCheckedStates Dependency Property
        /// </summary>
        public static readonly DependencyProperty RecycleUncheckedStateImagesForCheckedStatesProperty =
            DependencyProperty.Register(
                "RecycleUncheckedStateImagesForCheckedStates",
                typeof(bool),
                typeof(ImageToggleButton),
                new PropertyMetadata(true, OnRecycleUncheckedStateImagesForCheckedStatesChanged));

        /// <summary>
        /// Gets or sets the RecycleUncheckedStateImagesForCheckedStates property. This dependency property 
        /// indicates whether the unchecked state images should be reused for checked states.
        /// </summary>
        public bool RecycleUncheckedStateImagesForCheckedStates
        {
            get { return (bool)GetValue(RecycleUncheckedStateImagesForCheckedStatesProperty); }
            set { SetValue(RecycleUncheckedStateImagesForCheckedStatesProperty, value); }
        }

        /// <summary>
        /// Handles changes to the RecycleUncheckedStateImagesForCheckedStates property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnRecycleUncheckedStateImagesForCheckedStatesChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageToggleButton)d;
            bool oldRecycleUncheckedStateImagesForCheckedStates = (bool)e.OldValue;
            bool newRecycleUncheckedStateImagesForCheckedStates = target.RecycleUncheckedStateImagesForCheckedStates;
            target.OnRecycleUncheckedStateImagesForCheckedStatesChanged(oldRecycleUncheckedStateImagesForCheckedStates, newRecycleUncheckedStateImagesForCheckedStates);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the RecycleUncheckedStateImagesForCheckedStates property.
        /// </summary>
        /// <param name="oldRecycleUncheckedStateImagesForCheckedStates">The old RecycleUncheckedStateImagesForCheckedStates value</param>
        /// <param name="newRecycleUncheckedStateImagesForCheckedStates">The new RecycleUncheckedStateImagesForCheckedStates value</param>
        protected virtual void OnRecycleUncheckedStateImagesForCheckedStatesChanged(
            bool oldRecycleUncheckedStateImagesForCheckedStates, bool newRecycleUncheckedStateImagesForCheckedStates)
        {
            UpdateCheckedStateImage();
            UpdateCheckedHoverStateImage();
            UpdateCheckedPressedStateImage();
            UpdateCheckedDisabledStateImage();
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
                typeof(ImageToggleButton),
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
            var target = (ImageToggleButton)d;
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
            UpdateNormalStateImage();
            UpdateHoverStateImage();
            UpdateRecycledHoverStateImages();
            UpdatePressedStateImage();
            UpdateDisabledStateImage();
            UpdateCheckedStateImage();
            UpdateCheckedHoverStateImage();
            UpdateRecycledCheckedHoverStateImages();
            UpdateCheckedPressedStateImage();
            UpdateCheckedDisabledStateImage();
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
                typeof(ImageToggleButton),
                new PropertyMetadata(null, OnHoverStateImageSourceChanged));

        /// <summary>
        /// Gets or sets the HoverStateImageSource property. This dependency property 
        /// indicates the ImageSource to use when the pointer is over the ToggleButton.
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
            var target = (ImageToggleButton)d;
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
            UpdateCheckedHoverStateImage();
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
                typeof(ImageToggleButton),
                new PropertyMetadata(null, OnPressedStateImageSourceChanged));

        /// <summary>
        /// Gets or sets the PressedStateImageSource property. This dependency property 
        /// indicates the ImageSource to use when the ToggleButton is pressed.
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
            var target = (ImageToggleButton)d;
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

            UpdateCheckedStateImage();
            UpdateCheckedHoverStateImage();
            UpdateRecycledCheckedHoverStateImages();
            UpdateCheckedPressedStateImage();
            UpdateCheckedDisabledStateImage();
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
                typeof(ImageToggleButton),
                new PropertyMetadata(null, OnDisabledStateImageSourceChanged));

        /// <summary>
        /// Gets or sets the DisabledStateImageSource property. This dependency property 
        /// indicates the ImageSource to use when the ToggleButton is Disabled.
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
            var target = (ImageToggleButton)d;
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

        #region CheckedStateImageSource
        /// <summary>
        /// CheckedStateImageSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty CheckedStateImageSourceProperty =
            DependencyProperty.Register(
                "CheckedStateImageSource",
                typeof(ImageSource),
                typeof(ImageToggleButton),
                new PropertyMetadata(null, OnCheckedStateImageSourceChanged));

        /// <summary>
        /// Gets or sets the CheckedStateImageSource property. This dependency property 
        /// indicates the ImageSource for the normal state.
        /// </summary>
        public ImageSource CheckedStateImageSource
        {
            get { return (ImageSource)GetValue(CheckedStateImageSourceProperty); }
            set { SetValue(CheckedStateImageSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CheckedStateImageSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCheckedStateImageSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageToggleButton)d;
            ImageSource oldCheckedStateImageSource = (ImageSource)e.OldValue;
            ImageSource newCheckedStateImageSource = target.CheckedStateImageSource;
            target.OnCheckedStateImageSourceChanged(oldCheckedStateImageSource, newCheckedStateImageSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CheckedStateImageSource property.
        /// </summary>
        /// <param name="oldCheckedStateImageSource">The old CheckedStateImageSource value</param>
        /// <param name="newCheckedStateImageSource">The new CheckedStateImageSource value</param>
        protected virtual void OnCheckedStateImageSourceChanged(
            ImageSource oldCheckedStateImageSource, ImageSource newCheckedStateImageSource)
        {
            UpdateCheckedStateImage();
            UpdateCheckedHoverStateImage();
            UpdateRecycledCheckedHoverStateImages();
            UpdateCheckedPressedStateImage();
            UpdateCheckedDisabledStateImage();
        }
        #endregion

        #region CheckedHoverStateImageSource
        /// <summary>
        /// CheckedHoverStateImageSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty CheckedHoverStateImageSourceProperty =
            DependencyProperty.Register(
                "CheckedHoverStateImageSource",
                typeof(ImageSource),
                typeof(ImageToggleButton),
                new PropertyMetadata(null, OnCheckedHoverStateImageSourceChanged));

        /// <summary>
        /// Gets or sets the CheckedHoverStateImageSource property. This dependency property 
        /// indicates the ImageSource to use when the pointer is over the ToggleButton.
        /// </summary>
        public ImageSource CheckedHoverStateImageSource
        {
            get { return (ImageSource)GetValue(CheckedHoverStateImageSourceProperty); }
            set { SetValue(CheckedHoverStateImageSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CheckedHoverStateImageSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCheckedHoverStateImageSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageToggleButton)d;
            ImageSource oldCheckedHoverStateImageSource = (ImageSource)e.OldValue;
            ImageSource newCheckedHoverStateImageSource = target.CheckedHoverStateImageSource;
            target.OnCheckedHoverStateImageSourceChanged(oldCheckedHoverStateImageSource, newCheckedHoverStateImageSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CheckedHoverStateImageSource property.
        /// </summary>
        /// <param name="oldCheckedHoverStateImageSource">The old CheckedHoverStateImageSource value</param>
        /// <param name="newCheckedHoverStateImageSource">The new CheckedHoverStateImageSource value</param>
        protected virtual void OnCheckedHoverStateImageSourceChanged(
            ImageSource oldCheckedHoverStateImageSource, ImageSource newCheckedHoverStateImageSource)
        {
            UpdateCheckedHoverStateImage();
        }
        #endregion

        #region CheckedPressedStateImageSource
        /// <summary>
        /// CheckedPressedStateImageSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty CheckedPressedStateImageSourceProperty =
            DependencyProperty.Register(
                "CheckedPressedStateImageSource",
                typeof(ImageSource),
                typeof(ImageToggleButton),
                new PropertyMetadata(null, OnCheckedPressedStateImageSourceChanged));

        /// <summary>
        /// Gets or sets the CheckedPressedStateImageSource property. This dependency property 
        /// indicates the ImageSource to use when the ToggleButton is pressed.
        /// </summary>
        public ImageSource CheckedPressedStateImageSource
        {
            get { return (ImageSource)GetValue(CheckedPressedStateImageSourceProperty); }
            set { SetValue(CheckedPressedStateImageSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CheckedPressedStateImageSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCheckedPressedStateImageSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageToggleButton)d;
            ImageSource oldCheckedPressedStateImageSource = (ImageSource)e.OldValue;
            ImageSource newCheckedPressedStateImageSource = target.CheckedPressedStateImageSource;
            target.OnCheckedPressedStateImageSourceChanged(oldCheckedPressedStateImageSource, newCheckedPressedStateImageSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CheckedPressedStateImageSource property.
        /// </summary>
        /// <param name="oldCheckedPressedStateImageSource">The old CheckedPressedStateImageSource value</param>
        /// <param name="newCheckedPressedStateImageSource">The new CheckedPressedStateImageSource value</param>
        protected virtual void OnCheckedPressedStateImageSourceChanged(
            ImageSource oldCheckedPressedStateImageSource, ImageSource newCheckedPressedStateImageSource)
        {
            UpdateCheckedPressedStateImage();
            UpdateRecycledCheckedHoverStateImages();
        }
        #endregion

        #region CheckedDisabledStateImageSource
        /// <summary>
        /// CheckedDisabledStateImageSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty CheckedDisabledStateImageSourceProperty =
            DependencyProperty.Register(
                "CheckedDisabledStateImageSource",
                typeof(ImageSource),
                typeof(ImageToggleButton),
                new PropertyMetadata(null, OnCheckedDisabledStateImageSourceChanged));

        /// <summary>
        /// Gets or sets the CheckedDisabledStateImageSource property. This dependency property 
        /// indicates the ImageSource to use when the ToggleButton is Disabled.
        /// </summary>
        public ImageSource CheckedDisabledStateImageSource
        {
            get { return (ImageSource)GetValue(CheckedDisabledStateImageSourceProperty); }
            set { SetValue(CheckedDisabledStateImageSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CheckedDisabledStateImageSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCheckedDisabledStateImageSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageToggleButton)d;
            ImageSource oldCheckedDisabledStateImageSource = (ImageSource)e.OldValue;
            ImageSource newCheckedDisabledStateImageSource = target.CheckedDisabledStateImageSource;
            target.OnCheckedDisabledStateImageSourceChanged(oldCheckedDisabledStateImageSource, newCheckedDisabledStateImageSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CheckedDisabledStateImageSource property.
        /// </summary>
        /// <param name="oldCheckedDisabledStateImageSource">The old CheckedDisabledStateImageSource value</param>
        /// <param name="newCheckedDisabledStateImageSource">The new CheckedDisabledStateImageSource value</param>
        protected virtual void OnCheckedDisabledStateImageSourceChanged(
            ImageSource oldCheckedDisabledStateImageSource, ImageSource newCheckedDisabledStateImageSource)
        {
            UpdateCheckedDisabledStateImage();
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
                typeof(ImageToggleButton),
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
            var target = (ImageToggleButton)d;
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
                typeof(ImageToggleButton),
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
            var target = (ImageToggleButton)d;
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
                typeof(ImageToggleButton),
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
            var target = (ImageToggleButton)d;
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
                typeof(ImageToggleButton),
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
            var target = (ImageToggleButton)d;
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

        #region CheckedStateImageUriSource
        /// <summary>
        /// CheckedStateImageUriSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty CheckedStateImageUriSourceProperty =
            DependencyProperty.Register(
                "CheckedStateImageUriSource",
                typeof(Uri),
                typeof(ImageToggleButton),
                new PropertyMetadata(null, OnCheckedStateImageUriSourceChanged));

        /// <summary>
        /// Gets or sets the CheckedStateImageUriSource property. This dependency property 
        /// indicates the uri to use for the normal image source.
        /// </summary>
        public Uri CheckedStateImageUriSource
        {
            get { return (Uri)GetValue(CheckedStateImageUriSourceProperty); }
            set { SetValue(CheckedStateImageUriSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CheckedStateImageUriSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCheckedStateImageUriSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageToggleButton)d;
            Uri oldCheckedStateImageUriSource = (Uri)e.OldValue;
            Uri newCheckedStateImageUriSource = target.CheckedStateImageUriSource;
            target.OnCheckedStateImageUriSourceChanged(oldCheckedStateImageUriSource, newCheckedStateImageUriSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CheckedStateImageUriSource property.
        /// </summary>
        /// <param name="oldCheckedStateImageUriSource">The old CheckedStateImageUriSource value</param>
        /// <param name="newCheckedStateImageUriSource">The new CheckedStateImageUriSource value</param>
        private void OnCheckedStateImageUriSourceChanged(
            Uri oldCheckedStateImageUriSource, Uri newCheckedStateImageUriSource)
        {
            if (newCheckedStateImageUriSource != null)
                this.CheckedStateImageSource = new BitmapImage(newCheckedStateImageUriSource);
            else
                this.CheckedStateImageSource = null;
        }
        #endregion

        #region CheckedHoverStateImageUriSource
        /// <summary>
        /// CheckedHoverStateImageUriSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty CheckedHoverStateImageUriSourceProperty =
            DependencyProperty.Register(
                "CheckedHoverStateImageUriSource",
                typeof(Uri),
                typeof(ImageToggleButton),
                new PropertyMetadata(null, OnCheckedHoverStateImageUriSourceChanged));

        /// <summary>
        /// Gets or sets the CheckedHoverStateImageUriSource property. This dependency property 
        /// indicates the uri to use for the normal image source.
        /// </summary>
        public Uri CheckedHoverStateImageUriSource
        {
            get { return (Uri)GetValue(CheckedHoverStateImageUriSourceProperty); }
            set { SetValue(CheckedHoverStateImageUriSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CheckedHoverStateImageUriSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCheckedHoverStateImageUriSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageToggleButton)d;
            Uri oldCheckedHoverStateImageUriSource = (Uri)e.OldValue;
            Uri newCheckedHoverStateImageUriSource = target.CheckedHoverStateImageUriSource;
            target.OnCheckedHoverStateImageUriSourceChanged(oldCheckedHoverStateImageUriSource, newCheckedHoverStateImageUriSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CheckedHoverStateImageUriSource property.
        /// </summary>
        /// <param name="oldCheckedHoverStateImageUriSource">The old CheckedHoverStateImageUriSource value</param>
        /// <param name="newCheckedHoverStateImageUriSource">The new CheckedHoverStateImageUriSource value</param>
        private void OnCheckedHoverStateImageUriSourceChanged(
            Uri oldCheckedHoverStateImageUriSource, Uri newCheckedHoverStateImageUriSource)
        {
            if (newCheckedHoverStateImageUriSource != null)
                this.CheckedHoverStateImageSource = new BitmapImage(newCheckedHoverStateImageUriSource);
            else
                this.CheckedHoverStateImageSource = null;
        }
        #endregion

        #region CheckedPressedStateImageUriSource
        /// <summary>
        /// CheckedPressedStateImageUriSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty CheckedPressedStateImageUriSourceProperty =
            DependencyProperty.Register(
                "CheckedPressedStateImageUriSource",
                typeof(Uri),
                typeof(ImageToggleButton),
                new PropertyMetadata(null, OnCheckedPressedStateImageUriSourceChanged));

        /// <summary>
        /// Gets or sets the CheckedPressedStateImageUriSource property. This dependency property 
        /// indicates the uri to use for the normal image source.
        /// </summary>
        public Uri CheckedPressedStateImageUriSource
        {
            get { return (Uri)GetValue(CheckedPressedStateImageUriSourceProperty); }
            set { SetValue(CheckedPressedStateImageUriSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CheckedPressedStateImageUriSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCheckedPressedStateImageUriSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageToggleButton)d;
            Uri oldCheckedPressedStateImageUriSource = (Uri)e.OldValue;
            Uri newCheckedPressedStateImageUriSource = target.CheckedPressedStateImageUriSource;
            target.OnCheckedPressedStateImageUriSourceChanged(oldCheckedPressedStateImageUriSource, newCheckedPressedStateImageUriSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CheckedPressedStateImageUriSource property.
        /// </summary>
        /// <param name="oldCheckedPressedStateImageUriSource">The old CheckedPressedStateImageUriSource value</param>
        /// <param name="newCheckedPressedStateImageUriSource">The new CheckedPressedStateImageUriSource value</param>
        private void OnCheckedPressedStateImageUriSourceChanged(
            Uri oldCheckedPressedStateImageUriSource, Uri newCheckedPressedStateImageUriSource)
        {
            if (newCheckedPressedStateImageUriSource != null)
                this.CheckedPressedStateImageSource = new BitmapImage(newCheckedPressedStateImageUriSource);
            else
                this.CheckedPressedStateImageSource = null;
        }
        #endregion

        #region CheckedDisabledStateImageUriSource
        /// <summary>
        /// CheckedDisabledStateImageUriSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty CheckedDisabledStateImageUriSourceProperty =
            DependencyProperty.Register(
                "CheckedDisabledStateImageUriSource",
                typeof(Uri),
                typeof(ImageToggleButton),
                new PropertyMetadata(null, OnCheckedDisabledStateImageUriSourceChanged));

        /// <summary>
        /// Gets or sets the CheckedDisabledStateImageUriSource property. This dependency property 
        /// indicates the uri to use for the normal image source.
        /// </summary>
        public Uri CheckedDisabledStateImageUriSource
        {
            get { return (Uri)GetValue(CheckedDisabledStateImageUriSourceProperty); }
            set { SetValue(CheckedDisabledStateImageUriSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CheckedDisabledStateImageUriSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCheckedDisabledStateImageUriSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageToggleButton)d;
            Uri oldCheckedDisabledStateImageUriSource = (Uri)e.OldValue;
            Uri newCheckedDisabledStateImageUriSource = target.CheckedDisabledStateImageUriSource;
            target.OnCheckedDisabledStateImageUriSourceChanged(oldCheckedDisabledStateImageUriSource, newCheckedDisabledStateImageUriSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CheckedDisabledStateImageUriSource property.
        /// </summary>
        /// <param name="oldCheckedDisabledStateImageUriSource">The old CheckedDisabledStateImageUriSource value</param>
        /// <param name="newCheckedDisabledStateImageUriSource">The new CheckedDisabledStateImageUriSource value</param>
        private void OnCheckedDisabledStateImageUriSourceChanged(
            Uri oldCheckedDisabledStateImageUriSource, Uri newCheckedDisabledStateImageUriSource)
        {
            if (newCheckedDisabledStateImageUriSource != null)
                this.CheckedDisabledStateImageSource = new BitmapImage(newCheckedDisabledStateImageUriSource);
            else
                this.CheckedDisabledStateImageSource = null;
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
                typeof(ImageToggleButton),
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
            var target = (ImageToggleButton)d;
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
            UpdateCheckedStateImage();
            UpdateCheckedHoverStateImage();
            UpdateCheckedPressedStateImage();
            UpdateCheckedDisabledStateImage();
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
                typeof(ImageToggleButton),
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
            var target = (ImageToggleButton)d;
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
                typeof(ImageToggleButton),
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
            var target = (ImageToggleButton)d;
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
                typeof(ImageToggleButton),
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
            var target = (ImageToggleButton)d;
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

        #region GeneratedCheckedStateLightenAmount
        /// <summary>
        /// GeneratedCheckedStateLightenAmount Dependency Property
        /// </summary>
        public static readonly DependencyProperty GeneratedCheckedStateLightenAmountProperty =
            DependencyProperty.Register(
                "GeneratedCheckedStateLightenAmount",
                typeof(double),
                typeof(ImageToggleButton),
                new PropertyMetadata(0.5, OnGeneratedCheckedStateLightenAmountChanged));

        /// <summary>
        /// Gets or sets the GeneratedCheckedStateLightenAmount property. This dependency property 
        /// indicates the lightening amount to use when generating the checked state image.
        /// </summary>
        public double GeneratedCheckedStateLightenAmount
        {
            get { return (double)GetValue(GeneratedCheckedStateLightenAmountProperty); }
            set { SetValue(GeneratedCheckedStateLightenAmountProperty, value); }
        }

        /// <summary>
        /// Handles changes to the GeneratedCheckedStateLightenAmount property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnGeneratedCheckedStateLightenAmountChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageToggleButton)d;
            double oldGeneratedCheckedStateLightenAmount = (double)e.OldValue;
            double newGeneratedCheckedStateLightenAmount = target.GeneratedCheckedStateLightenAmount;
            target.OnGeneratedCheckedStateLightenAmountChanged(oldGeneratedCheckedStateLightenAmount, newGeneratedCheckedStateLightenAmount);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the GeneratedCheckedStateLightenAmount property.
        /// </summary>
        /// <param name="oldGeneratedCheckedStateLightenAmount">The old GeneratedCheckedStateLightenAmount value</param>
        /// <param name="newGeneratedCheckedStateLightenAmount">The new GeneratedCheckedStateLightenAmount value</param>
        protected virtual void OnGeneratedCheckedStateLightenAmountChanged(
            double oldGeneratedCheckedStateLightenAmount, double newGeneratedCheckedStateLightenAmount)
        {
            UpdateCheckedStateImage();
        }
        #endregion

        #region GeneratedCheckedHoverStateLightenAmount
        /// <summary>
        /// GeneratedCheckedHoverStateLightenAmount Dependency Property
        /// </summary>
        public static readonly DependencyProperty GeneratedCheckedHoverStateLightenAmountProperty =
            DependencyProperty.Register(
                "GeneratedCheckedHoverStateLightenAmount",
                typeof(double),
                typeof(ImageToggleButton),
                new PropertyMetadata(0.65, OnGeneratedCheckedHoverStateLightenAmountChanged));

        /// <summary>
        /// Gets or sets the GeneratedCheckedHoverStateLightenAmount property. This dependency property 
        /// indicates the lightening amount to use when generating the checked hover state image.
        /// </summary>
        public double GeneratedCheckedHoverStateLightenAmount
        {
            get { return (double)GetValue(GeneratedCheckedHoverStateLightenAmountProperty); }
            set { SetValue(GeneratedCheckedHoverStateLightenAmountProperty, value); }
        }

        /// <summary>
        /// Handles changes to the GeneratedCheckedHoverStateLightenAmount property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnGeneratedCheckedHoverStateLightenAmountChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageToggleButton)d;
            double oldGeneratedCheckedHoverStateLightenAmount = (double)e.OldValue;
            double newGeneratedCheckedHoverStateLightenAmount = target.GeneratedCheckedHoverStateLightenAmount;
            target.OnGeneratedCheckedHoverStateLightenAmountChanged(oldGeneratedCheckedHoverStateLightenAmount, newGeneratedCheckedHoverStateLightenAmount);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the GeneratedCheckedHoverStateLightenAmount property.
        /// </summary>
        /// <param name="oldGeneratedCheckedHoverStateLightenAmount">The old GeneratedCheckedHoverStateLightenAmount value</param>
        /// <param name="newGeneratedCheckedHoverStateLightenAmount">The new GeneratedCheckedHoverStateLightenAmount value</param>
        protected virtual void OnGeneratedCheckedHoverStateLightenAmountChanged(
            double oldGeneratedCheckedHoverStateLightenAmount, double newGeneratedCheckedHoverStateLightenAmount)
        {
            UpdateCheckedHoverStateImage();
        }
        #endregion

        #region GeneratedCheckedPressedStateLightenAmount
        /// <summary>
        /// GeneratedCheckedPressedStateLightenAmount Dependency Property
        /// </summary>
        public static readonly DependencyProperty GeneratedCheckedPressedStateLightenAmountProperty =
            DependencyProperty.Register(
                "GeneratedCheckedPressedStateLightenAmount",
                typeof(double),
                typeof(ImageToggleButton),
                new PropertyMetadata(0.8, OnGeneratedCheckedPressedStateLightenAmountChanged));

        /// <summary>
        /// Gets or sets the GeneratedCheckedPressedStateLightenAmount property. This dependency property 
        /// indicates the lightening amount to use when generating the checked pressed state image.
        /// </summary>
        public double GeneratedCheckedPressedStateLightenAmount
        {
            get { return (double)GetValue(GeneratedCheckedPressedStateLightenAmountProperty); }
            set { SetValue(GeneratedCheckedPressedStateLightenAmountProperty, value); }
        }

        /// <summary>
        /// Handles changes to the GeneratedCheckedPressedStateLightenAmount property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnGeneratedCheckedPressedStateLightenAmountChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageToggleButton)d;
            double oldGeneratedCheckedPressedStateLightenAmount = (double)e.OldValue;
            double newGeneratedCheckedPressedStateLightenAmount = target.GeneratedCheckedPressedStateLightenAmount;
            target.OnGeneratedCheckedPressedStateLightenAmountChanged(oldGeneratedCheckedPressedStateLightenAmount, newGeneratedCheckedPressedStateLightenAmount);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the GeneratedCheckedPressedStateLightenAmount property.
        /// </summary>
        /// <param name="oldGeneratedCheckedPressedStateLightenAmount">The old GeneratedCheckedPressedStateLightenAmount value</param>
        /// <param name="newGeneratedCheckedPressedStateLightenAmount">The new GeneratedCheckedPressedStateLightenAmount value</param>
        protected virtual void OnGeneratedCheckedPressedStateLightenAmountChanged(
            double oldGeneratedCheckedPressedStateLightenAmount, double newGeneratedCheckedPressedStateLightenAmount)
        {
            UpdateCheckedPressedStateImage();
        }
        #endregion

        #region GeneratedCheckedDisabledStateGrayscaleAmount
        /// <summary>
        /// GeneratedCheckedDisabledStateGrayscaleAmount Dependency Property
        /// </summary>
        public static readonly DependencyProperty GeneratedCheckedDisabledStateGrayscaleAmountProperty =
            DependencyProperty.Register(
                "GeneratedCheckedDisabledStateGrayscaleAmount",
                typeof(double),
                typeof(ImageToggleButton),
                new PropertyMetadata(1.0, OnGeneratedCheckedDisabledStateGrayscaleAmountChanged));

        /// <summary>
        /// Gets or sets the GeneratedCheckedDisabledStateGrayscaleAmount property. This dependency property 
        /// indicates the grayscale amount to use when generating the checked disabled state image.
        /// </summary>
        public double GeneratedCheckedDisabledStateGrayscaleAmount
        {
            get { return (double)GetValue(GeneratedCheckedDisabledStateGrayscaleAmountProperty); }
            set { SetValue(GeneratedCheckedDisabledStateGrayscaleAmountProperty, value); }
        }

        /// <summary>
        /// Handles changes to the GeneratedCheckedDisabledStateGrayscaleAmount property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnGeneratedCheckedDisabledStateGrayscaleAmountChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageToggleButton)d;
            double oldGeneratedCheckedDisabledStateGrayscaleAmount = (double)e.OldValue;
            double newGeneratedCheckedDisabledStateGrayscaleAmount = target.GeneratedCheckedDisabledStateGrayscaleAmount;
            target.OnGeneratedCheckedDisabledStateGrayscaleAmountChanged(oldGeneratedCheckedDisabledStateGrayscaleAmount, newGeneratedCheckedDisabledStateGrayscaleAmount);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the GeneratedCheckedDisabledStateGrayscaleAmount property.
        /// </summary>
        /// <param name="oldGeneratedCheckedDisabledStateGrayscaleAmount">The old GeneratedCheckedDisabledStateGrayscaleAmount value</param>
        /// <param name="newGeneratedCheckedDisabledStateGrayscaleAmount">The new GeneratedCheckedDisabledStateGrayscaleAmount value</param>
        protected virtual void OnGeneratedCheckedDisabledStateGrayscaleAmountChanged(
            double oldGeneratedCheckedDisabledStateGrayscaleAmount, double newGeneratedCheckedDisabledStateGrayscaleAmount)
        {
            UpdateCheckedDisabledStateImage();
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

        #region GenerateCheckedStateImage()
        private async void GenerateCheckedStateImage()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            var wb = new WriteableBitmap(1, 1);
            await wb.FromBitmapImage((BitmapImage)NormalStateImageSource);
            await wb.WaitForLoadedAsync();
            wb.Lighten(GeneratedCheckedStateLightenAmount);
            _checkedStateImage.Source = wb;
        }
        #endregion

        #region GenerateCheckedHoverStateImage()
        private async void GenerateCheckedHoverStateImage()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            var wb = new WriteableBitmap(1, 1);

            if (CheckedStateImageSource != null)
            {
                await wb.FromBitmapImage((BitmapImage)CheckedStateImageSource);
            }
            else if (NormalStateImageSource != null)
            {
                await wb.FromBitmapImage((BitmapImage)NormalStateImageSource);
            }

            await wb.WaitForLoadedAsync();
            wb.Lighten(GeneratedCheckedHoverStateLightenAmount);
            _checkedHoverStateImage.Source = wb;
        } 
        #endregion

        #region GenerateCheckedPressedStateImage()
        private async void GenerateCheckedPressedStateImage()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            var wb = new WriteableBitmap(1, 1);

            if (CheckedStateImageSource != null)
            {
                await wb.FromBitmapImage((BitmapImage)CheckedStateImageSource);
            }
            else if (NormalStateImageSource != null)
            {
                await wb.FromBitmapImage((BitmapImage)NormalStateImageSource);
            }

            await wb.WaitForLoadedAsync();
            wb.Lighten(GeneratedCheckedPressedStateLightenAmount);
            _checkedPressedStateImage.Source = wb;
        } 
        #endregion

        #region GenerateCheckedDisabledStateImage()
        private async void GenerateCheckedDisabledStateImage()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            var wb = new WriteableBitmap(1, 1);

            if (CheckedStateImageSource != null)
            {
                await wb.FromBitmapImage((BitmapImage)CheckedStateImageSource);
                await wb.WaitForLoadedAsync();
                wb.Grayscale(GeneratedCheckedDisabledStateGrayscaleAmount);
            }
            else if (PressedStateImageSource != null)
            {
                await wb.FromBitmapImage((BitmapImage)PressedStateImageSource);
                await wb.WaitForLoadedAsync();
                wb.Grayscale(GeneratedCheckedDisabledStateGrayscaleAmount);
            }
            else if (NormalStateImageSource != null)
            {
                await wb.FromBitmapImage((BitmapImage)NormalStateImageSource);
                await wb.WaitForLoadedAsync();
                wb.Grayscale(GeneratedCheckedDisabledStateGrayscaleAmount);
            }

            _checkedDisabledStateImage.Source = wb;
        } 
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageToggleButton" /> class.
        /// </summary>
        public ImageToggleButton()
        {
            DefaultStyleKey = typeof (ImageToggleButton);
        }

        #region OnApplyTemplate()
        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _normalStateImage = GetTemplateChild(NormalStateImageName) as Image;
            _hoverStateImage = GetTemplateChild(HoverStateImageName) as Image;
            _hoverStateRecycledCheckedStateImage = GetTemplateChild(HoverStateRecycledNormalStateImageName) as Image;
            _hoverStateRecycledCheckedPressedStateImage = GetTemplateChild(HoverStateRecycledPressedStateImageName) as Image;
            _pressedStateImage = GetTemplateChild(PressedStateImageName) as Image;
            _disabledStateImage = GetTemplateChild(DisabledStateImageName) as Image;
            _checkedStateImage = GetTemplateChild(CheckedStateImageName) as Image;
            _checkedHoverStateImage = GetTemplateChild(CheckedHoverStateImageName) as Image;
            _checkedHoverStateRecycledCheckedStateImage = GetTemplateChild(CheckedHoverStateRecycledCheckedStateImageName) as Image;
            _checkedHoverStateRecycledCheckedPressedStateImage = GetTemplateChild(CheckedHoverStateRecycledCheckedPressedStateImageName) as Image;
            _checkedPressedStateImage = GetTemplateChild(CheckedPressedStateImageName) as Image;
            _checkedDisabledStateImage = GetTemplateChild(CheckedDisabledStateImageName) as Image;

            UpdateNormalStateImage();
            UpdateHoverStateImage();
            UpdateRecycledHoverStateImages();
            UpdatePressedStateImage();
            UpdateDisabledStateImage();
            UpdateCheckedStateImage();
            UpdateCheckedHoverStateImage();
            UpdateRecycledCheckedHoverStateImages();
            UpdateCheckedPressedStateImage();
            UpdateCheckedDisabledStateImage();
        }
        #endregion

        #region UpdateNormalStateImage()
        private void UpdateNormalStateImage()
        {
            if (_normalStateImage != null)
                _normalStateImage.Source = NormalStateImageSource;
        } 
        #endregion

        #region UpdateHoverStateImage()
        private void UpdateHoverStateImage()
        {
            if (_hoverStateImage == null)
                return;

            if (!GenerateMissingImages ||
                NormalStateImageSource == null)
                _hoverStateImage.Source = HoverStateImageSource;
            else
#pragma warning disable 4014
                GenerateHoverStateImage();
#pragma warning restore 4014

            // If hover state is still not set - need to use normal state at least to avoid missing image
            if (_hoverStateImage.Source == null)
                _hoverStateImage.Source = NormalStateImageSource;
        } 
        #endregion

        #region UpdateRecycledHoverStateImages()
        private void UpdateRecycledHoverStateImages()
        {
            if (_hoverStateRecycledCheckedStateImage != null)
            {
                if (RecyclePressedStateImageForHover &&
                    NormalStateImageSource != null)
                    _hoverStateRecycledCheckedStateImage.Source = NormalStateImageSource;
                else
                    _hoverStateRecycledCheckedStateImage.Source = null;
            }

            if (_hoverStateRecycledCheckedPressedStateImage != null)
            {
                if (RecyclePressedStateImageForHover &&
                    PressedStateImageSource != null)
                    _hoverStateRecycledCheckedPressedStateImage.Source = PressedStateImageSource;
                else
                    _hoverStateRecycledCheckedPressedStateImage.Source = null;
            }
        } 
        #endregion

        #region UpdatePressedStateImage()
        private void UpdatePressedStateImage()
        {
            if (_pressedStateImage != null)
            {
                if (!GenerateMissingImages ||
                    NormalStateImageSource == null)
                    _pressedStateImage.Source = PressedStateImageSource;
                else
#pragma warning disable 4014
                    GeneratePressedStateImage();
#pragma warning restore 4014
            }
        } 
        #endregion

        #region UpdateDisabledStateImage()
        private void UpdateDisabledStateImage()
        {
            if (_disabledStateImage != null)
            {
                if (!GenerateMissingImages ||
                    NormalStateImageSource == null)
                    _disabledStateImage.Source = DisabledStateImageSource;
                else
#pragma warning disable 4014
                    GenerateDisabledStateImage();
#pragma warning restore 4014
            }
        } 
        #endregion

        #region UpdateCheckedStateImage()
        private void UpdateCheckedStateImage()
        {
            if (_checkedStateImage != null)
            {
                if (CheckedStateImageSource != null)
                {
                    _checkedStateImage.Source = CheckedStateImageSource;
                }
                else if (
                    RecycleUncheckedStateImagesForCheckedStates &&
                    PressedStateImageSource != null)
                {
                    _checkedStateImage.Source = PressedStateImageSource;
                }
                else if (GenerateMissingImages)
                {
#pragma warning disable 4014
                    GenerateCheckedStateImage();
#pragma warning restore 4014
                }
            }
        }
        #endregion

        #region UpdateCheckedHoverStateImage()
        private void UpdateCheckedHoverStateImage()
        {
            if (_checkedHoverStateImage == null)
                return;

            if (CheckedHoverStateImageSource != null)
            {
                _checkedHoverStateImage.Source = CheckedHoverStateImageSource;
            }
            else if (
                RecycleUncheckedStateImagesForCheckedStates &&
                HoverStateImageSource != null)
            {
                _checkedHoverStateImage.Source = HoverStateImageSource;
            }
            else if (GenerateMissingImages)
            {
#pragma warning disable 4014
                GenerateCheckedHoverStateImage();
#pragma warning restore 4014
            }

            // If checked hover state is still not set - need to use checked state at least to avoid missing image
            if (_checkedHoverStateImage.Source == null)
                _checkedHoverStateImage.Source = CheckedStateImageSource;
        }
        #endregion

        #region UpdateRecycledCheckedHoverStateImages()
        private void UpdateRecycledCheckedHoverStateImages()
        {
            if (_checkedHoverStateRecycledCheckedStateImage != null)
            {
                if (RecyclePressedStateImageForHover)
                {
                    if (CheckedStateImageSource != null)
                    {
                        _checkedHoverStateRecycledCheckedStateImage.Source = CheckedStateImageSource;
                    }
                }
                else
                {
                    _checkedHoverStateRecycledCheckedStateImage.Source = null;
                }
            }

            if (_checkedHoverStateRecycledCheckedPressedStateImage != null)
            {
                if (RecyclePressedStateImageForHover)
                {
                    if (CheckedPressedStateImageSource != null)
                    {
                        _checkedHoverStateRecycledCheckedPressedStateImage.Source = CheckedPressedStateImageSource;
                    }
                    else
                    {
                        _checkedHoverStateRecycledCheckedPressedStateImage.Source = null;
                    }
                }
            }
        }
        #endregion

        #region UpdateCheckedPressedStateImage()
        private void UpdateCheckedPressedStateImage()
        {
            if (_checkedPressedStateImage == null)
                return;

            if (CheckedPressedStateImageSource != null)
            {
                _checkedPressedStateImage.Source = CheckedPressedStateImageSource;
            }
            else if (
                RecycleUncheckedStateImagesForCheckedStates &&
                PressedStateImageSource != null)
            {
                _checkedPressedStateImage.Source = PressedStateImageSource;
            }
            else if (GenerateMissingImages)
            {
#pragma warning disable 4014
                GenerateCheckedPressedStateImage();
#pragma warning restore 4014
            }
        }
        #endregion

        #region UpdateCheckedDisabledStateImage()
        private void UpdateCheckedDisabledStateImage()
        {
            if (_disabledStateImage == null)
                return;

            if (CheckedDisabledStateImageSource != null)
            {
                _checkedDisabledStateImage.Source = CheckedDisabledStateImageSource;
            }
            else if (
                RecycleUncheckedStateImagesForCheckedStates &&
                DisabledStateImageSource != null)
            {
                _checkedDisabledStateImage.Source = DisabledStateImageSource;
            }
            else if (GenerateMissingImages)
            {
#pragma warning disable 4014
                GenerateCheckedDisabledStateImage();
#pragma warning restore 4014
            }
        }
        #endregion
    }
}
