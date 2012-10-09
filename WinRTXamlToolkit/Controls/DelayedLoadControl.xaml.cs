using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
{
    public sealed partial class DelayedLoadControl : UserControl
    {
        private int _loadRequestId;

        #region Delay
        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register(
                "Delay",
                typeof(TimeSpan),
                typeof(DelayedLoadControl),
                new PropertyMetadata(TimeSpan.FromSeconds(0.0)));

        public TimeSpan Delay
        {
            get { return (TimeSpan)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }
        #endregion

        #region ContentTemplate
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(
                "ContentTemplate",
                typeof(DataTemplate),
                typeof(DelayedLoadControl),
                new PropertyMetadata(null));

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        } 
        #endregion

        public DelayedLoadControl()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
            this.IsEnabledChanged += OnIsEnabledChanged;
        }

        private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            this.DelayedLoad();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.DelayedLoad();
        }

        private async void DelayedLoad()
        {
            _loadRequestId++;

            if (!IsEnabled)
                return;

            var handledRequestId = _loadRequestId;
            await Task.Delay(Delay);

            if (handledRequestId == _loadRequestId)
            {
                LayoutRoot.Children.Add((UIElement) ContentTemplate.LoadContent());
            }
        }
    }
}
