using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// A control that loads the ContentTemplate
    /// after specified Delay elapses from the time the control itself loads.
    /// </summary>
    /// <remarks>
    /// If the IsEnabled property is false - the content does not load.
    /// Instead it only starts loading after the given Delay from the time
    /// IsEnabled is set to true again.
    /// </remarks>
    [TemplatePart(Name = ContentBorderName, Type = typeof(Border))]
    public class DelayedLoadControl : Control
    {
        private const string ContentBorderName = "PART_ContentBorder";
        private Border _contentBorder;
        private int _loadRequestId;

        #region Delay
        /// <summary>
        /// The delay property.
        /// </summary>
        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register(
                "Delay",
                typeof(TimeSpan),
                typeof(DelayedLoadControl),
                new PropertyMetadata(TimeSpan.FromSeconds(0.0)));

        /// <summary>
        /// Gets or sets the delay after which the ContentTemplate should be loaded.
        /// </summary>
        /// <remarks>
        /// If the IsEnabled property is false - the content does not load.
        /// Instead it only starts loading after the given Delay from the time
        /// IsEnabled is set to true again.
        /// </remarks>
        /// <value>
        /// The delay.
        /// </value>
        public TimeSpan Delay
        {
            get { return (TimeSpan)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }
        #endregion

        #region ContentTemplate
        /// <summary>
        /// The content template property
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(
                "ContentTemplate",
                typeof(DataTemplate),
                typeof(DelayedLoadControl),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the content template that should be loaded
        /// after the specified Delay.
        /// </summary>
        /// <value>
        /// The content template.
        /// </value>
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DelayedLoadControl" /> class.
        /// </summary>
        public DelayedLoadControl()
        {
            this.DefaultStyleKey = typeof (DelayedLoadControl);
            this.Loaded += OnLoaded;
            this.IsEnabledChanged += OnIsEnabledChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _contentBorder = GetTemplateChild(ContentBorderName) as Border;
        }

        private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            _loadRequestId++;

            if (IsEnabled)
            {
                this.DelayedLoad();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (IsEnabled)
            {
                this.DelayedLoad();
            }
        }

        private async void DelayedLoad()
        {
            _loadRequestId++;

            var handledRequestId = _loadRequestId;
            await Task.Delay(Delay);

            if (handledRequestId == _loadRequestId &&
                _contentBorder.Child == null)
            {
                _contentBorder.Child = (UIElement)ContentTemplate.LoadContent();
            }
        }
    }
}
