using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Debugging;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class WindowTitleBar : UserControl
    {
        private bool alwaysShow;

        public static WindowTitleBar Instance { get; private set; }

        public static void SetText(string text, bool alwaysShow)
        {
            Instance.Text = text;
            Instance.alwaysShow = alwaysShow;
        }

        public string Text
        {
            get
            {
                return this.Label.Text;
            }
            set
            {
                this.Label.Text = value;
            }
        }

        public event EventHandler<bool> BackButtonClicked;
        private bool isBackButtonVisible;
        public bool IsBackButtonVisible
        {
            get
            {
                return isBackButtonVisible;
            }
            set
            {
                this.isBackButtonVisible = value;
                this.UpdateBackButtonVisibility();
            }
        }

        private void UpdateBackButtonVisibility()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            this.BackButton.Visibility = this.isBackButtonVisible && coreTitleBar.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public WindowTitleBar()
        {
            Instance = this;
            this.InitializeComponent();
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            this.UpdateDebugConsoleMargin(coreTitleBar);
            this.Visibility = coreTitleBar.IsVisible || this.alwaysShow ? Visibility.Visible : Visibility.Collapsed;
            this.UpdateBackButtonVisibility();
            Window.Current.SetTitleBar(this.LabelGrid);
            
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.DeferInitializationAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
        }

        private void UpdateDebugConsoleMargin(CoreApplicationViewTitleBar coreTitleBar = null)
        {
            coreTitleBar = coreTitleBar ?? CoreApplication.GetCurrentView().TitleBar;
            DC.Margin = new Thickness(0, coreTitleBar.IsVisible ? coreTitleBar.Height : 0, 0, 0);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Window.Current.Activated += this.OnActivated;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.Activated -= this.OnActivated;
        }

        private void OnActivated(object sender, WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                this.RootGrid.Opacity = .5;
            }
            else
            {
                this.RootGrid.Opacity = 1;
            }
        }

        private async Task DeferInitializationAsync()
        {
            await Task.Delay(1000);
            this.UpdateContentLayout(CoreApplication.GetCurrentView().TitleBar);
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            this.Visibility = coreTitleBar.IsVisible || this.alwaysShow ? Visibility.Visible : Visibility.Collapsed;
            this.UpdateBackButtonVisibility();
            this.UpdateDebugConsoleMargin(coreTitleBar);
            //this.Visibility = Visibility.Visible;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar coreTitleBar, object args)
        {
            UpdateContentLayout(coreTitleBar);
            this.UpdateDebugConsoleMargin(coreTitleBar);
        }

        private void UpdateContentLayout(CoreApplicationViewTitleBar sender)
        {
            this.DebugButton.Height =
            this.BackButton.Height = this.Height = sender.Height;

            this.LeftColumn.Width = new GridLength(sender.SystemOverlayLeftInset);
            this.RightColumn.Width = new GridLength(sender.SystemOverlayRightInset);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.BackButtonClicked?.Invoke(this, true);
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            DC.ShowVisualTreeAsync(this);
        }
    }
}
