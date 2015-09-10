using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            this.Visibility = coreTitleBar.IsVisible || this.alwaysShow ? Visibility.Visible : Visibility.Collapsed;
            this.UpdateBackButtonVisibility();
            Window.Current.SetTitleBar(this);
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            this.Visibility = coreTitleBar.IsVisible || this.alwaysShow ? Visibility.Visible : Visibility.Collapsed;
            this.UpdateBackButtonVisibility();
            //this.Visibility = Visibility.Visible;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            this.BackButton.Height = this.Height = sender.Height;

            if (this.FlowDirection == FlowDirection.LeftToRight)
            {
                this.LeftColumn.Width = new GridLength(sender.SystemOverlayLeftInset);
                this.RightColumn.Width = new GridLength(0);
            }
            else
            {
                this.LeftColumn.Width = new GridLength(0);
                this.RightColumn.Width = new GridLength(sender.SystemOverlayRightInset);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.BackButtonClicked?.Invoke(this, true);
        }
    }
}
