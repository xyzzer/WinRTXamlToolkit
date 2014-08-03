using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// A simple web browser control based on the WebView control with address bar, navigation buttons and navigation history.
    /// </summary>
    [TemplatePart(Name = LayoutRootPanelName, Type = typeof(Panel))]
    [TemplatePart(Name = WebViewName, Type = typeof(WebView))]
    [TemplatePart(Name = WebViewBrushName, Type = typeof(WebViewBrush))]
    [TemplatePart(Name = AddressBarName, Type = typeof(TextBox))]
    [TemplatePart(Name = TitleTextBlockName, Type = typeof(TextBlock))]
    [TemplatePart(Name = FavIconImageName, Type = typeof(Image))]
    [TemplatePart(Name = ProgressIndicatorName, Type = typeof(ProgressBar))]
    [TemplatePart(Name = BackButtonName, Type = typeof(Button))]
    [TemplatePart(Name = ForwardButtonName, Type = typeof(Button))]
    [TemplatePart(Name = GoButtonName, Type = typeof(Button))]
    [TemplatePart(Name = RefreshButtonName, Type = typeof(Button))]
    [TemplatePart(Name = AddressAppBarName, Type = typeof(CustomAppBar))]
    [TemplatePart(Name = TitleAppBarName, Type = typeof(CustomAppBar))]
    [TemplateVisualState(GroupName = LoadingStatesGroupName, Name = LoadingStateName)]
    [TemplateVisualState(GroupName = LoadingStatesGroupName, Name = LoadedStateName)]
    [TemplateVisualState(GroupName = AddressBarStatesGroupName, Name = AddressBarFocusedStateName)]
    [TemplateVisualState(GroupName = AddressBarStatesGroupName, Name = AddressBarUnfocusedStateName)]
    public class WebBrowser : Control
    {
        #region Template Part Names
        private const string LoadingStatesGroupName = "LoadingStates";
        private const string LoadingStateName = "LoadingState";
        private const string LoadedStateName = "LoadedState";
        private const string AddressBarStatesGroupName = "AddressBarStates";
        private const string AddressBarFocusedStateName = "AddressBarFocused";
        private const string AddressBarUnfocusedStateName = "AddressBarUnfocused";

        private const string LayoutRootPanelName = "LayoutRoot";
        private const string WebViewName = "PART_WebView";
        private const string WebViewBrushName = "PART_WebViewBrush";
        private const string AddressBarName = "PART_AddressBar";
        private const string TitleTextBlockName = "PART_TitleBar";
        private const string AddressAppBarName = "PART_AddressAppBar";
        private const string TitleAppBarName = "PART_TitleAppBar";
        private const string FavIconImageName = "PART_FavIconImage";
        private const string ProgressIndicatorName = "PART_ProgressIndicator";
        private const string BackButtonName = "PART_BackButton";
        private const string ForwardButtonName = "PART_ForwardButton";
        private const string GoButtonName = "PART_GoButton";
        private const string StopButtonName = "PART_StopButton";
        private const string RefreshButtonName = "PART_RefreshButton";
        #endregion

        #region Template Part Fields
        private Panel _layoutRoot;
        private WebView _webView;
        private WebViewBrush _webViewBrush;
        private TextBox _addressBar;
        private TextBlock _titleBar;
        private CustomAppBar _addressAppBar;
        private CustomAppBar _titleAppBar;
        private Image _favIconImage;
        private ProgressBar _progressIndicator;
        private Button _backButton;
        private Button _forwardButton;
        private Button _goButton;
        private Button _stopButton;
        private Button _refreshButton;
        #endregion

        private bool _ctrlPressed;
        private List<Uri> _backStack = new List<Uri>();
        private int _backStackPosition = -1;
        private bool _settingSourceWithCode;
        private bool _pendingNavigation;

        #region AutoNavigate
        /// <summary>
        /// AutoNavigate Dependency Property
        /// </summary>
        public static readonly DependencyProperty AutoNavigateProperty =
            DependencyProperty.Register(
                "AutoNavigate",
                typeof(bool),
                typeof(WebBrowser),
                new PropertyMetadata(true, OnAutoNavigateChanged));

        /// <summary>
        /// Gets or sets the AutoNavigate property. This dependency property 
        /// indicates whether the browser should automatically navigate when the Source property is set.
        /// </summary>
        public bool AutoNavigate
        {
            get { return (bool)GetValue(AutoNavigateProperty); }
            set { SetValue(AutoNavigateProperty, value); }
        }

        /// <summary>
        /// Handles changes to the AutoNavigate property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnAutoNavigateChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (WebBrowser)d;
            bool oldAutoNavigate = (bool)e.OldValue;
            bool newAutoNavigate = target.AutoNavigate;
            target.OnAutoNavigateChanged(oldAutoNavigate, newAutoNavigate);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the AutoNavigate property.
        /// </summary>
        /// <param name="oldAutoNavigate">The old AutoNavigate value</param>
        /// <param name="newAutoNavigate">The new AutoNavigate value</param>
        private void OnAutoNavigateChanged(
            bool oldAutoNavigate, bool newAutoNavigate)
        {
            if (newAutoNavigate &&
                this.Source != null)
            {
                this.Navigate(this.Source);
            }
        }
        #endregion

        #region Source
        /// <summary>
        /// Source Dependency Property
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                "Source",
                typeof(Uri),
                typeof(WebBrowser),
                new PropertyMetadata(null, OnSourceChanged));

        /// <summary>
        /// Gets or sets the Source property. This dependency property 
        /// indicates the source to navigate to.
        /// </summary>
        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
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
            var target = (WebBrowser)d;
            Uri oldSource = (Uri)e.OldValue;
            Uri newSource = target.Source;
            target.OnSourceChanged(oldSource, newSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Source property.
        /// </summary>
        /// <param name="oldSource">The old Source value</param>
        /// <param name="newSource">The new Source value</param>
        protected virtual void OnSourceChanged(
            Uri oldSource, Uri newSource)
        {
            if (this.AutoNavigate &&
                !_settingSourceWithCode &&
                oldSource != newSource)
            {
                this.Navigate(this.Source);
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Title Dependency Property
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(string),
                typeof(WebBrowser),
                new PropertyMetadata(null, OnTitleChanged));

        /// <summary>
        /// Gets or sets the Title property. This dependency property 
        /// indicates the title of the current source.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Title property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnTitleChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (WebBrowser)d;
            string oldTitle = (string)e.OldValue;
            string newTitle = target.Title;
            target.OnTitleChanged(oldTitle, newTitle);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Title property.
        /// </summary>
        /// <param name="oldTitle">The old Title value</param>
        /// <param name="newTitle">The new Title value</param>
        protected virtual void OnTitleChanged(
            string oldTitle, string newTitle)
        {
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowser"/> class.
        /// </summary>
        public WebBrowser()
        {
            DefaultStyleKey = typeof (WebBrowser);
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _layoutRoot = GetTemplateChild(LayoutRootPanelName) as Panel;
            _webView = (WebView)GetTemplateChild(WebViewName);
            _webViewBrush = GetTemplateChild(WebViewBrushName) as WebViewBrush;
            _addressBar = GetTemplateChild(AddressBarName) as TextBox;
            _titleBar = GetTemplateChild(TitleTextBlockName) as TextBlock;
            _favIconImage = GetTemplateChild(FavIconImageName) as Image;
            _progressIndicator = GetTemplateChild(ProgressIndicatorName) as ProgressBar;
            _backButton = GetTemplateChild(BackButtonName) as Button;
            _forwardButton = GetTemplateChild(ForwardButtonName) as Button;
            _goButton = GetTemplateChild(GoButtonName) as Button;
            _stopButton = GetTemplateChild(StopButtonName) as Button;
            _refreshButton = GetTemplateChild(RefreshButtonName) as Button;
            _addressAppBar = GetTemplateChild(AddressAppBarName) as CustomAppBar;
            _titleAppBar = GetTemplateChild(TitleAppBarName) as CustomAppBar;
            
            VisualStateManager.GoToState(this, AddressBarUnfocusedStateName, true);

            if (_addressBar != null)
            {
                _addressBar.KeyDown += OnAddressBarKeyDown;
                _addressBar.KeyUp += OnAddressBarKeyUp;
                _addressBar.GotFocus += OnAddressBarGotFocus;
                _addressBar.LostFocus += OnAddressBarLostFocus;
                _addressBar.TextChanged += OnAddressBarTextChanged;
            }

            if (_backButton != null)
            {
                _backButton.IsEnabled = false;
                _backButton.Click += OnBackButtonClick;
            }
            if (_forwardButton != null)
            {
                _forwardButton.IsEnabled = false;
                _forwardButton.Click += OnForwardButtonClick;
            }
            if (_goButton != null)
            {
                //_goButton.IsEnabled = false;
                _goButton.Click += OnGoButtonClick;
            }
            if (_stopButton != null)
            {
                _stopButton.IsEnabled = false;
                _stopButton.Click += OnStopButtonClick;
            }
            if (_refreshButton != null)
            {
                _refreshButton.IsEnabled = false;
                _refreshButton.Click += OnRefreshButtonClick;
            }
            if (_addressAppBar != null)
            {
                _addressAppBar.Opened += OnAppBarOpenedOrClosed;
                _addressAppBar.Closed += OnAppBarOpenedOrClosed;
            }
            if (_titleAppBar != null)
            {
                _titleAppBar.Opened += OnAppBarOpenedOrClosed;
                _titleAppBar.Closed += OnAppBarOpenedOrClosed;
            }

            _webView.NavigationCompleted += OnNavigationCompleted;

            if (this.Source != null)
            {
                _webView.Source = this.Source;
            }
            else
            {
                this.Source = _webView.Source;
            }

            if (_pendingNavigation)
            {
                Navigate(this.Source);
            }
        }

        private async void OnAppBarOpenedOrClosed(object sender, object e)
        {
            if (_addressAppBar != null && _addressAppBar.IsOpen ||
                _titleAppBar != null && _titleAppBar.IsOpen)
            {
                _webViewBrush.SetSource(_webView);
                _webView.Visibility = Visibility.Collapsed;
            }
            else
            {
                await Task.Delay(200);
                _webView.Visibility = Visibility.Visible;
            }
        }

        private void OnAddressBarTextChanged(object sender, TextChangedEventArgs e)
        {
            //_goButton.IsEnabled =
            //    _addressBar.Text.Length > 0 &&
            //    (_backStackPosition < 0 ||
            //     _backStack[_backStackPosition] == _webView.Source);
            //&& _goButton.FocusState != FocusState.Unfocused;
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            _backStackPosition--;
            this.Navigate(_backStack[_backStackPosition]);
            UpdateBackStackKeys();
        }

        private void UpdateBackStackKeys()
        {
            _backButton.IsEnabled = (_backStackPosition != 0);
            _forwardButton.IsEnabled = (_backStackPosition < _backStack.Count - 1);
        }

        private void OnForwardButtonClick(object sender, RoutedEventArgs e)
        {
            _backStackPosition++;
            Navigate(_backStack[_backStackPosition]);
            UpdateBackStackKeys();
        }

        private void OnGoButtonClick(object sender, RoutedEventArgs e)
        {
            var url = _addressBar.Text;
            if (!url.StartsWith("http") && !url.Contains(":"))
                url = "http://" + url;
            this.Navigate(new Uri(url));
        }

        private void OnStopButtonClick(object sender, RoutedEventArgs e)
        {
            //_webView.
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void OnAddressBarGotFocus(object sender, RoutedEventArgs e)
        {
            if (_addressBar.Text.StartsWith("http://"))
                _addressBar.Text = _addressBar.Text.Substring(7);
            _addressBar.SelectAll();
            VisualStateManager.GoToState(this, AddressBarFocusedStateName, true);
        }

        private void OnAddressBarLostFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, AddressBarUnfocusedStateName, true);
        }

        private void OnAddressBarKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Control)
            {
                _ctrlPressed = true;
            }

            if (e.Key == VirtualKey.Enter)
            {
                var url = _addressBar.Text;

                if (_ctrlPressed)
                {
                    if (!url.EndsWith(".com") && !url.Replace("://", "").Contains("/"))
                        url = url + ".com";
                    if (!url.StartsWith("http") && !url.StartsWith("www."))
                        url = "http://www." + url;
                }

                if (!url.StartsWith("http") && !url.Contains(":"))
                    url = "http://" + url;
                this.Navigate(new Uri(url));
            }
        }

        private void OnAddressBarKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Control)
            {
                _ctrlPressed = false;
            }
        }

        private void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                OnNavigationSucceeded();
            }
            else
            {
                OnNavigationFailed(e.WebErrorStatus);
            }
        }

        private async void OnNavigationSucceeded()
        {
            VisualStateManager.GoToState(this, LoadedStateName, true);
            //await Task.Delay(100);
            // This doesn't work
            //_webViewBrush.SetSource(_webView);
            // Need to close the app bars instead to force the WebView to show up
            _addressAppBar.IsOpen = false;
            _titleAppBar.IsOpen = false;
            var address = await _webView.GetAddress();

            this.Source = address == null ? null : new Uri(address);

            if (_addressBar != null)
            {
                _addressBar.Text = address ?? string.Empty;
            }

            if (_titleBar != null)
            {
                _titleBar.Text = await _webView.GetTitle();
            }

            if (_favIconImage != null &&
                address != null)
            {
                var favIconUri = await _webView.GetFavIconLink();

                if (favIconUri == null)
                    _favIconImage.Source = null;
                else
                    _favIconImage.Source = new BitmapImage(favIconUri);
            }

            if (_backStackPosition < 0 ||
                _backStack[_backStackPosition].ToString() != address)
            {
                if (_backStack.Count > _backStackPosition + 1)
                {
                    _backStack.RemoveRange(_backStackPosition + 1, _backStack.Count - _backStackPosition - 1);
                }

                _backStack.Add(_webView.Source);

                _backStackPosition = _backStack.Count - 1;
            }

            UpdateBackStackKeys();
        }

        private void OnNavigationFailed(WebErrorStatus webErrorStatus)
        {
            VisualStateManager.GoToState(this, LoadedStateName, true);

            if (_titleBar != null)
            {
                _titleBar.Text = webErrorStatus.ToString();
            }
        }

        /// <summary>
        /// Navigates to the specified source uri.
        /// </summary>
        /// <param name="source">The source.</param>
        public void Navigate(Uri source)
        {
#pragma warning disable 4014
            this.NavigateAsync(source);
#pragma warning restore 4014
        }

        /// <summary>
        /// Navigates to the specified source uri asynchronously.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public async Task NavigateAsync(Uri source)
        {
            _settingSourceWithCode = true;
            this.Source = source;
            _settingSourceWithCode = false;

            if (_webView == null)
            {
                _pendingNavigation = true;
                return;
            }

            VisualStateManager.GoToState(this, LoadingStateName, true);
            _refreshButton.IsEnabled = false;
            _goButton.Focus(FocusState.Programmatic);
            //_goButton.IsEnabled = false;
            _addressBar.Text = source.ToString();
            await _webView.NavigateAsync(source);
            _refreshButton.IsEnabled = true;
        }

        /// <summary>
        /// Refreshes the current page.
        /// </summary>
        public void Refresh()
        {
            Navigate(_webView.Source);
        }
    }
}
