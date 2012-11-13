//#define CascadingTextBlock_REPEATFOREVER
using System;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// A TextBlock-like control with unique 3D transitions.
    /// </summary>
    public partial class CascadingTextBlock : UserControl
    {
        #region AnimateOnLoaded
        /// <summary>
        /// AnimateOnLoaded Dependency Property
        /// </summary>
        public static readonly DependencyProperty AnimateOnLoadedProperty =
            DependencyProperty.Register(
                "AnimateOnLoaded",
                typeof(bool),
                typeof(CascadingTextBlock),
                new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the AnimateOnLoaded property. This dependency property 
        /// indicates whether the cascading transitions should start when the control loads.
        /// </summary>
        public bool AnimateOnLoaded
        {
            get { return (bool)GetValue(AnimateOnLoadedProperty); }
            set { SetValue(AnimateOnLoadedProperty, value); }
        }
        #endregion
        
        #region Text
        /// <summary>
        /// Text Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(CascadingTextBlock),
                new PropertyMetadata("", OnTextChanged));

        /// <summary>
        /// Gets or sets the Text property. This dependency property 
        /// indicates the Text to display.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Text property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnTextChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CascadingTextBlock)d;
            string oldText = (string)e.OldValue;
            string newText = target.Text;
            target.OnTextChanged(oldText, newText);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Text property.
        /// </summary>
        /// <param name="oldText">The old Text value</param>
        /// <param name="newText">The new Text value</param>
        private void OnTextChanged(
            string oldText, string newText)
        {
        }
        #endregion

        #region TextBlockTemplate
        /// <summary>
        /// TextBlockTemplate Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextBlockTemplateProperty =
            DependencyProperty.Register(
                "TextBlockTemplate",
                typeof(DataTemplate),
                typeof(CascadingTextBlock),
                new PropertyMetadata((DataTemplate)null));

        /// <summary>
        /// Gets or sets the TextBlockTemplate property. This dependency property 
        /// indicates the template of a TextBlock to use for the animated letter TextBlocks.
        /// </summary>
        public DataTemplate TextBlockTemplate
        {
            get { return (DataTemplate)GetValue(TextBlockTemplateProperty); }
            set { SetValue(TextBlockTemplateProperty, value); }
        }
        #endregion

        #region StartDelay
        /// <summary>
        /// StartDelay Dependency Property
        /// </summary>
        public static readonly DependencyProperty StartDelayProperty =
            DependencyProperty.Register(
                "StartDelay",
                typeof(int),
                typeof(CascadingTextBlock),
                new PropertyMetadata((int)0));

        /// <summary>
        /// Gets or sets the StartDelay property. This dependency property 
        /// indicates the delay in ms after which the cascading animation should start.
        /// </summary>
        public int StartDelay
        {
            get { return (int)GetValue(StartDelayProperty); }
            set { SetValue(StartDelayProperty, value); }
        }
        #endregion

        #region CascadeIn
        /// <summary>
        /// CascadeIn Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeInProperty =
            DependencyProperty.Register(
                "CascadeIn",
                typeof(bool),
                typeof(CascadingTextBlock),
                new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the CascadeIn property. This dependency property 
        /// indicates whether the letters should cascade in.
        /// </summary>
        public bool CascadeIn
        {
            get { return (bool)GetValue(CascadeInProperty); }
            set { SetValue(CascadeInProperty, value); }
        }
        #endregion

        #region CascadeOut
        /// <summary>
        /// CascadeOut Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeOutProperty =
            DependencyProperty.Register(
                "CascadeOut",
                typeof(bool),
                typeof(CascadingTextBlock),
                new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the CascadeOut property. This dependency property 
        /// indicates whether the letters should cascade out.
        /// </summary>
        public bool CascadeOut
        {
            get { return (bool)GetValue(CascadeOutProperty); }
            set { SetValue(CascadeOutProperty, value); }
        }
        #endregion

        #region HoldDuration
        /// <summary>
        /// HoldDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty HoldDurationProperty =
            DependencyProperty.Register(
                "HoldDuration",
                typeof(TimeSpan),
                typeof(CascadingTextBlock),
                new PropertyMetadata(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// Gets or sets the HoldDuration property. This dependency property 
        /// indicates the duration that the letters should stay between cascades.
        /// </summary>
        public TimeSpan HoldDuration
        {
            get { return (TimeSpan)GetValue(HoldDurationProperty); }
            set { SetValue(HoldDurationProperty, value); }
        }
        #endregion

        #region HoldDurationString
        /// <summary>
        /// HoldDurationString Dependency Property
        /// </summary>
        public static readonly DependencyProperty HoldDurationStringProperty =
            DependencyProperty.Register(
                "HoldDurationString",
                typeof(string),
                typeof(CascadingTextBlock),
                new PropertyMetadata((string)"0:0:3", new PropertyChangedCallback(OnHoldDurationStringChanged)));

        /// <summary>
        /// Gets or sets the HoldDurationString property. This dependency property 
        /// indicates the interval of the cascade out animation as a string.
        /// </summary>
        public string HoldDurationString
        {
            get { return (string)GetValue(HoldDurationStringProperty); }
            set { SetValue(HoldDurationStringProperty, value); }
        }

        /// <summary>
        /// Handles changes to the HoldDurationString property.
        /// </summary>
        private static void OnHoldDurationStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CascadingTextBlock target = (CascadingTextBlock)d;
            string oldHoldDurationString = (string)e.OldValue;
            string newHoldDurationString = target.HoldDurationString;
            target.OnHoldDurationStringChanged(oldHoldDurationString, newHoldDurationString);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the HoldDurationString property.
        /// </summary>
        protected virtual void OnHoldDurationStringChanged(string oldHoldDurationString, string newHoldDurationString)
        {
            HoldDuration = TimeSpan.Parse(newHoldDurationString);
        }
        #endregion

        #region CascadeInDuration
        /// <summary>
        /// CascadeInDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeInDurationProperty =
            DependencyProperty.Register(
                "CascadeInDuration",
                typeof(TimeSpan),
                typeof(CascadingTextBlock),
                new PropertyMetadata(TimeSpan.FromSeconds(1)));

        /// <summary>
        /// Gets or sets the CascadeInDuration property. This dependency property 
        /// indicates the duration that each of the letters should spend cascading in.
        /// </summary>
        public TimeSpan CascadeInDuration
        {
            get { return (TimeSpan)GetValue(CascadeInDurationProperty); }
            set { SetValue(CascadeInDurationProperty, value); }
        }
        #endregion

        #region CascadeInDurationString
        /// <summary>
        /// CascadeInDurationString Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeInDurationStringProperty =
            DependencyProperty.Register(
                "CascadeInDurationString",
                typeof(string),
                typeof(CascadingTextBlock),
                new PropertyMetadata((string)"0:0:1", new PropertyChangedCallback(OnCascadeInDurationStringChanged)));

        /// <summary>
        /// Gets or sets the CascadeInDurationString property. This dependency property 
        /// indicates the duration of the cascade in animation as a string.
        /// </summary>
        public string CascadeInDurationString
        {
            get { return (string)GetValue(CascadeInDurationStringProperty); }
            set { SetValue(CascadeInDurationStringProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CascadeInDurationString property.
        /// </summary>
        private static void OnCascadeInDurationStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CascadingTextBlock target = (CascadingTextBlock)d;
            string oldCascadeInDurationString = (string)e.OldValue;
            string newCascadeInDurationString = target.CascadeInDurationString;
            target.OnCascadeInDurationStringChanged(oldCascadeInDurationString, newCascadeInDurationString);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CascadeInDurationString property.
        /// </summary>
        protected virtual void OnCascadeInDurationStringChanged(string oldCascadeInDurationString, string newCascadeInDurationString)
        {
            CascadeInDuration = TimeSpan.Parse(newCascadeInDurationString);
        }
        #endregion

        #region CascadeOutDuration
        /// <summary>
        /// CascadeOutDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeOutDurationProperty =
            DependencyProperty.Register(
                "CascadeOutDuration",
                typeof(TimeSpan),
                typeof(CascadingTextBlock),
                new PropertyMetadata(TimeSpan.FromSeconds(1)));

        /// <summary>
        /// Gets or sets the CascadeOutDuration property. This dependency property 
        /// indicates the duration that each of the letters should spend cascading out.
        /// </summary>
        public TimeSpan CascadeOutDuration
        {
            get { return (TimeSpan)GetValue(CascadeOutDurationProperty); }
            set { SetValue(CascadeOutDurationProperty, value); }
        }
        #endregion

        #region CascadeOutDurationString
        /// <summary>
        /// CascadeOutDurationString Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeOutDurationStringProperty =
            DependencyProperty.Register(
                "CascadeOutDurationString",
                typeof(string),
                typeof(CascadingTextBlock),
                new PropertyMetadata((string)"0:0:1", new PropertyChangedCallback(OnCascadeOutDurationStringChanged)));

        /// <summary>
        /// Gets or sets the CascadeOutDurationString property. This dependency property 
        /// indicates the duration of the cascade out animation as a string.
        /// </summary>
        public string CascadeOutDurationString
        {
            get { return (string)GetValue(CascadeOutDurationStringProperty); }
            set { SetValue(CascadeOutDurationStringProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CascadeOutDurationString property.
        /// </summary>
        private static void OnCascadeOutDurationStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CascadingTextBlock target = (CascadingTextBlock)d;
            string oldCascadeOutDurationString = (string)e.OldValue;
            string newCascadeOutDurationString = target.CascadeOutDurationString;
            target.OnCascadeOutDurationStringChanged(oldCascadeOutDurationString, newCascadeOutDurationString);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CascadeOutDurationString property.
        /// </summary>
        protected virtual void OnCascadeOutDurationStringChanged(string oldCascadeOutDurationString, string newCascadeOutDurationString)
        {
            CascadeOutDuration = TimeSpan.Parse(newCascadeOutDurationString);
        }
        #endregion
        
        #region CascadeInterval
        /// <summary>
        /// CascadeInterval Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeIntervalProperty =
            DependencyProperty.Register(
                "CascadeInterval",
                typeof(TimeSpan),
                typeof(CascadingTextBlock),
                new PropertyMetadata(TimeSpan.FromMilliseconds(100)));

        /// <summary>
        /// Gets or sets the CascadeInterval property. This dependency property 
        /// indicates the interval between when each letter starts cascading.
        /// </summary>
        public TimeSpan CascadeInterval
        {
            get { return (TimeSpan)GetValue(CascadeIntervalProperty); }
            set { SetValue(CascadeIntervalProperty, value); }
        }
        #endregion

        #region CascadeIntervalString
        /// <summary>
        /// CascadeIntervalString Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeIntervalStringProperty =
            DependencyProperty.Register(
                "CascadeIntervalString",
                typeof(string),
                typeof(CascadingTextBlock),
                new PropertyMetadata((string)"0:0:0.1", new PropertyChangedCallback(OnCascadeIntervalStringChanged)));

        /// <summary>
        /// Gets or sets the CascadeIntervalString property. This dependency property 
        /// indicates the interval of the cascade out animation as a string.
        /// </summary>
        public string CascadeIntervalString
        {
            get { return (string)GetValue(CascadeIntervalStringProperty); }
            set { SetValue(CascadeIntervalStringProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CascadeIntervalString property.
        /// </summary>
        private static void OnCascadeIntervalStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CascadingTextBlock target = (CascadingTextBlock)d;
            string oldCascadeIntervalString = (string)e.OldValue;
            string newCascadeIntervalString = target.CascadeIntervalString;
            target.OnCascadeIntervalStringChanged(oldCascadeIntervalString, newCascadeIntervalString);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CascadeIntervalString property.
        /// </summary>
        protected virtual void OnCascadeIntervalStringChanged(string oldCascadeIntervalString, string newCascadeIntervalString)
        {
            CascadeInterval = TimeSpan.Parse(newCascadeIntervalString);
        }
        #endregion

        #region CascadeInEasingFunction
        /// <summary>
        /// CascadeInEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeInEasingFunctionProperty =
            DependencyProperty.Register(
                "CascadeInEasingFunction",
                typeof(EasingFunctionBase),
                typeof(CascadingTextBlock),
                new PropertyMetadata(new PowerEase { EasingMode = EasingMode.EaseOut, Power = 2 }));

        /// <summary>
        /// Gets or sets the CascadeInEasingFunction property. This dependency property 
        /// indicates the easing function to use in the cascade in transition.
        /// </summary>
        public EasingFunctionBase CascadeInEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(CascadeInEasingFunctionProperty); }
            set { SetValue(CascadeInEasingFunctionProperty, value); }
        }
        #endregion

        #region CascadeOutEasingFunction
        /// <summary>
        /// CascadeOutEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeOutEasingFunctionProperty =
            DependencyProperty.Register(
                "CascadeOutEasingFunction",
                typeof(EasingFunctionBase),
                typeof(CascadingTextBlock),
                new PropertyMetadata(new PowerEase { EasingMode = EasingMode.EaseIn, Power = 2 }));

        /// <summary>
        /// Gets or sets the CascadeOutEasingFunction property. This dependency property 
        /// indicates the easing function to use in the cascade out transition.
        /// </summary>
        public EasingFunctionBase CascadeOutEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(CascadeOutEasingFunctionProperty); }
            set { SetValue(CascadeOutEasingFunctionProperty, value); }
        }
        #endregion

        #region FromVerticalOffset
        /// <summary>
        /// FromVerticalOffset Dependency Property
        /// </summary>
        public static readonly DependencyProperty FromVerticalOffsetProperty =
            DependencyProperty.Register(
                "FromVerticalOffset",
                typeof(double),
                typeof(CascadingTextBlock),
                new PropertyMetadata(-200.0));

        /// <summary>
        /// Gets or sets the FromVerticalOffset property. This dependency property 
        /// indicates the offset from which the letters cascade in.
        /// </summary>
        public double FromVerticalOffset
        {
            get { return (double)GetValue(FromVerticalOffsetProperty); }
            set { SetValue(FromVerticalOffsetProperty, value); }
        }
        #endregion

        #region ToVerticalOffset
        /// <summary>
        /// ToVerticalOffset Dependency Property
        /// </summary>
        public static readonly DependencyProperty ToVerticalOffsetProperty =
            DependencyProperty.Register(
                "ToVerticalOffset",
                typeof(double),
                typeof(CascadingTextBlock),
                new PropertyMetadata(200.0));

        /// <summary>
        /// Gets or sets the ToVerticalOffset property. This dependency property 
        /// indicates the offset to which the letters cascade out.
        /// </summary>
        public double ToVerticalOffset
        {
            get { return (double)GetValue(ToVerticalOffsetProperty); }
            set { SetValue(ToVerticalOffsetProperty, value); }
        }
        #endregion

        #region FromRotation
        /// <summary>
        /// FromRotation Dependency Property
        /// </summary>
        public static readonly DependencyProperty FromRotationProperty =
            DependencyProperty.Register(
                "FromRotation",
                typeof(double),
                typeof(CascadingTextBlock),
                new PropertyMetadata(-90.0));

        /// <summary>
        /// Gets or sets the FromRotation property. This dependency property 
        /// indicates the rotation from which the letters will cascade in.
        /// </summary>
        public double FromRotation
        {
            get { return (double)GetValue(FromRotationProperty); }
            set { SetValue(FromRotationProperty, value); }
        }
        #endregion

        #region ToRotation
        /// <summary>
        /// ToRotation Dependency Property
        /// </summary>
        public static readonly DependencyProperty ToRotationProperty =
            DependencyProperty.Register(
                "ToRotation",
                typeof(double),
                typeof(CascadingTextBlock),
                new PropertyMetadata(90.0));

        /// <summary>
        /// Gets or sets the ToRotation property. This dependency property 
        /// indicates the rotation to which the letters cascade out.
        /// </summary>
        public double ToRotation
        {
            get { return (double)GetValue(ToRotationProperty); }
            set { SetValue(ToRotationProperty, value); }
        }
        #endregion

        #region UseFade
        /// <summary>
        /// UseFade Dependency Property
        /// </summary>
        public static readonly DependencyProperty UseFadeProperty =
            DependencyProperty.Register(
                "UseFade",
                typeof(bool),
                typeof(CascadingTextBlock),
                new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the UseFade property. This dependency property 
        /// indicates whether the letters should fade in and out during cascading transitions.
        /// </summary>
        public bool UseFade
        {
            get { return (bool)GetValue(UseFadeProperty); }
            set { SetValue(UseFadeProperty, value); }
        }
        #endregion

        #region UseRotation
        /// <summary>
        /// UseRotation Dependency Property
        /// </summary>
        public static readonly DependencyProperty UseRotationProperty =
            DependencyProperty.Register(
                "UseRotation",
                typeof(bool),
                typeof(CascadingTextBlock),
                new PropertyMetadata((bool)true));

        /// <summary>
        /// Gets or sets the UseRotation property. This dependency property 
        /// indicates whether the letters should rotate around Y axis when cascading.
        /// </summary>
        public bool UseRotation
        {
            get { return (bool)GetValue(UseRotationProperty); }
            set { SetValue(UseRotationProperty, value); }
        }
        #endregion

        #region FadeInEasingFunction
        /// <summary>
        /// FadeInEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty FadeInEasingFunctionProperty =
            DependencyProperty.Register(
                "FadeInEasingFunction",
                typeof(EasingFunctionBase),
                typeof(CascadingTextBlock),
                new PropertyMetadata(new PowerEase { EasingMode = EasingMode.EaseIn, Power = 1.5 }));

        /// <summary>
        /// Gets or sets the FadeInEasingFunction property. This dependency property 
        /// indicates the easing function to use for fading in the cascade in transition.
        /// </summary>
        public EasingFunctionBase FadeInEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(FadeInEasingFunctionProperty); }
            set { SetValue(FadeInEasingFunctionProperty, value); }
        }
        #endregion

        #region FadeOutEasingFunction
        /// <summary>
        /// FadeOutEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty FadeOutEasingFunctionProperty =
            DependencyProperty.Register(
                "FadeOutEasingFunction",
                typeof(EasingFunctionBase),
                typeof(CascadingTextBlock),
                new PropertyMetadata(new PowerEase { EasingMode = EasingMode.EaseOut, Power = 1.5 }));

        /// <summary>
        /// Gets or sets the FadeOutEasingFunction property. This dependency property 
        /// indicates the easing function to use for fading out the cascade out transition.
        /// </summary>
        public EasingFunctionBase FadeOutEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(FadeOutEasingFunctionProperty); }
            set { SetValue(FadeOutEasingFunctionProperty, value); }
        }
        #endregion

        /// <summary>
        /// Occurs when cascade animation completes.
        /// </summary>
        public event EventHandler CascadeCompleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="CascadingTextBlock" /> class.
        /// </summary>
        public CascadingTextBlock()
        {
            InitializeComponent();
            this.Loaded += this.OnCascadingTextBlockLoaded;
        }

        private void OnCascadingTextBlockLoaded(object sender, RoutedEventArgs e)
        {
#pragma warning disable 4014
            if (AnimateOnLoaded)
                BeginCascadingTransitionAsync();
#pragma warning restore 4014
        }

        /// <summary>
        /// Begins the cascading transition asynchronously (waits for it to complete).
        /// </summary>
        /// <returns></returns>
        public async Task BeginCascadingTransitionAsync()
        {
            var transparentBrush =
                new SolidColorBrush(Colors.Transparent);
            LayoutRoot.Children.Clear();

            var totalDelay = TimeSpan.FromSeconds(0);

            var cascadeStoryboard = new Storyboard();

            var previousCharacterRect = new Rect(-100000,0,0,0);

            for (int i = 0; i < Text.Length; )
            {
                int j = 1;

                while (
                    i + j < Text.Length &&
                    Text[i + j] == ' ')
                {
                    j++;
                }

                var tt = new TranslateTransform();

                if (CascadeIn)
                {
                    tt.Y = FromVerticalOffset;
                }

                TextBlock tb = CreateTextBlock(tt);

                if (i > 0)
                {
                    tb.Inlines.Add(
                        new Run
                        {
                            Text = Text.Substring(0, i),
                            Foreground = transparentBrush
                        });
                }
                
                var singleLetterRun = new Run { Text = Text.Substring(i, j) };
                tb.Inlines.Add(singleLetterRun);
                //.GetPositionAtOffset(1, LogicalDirection.Backward)

                if (i + j < Text.Length)
                {
                    tb.Inlines.Add(
                        new Run
                        {
                            Text = Text.Substring(i + j),
                            Foreground = transparentBrush
                        });
                }

                LayoutRoot.Children.Add(tb);

                DoubleAnimationUsingKeyFrames opacityAnimation = null;

                if (UseFade)
                {
                    opacityAnimation = new DoubleAnimationUsingKeyFrames();

                    if (CascadeIn)
                        tb.Opacity = 0;

                    Storyboard.SetTarget(opacityAnimation, tb);
                    Storyboard.SetTargetProperty(opacityAnimation, "UIElement.Opacity");
                    cascadeStoryboard.Children.Add(opacityAnimation);
                }

                DoubleAnimationUsingKeyFrames yAnimation = null;

                if (CascadeIn || CascadeOut)
                {
                    yAnimation = new DoubleAnimationUsingKeyFrames();
                    Storyboard.SetTarget(yAnimation, tt);
                    Storyboard.SetTargetProperty(yAnimation, "TranslateTransform.Y");
                    cascadeStoryboard.Children.Add(yAnimation);
                }

                DoubleAnimationUsingKeyFrames rotationAnimation = null;
                PlaneProjection planeProjection = null;

                if (UseRotation)
                {
                    await tb.WaitForNonZeroSizeAsync();
                    //await Task.Delay(100);

                    var aw = tb.ActualWidth;
                    var ah = tb.ActualHeight;

                    var characterRect = tb.GetCharacterRect(i);
                    tb.Projection = planeProjection = new PlaneProjection();
                    planeProjection.CenterOfRotationX = (characterRect.X + (characterRect.Width / 2)) / aw;
                    
                    if (CascadeIn)
                        planeProjection.RotationY = FromRotation;

                    //var pointer = tb.ContentStart.GetPositionAtOffset(offset, LogicalDirection.Forward);
                    //var rect = pointer.GetCharacterRect(LogicalDirection.Forward);

                    //while (
                    //    rect == previousCharacterRect ||
                    //    rect.X - previousCharacterRect.X < 4)
                    //{
                    //    offset++;
                    //    if (offset > tb.ContentEnd.Offset)
                    //        break;
                    //    pointer = tb.ContentStart.GetPositionAtOffset(offset, LogicalDirection.Forward);
                    //    rect = pointer.GetCharacterRect(LogicalDirection.Forward);
                    //}

                    //previousCharacterRect = rect;

                    //var x = rect.X;
                    //var y = rect.Y;
                    //var w = rect.Width;
                    //var h = rect.Height;

                    //tb.Projection = planeProjection = new PlaneProjection();
                    //planeProjection.CenterOfRotationX = (x + (w / 2)) / aw;
                    //planeProjection.RotationY = FromRotation;

                    //if (!headerPrinted)
                    //{
                    //    Debug.WriteLine("ActualWidth: {0}", aw);
                    //    Debug.WriteLine("ActualHeight: {0}\r\n", ah);
                    //    Debug.WriteLine("po\ti\tj\tx\ty\tw\th\tpx");
                    //    headerPrinted = true;
                    //}

                    //Debug.WriteLine(
                    //    "{0:F0}\t{1:F0}\t{2:F0}\t{3:F0}\t{4:F0}\t{5:F0}\t{6:F0}\t{7:F3}",
                    //    pointer.Offset, i, j, x, y, w, h, planeProjection.CenterOfRotationX);

                    rotationAnimation = new DoubleAnimationUsingKeyFrames();
                    Storyboard.SetTarget(rotationAnimation, planeProjection);
                    Storyboard.SetTargetProperty(rotationAnimation, "PlaneProjection.RotationY");
                    cascadeStoryboard.Children.Add(rotationAnimation);

                    if (CascadeIn)
                    {
                        rotationAnimation.KeyFrames.Add(
                            new DiscreteDoubleKeyFrame
                            {
                                KeyTime = totalDelay,
                                Value = FromRotation
                            });
                        rotationAnimation.KeyFrames.Add(
                            new EasingDoubleKeyFrame
                            {
                                KeyTime = totalDelay + CascadeInDuration,
                                EasingFunction = CascadeInEasingFunction,
                                Value = 0
                            });
                    }

                    if (CascadeOut)
                    {
                        rotationAnimation.KeyFrames.Add(
                            new DiscreteDoubleKeyFrame
                            {
                                KeyTime = totalDelay + (CascadeIn ? CascadeInDuration : TimeSpan.Zero) + HoldDuration,
                                Value = 0
                            });
                        rotationAnimation.KeyFrames.Add(
                            new EasingDoubleKeyFrame
                            {
                                KeyTime = totalDelay + (CascadeIn ? CascadeInDuration : TimeSpan.Zero) + HoldDuration + CascadeOutDuration,
                                EasingFunction = CascadeOutEasingFunction,
                                Value = ToRotation
                            });
                    }
                }

                if (CascadeIn)
                {
                    yAnimation.KeyFrames.Add(
                        new DiscreteDoubleKeyFrame
                        {
                            KeyTime = totalDelay,
                            Value = FromVerticalOffset
                        });
                    yAnimation.KeyFrames.Add(
                        new EasingDoubleKeyFrame
                        {
                            KeyTime = totalDelay + CascadeInDuration,
                            EasingFunction = CascadeInEasingFunction,
                            Value = 0
                        });

                    if (UseFade)
                    {
                        opacityAnimation.KeyFrames.Add(
                            new DiscreteDoubleKeyFrame
                            {
                                KeyTime = totalDelay,
                                Value = 0
                            });
                        opacityAnimation.KeyFrames.Add(
                            new EasingDoubleKeyFrame
                            {
                                KeyTime = totalDelay + CascadeInDuration,
                                EasingFunction = FadeInEasingFunction,
                                Value = 1.0
                            });
                    }
                }

                if (CascadeOut)
                {
                    yAnimation.KeyFrames.Add(
                        new DiscreteDoubleKeyFrame
                        {
                            KeyTime = totalDelay + (CascadeIn ? CascadeInDuration : TimeSpan.Zero) + HoldDuration,
                            Value = 0
                        });
                    yAnimation.KeyFrames.Add(
                        new EasingDoubleKeyFrame
                        {
                            KeyTime = totalDelay + (CascadeIn ? CascadeInDuration : TimeSpan.Zero) + HoldDuration + CascadeOutDuration,
                            EasingFunction = CascadeOutEasingFunction,
                            Value = ToVerticalOffset
                        });

                    if (UseFade)
                    {
                        opacityAnimation.KeyFrames.Add(
                            new DiscreteDoubleKeyFrame
                            {
                                KeyTime = totalDelay + (CascadeIn ? CascadeInDuration : TimeSpan.Zero) + HoldDuration,
                                Value = 1.00
                            });
                        opacityAnimation.KeyFrames.Add(
                            new EasingDoubleKeyFrame
                            {
                                KeyTime = totalDelay + (CascadeIn ? CascadeInDuration : TimeSpan.Zero) + HoldDuration + CascadeOutDuration,
                                EasingFunction = FadeOutEasingFunction,
                                Value = 0.0
                            });
                    }
                }

                totalDelay += CascadeInterval;
                i += j;
            }

            EventHandler<object> eh = null;
            eh = (s, e) =>
            {
                cascadeStoryboard.Completed -= eh;
                //LayoutRoot.Children.Clear();
                //var tb2 = CreateTextBlock(null);
                //tb2.Text = Text;
                //LayoutRoot.Children.Add(tb2);

#if CascadingTextBlock_REPEATFOREVER
                BeginCascadingTransition();
#else
                if (CascadeCompleted != null)
                    CascadeCompleted(this, EventArgs.Empty);
#endif
            };

            cascadeStoryboard.Completed += eh;
            await Task.Delay(StartDelay);
            await cascadeStoryboard.BeginAsync();
        }

        private TextBlock CreateTextBlock(TranslateTransform tt)
        {
            TextBlock tb;
            if (TextBlockTemplate == null)
            {
                tb = new TextBlock
                {
                    Foreground = this.Foreground,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    RenderTransform = tt
                };
            }
            else
            {
                tb = (TextBlock)TextBlockTemplate.LoadContent();
                tb.HorizontalAlignment = HorizontalAlignment.Left;
                tb.RenderTransform = tt;
            }
            return tb;
        }
    }
}
