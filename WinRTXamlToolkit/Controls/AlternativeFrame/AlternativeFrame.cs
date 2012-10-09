using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRTXamlToolkit.AwaitableUI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WinRTXamlToolkit.Controls
{
    [TemplatePart(Name = "PagePresentersPanelName", Type = typeof(Panel))]
    public class AlternativeFrame : ContentControl
    {
        private const int WaitForImagesToLoadTimeout = 1000;

        public event AlternativeNavigationEventHandler Navigated;
        public event AlternativeNavigatingCancelEventHandler Navigating;

        #region fields
        private const string PagePresentersPanelName = "PART_PagePresentersPanel";
        private readonly TaskCompletionSource<bool> _waitForApplyTemplateTaskSource = new TaskCompletionSource<bool>(false);
        private readonly Dictionary<JournalEntry, ContentPresenter> _preloadedPageCache;
        private ContentPresenter _currentPagePresenter;
        private Panel _pagePresentersPanel;
        private bool _isNavigating;
        #endregion

        #region Properties
        public Stack<JournalEntry> BackStack { get; private set; }
        public JournalEntry CurrentJournalEntry { get; private set; }
        public Stack<JournalEntry> ForwardStack { get; private set; }
        public Type CurrentSourcePageType
        {
            get
            {
                return CurrentJournalEntry.SourcePageType;
            }
        }

        #region PagePresenterStyle
        /// <summary>
        /// PagePresenterStyle Dependency Property
        /// </summary>
        public static readonly DependencyProperty PagePresenterStyleProperty =
            DependencyProperty.Register(
                "PagePresenterStyle",
                typeof(Style),
                typeof(AlternativeFrame),
                new PropertyMetadata(null, OnPagePresenterStyleChanged));

        /// <summary>
        /// Gets or sets the PagePresenterStyle property. This dependency property 
        /// indicates the style of the ContentPresenters used to host the pages.
        /// </summary>
        public Style PagePresenterStyle
        {
            get { return (Style)GetValue(PagePresenterStyleProperty); }
            set { SetValue(PagePresenterStyleProperty, value); }
        }

        /// <summary>
        /// Handles changes to the PagePresenterStyle property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnPagePresenterStyleChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (AlternativeFrame)d;
            Style oldPagePresenterStyle = (Style)e.OldValue;
            Style newPagePresenterStyle = target.PagePresenterStyle;
            target.OnPagePresenterStyleChanged(oldPagePresenterStyle, newPagePresenterStyle);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the PagePresenterStyle property.
        /// </summary>
        /// <param name="oldPagePresenterStyle">The old PagePresenterStyle value</param>
        /// <param name="newPagePresenterStyle">The new PagePresenterStyle value</param>
        protected virtual void OnPagePresenterStyleChanged(
            Style oldPagePresenterStyle, Style newPagePresenterStyle)
        {
        }
        #endregion

        #region PageTransition
        /// <summary>
        /// PageTransition Dependency Property
        /// </summary>
        public static readonly DependencyProperty PageTransitionProperty =
            DependencyProperty.Register(
                "PageTransition",
                typeof(PageTransition),
                typeof(AlternativeFrame),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the PageTransition property. This dependency property 
        /// indicates the PageTransition to use to transition between pages.
        /// </summary>
        public PageTransition PageTransition
        {
            get { return (PageTransition)GetValue(PageTransitionProperty); }
            set { SetValue(PageTransitionProperty, value); }
        }
        #endregion

        #region CanGoBack
        /// <summary>
        /// CanGoBack Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanGoBackProperty =
            DependencyProperty.Register(
                "CanGoBack",
                typeof(bool),
                typeof(AlternativeFrame),
                new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the CanGoBack property. This dependency property 
        /// indicates whether you can go back in the navigation history.
        /// </summary>
        public bool CanGoBack
        {
            get { return (bool)GetValue(CanGoBackProperty); }
            private set { SetValue(CanGoBackProperty, value); }
        }
        #endregion

        #region CanGoForward
        /// <summary>
        /// CanGoForward Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanGoForwardProperty =
            DependencyProperty.Register(
                "CanGoForward",
                typeof(bool),
                typeof(AlternativeFrame),
                new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the CanGoForward property. This dependency property 
        /// indicates whether you can go forward in the navigation history.
        /// </summary>
        public bool CanGoForward
        {
            get { return (bool)GetValue(CanGoForwardProperty); }
            private set { SetValue(CanGoForwardProperty, value); }
        }
        #endregion

        #region CanNavigate
        /// <summary>
        /// CanNavigate Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanNavigateProperty =
            DependencyProperty.Register(
                "CanNavigate",
                typeof(bool),
                typeof(AlternativeFrame),
                new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the CanNavigate property. This dependency property 
        /// indicates whether the frame is in a state where a navigation call will be accepted.
        /// </summary>
        public bool CanNavigate
        {
            get { return (bool)GetValue(CanNavigateProperty); }
            private set { SetValue(CanNavigateProperty, value); }
        }
        #endregion

        #region ShouldWaitForImagesToLoad
        /// <summary>
        /// ShouldWaitForImagesToLoad Dependency Property
        /// </summary>
        public static readonly DependencyProperty ShouldWaitForImagesToLoadProperty =
            DependencyProperty.Register(
                "ShouldWaitForImagesToLoad",
                typeof(bool?),
                typeof(AlternativeFrame),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the ShouldWaitForImagesToLoad property. This dependency property 
        /// indicates whether the frame should wait for all images in a page to load
        /// before transitioning to the next page.
        /// </summary>
        public bool? ShouldWaitForImagesToLoad
        {
            get { return (bool?)GetValue(ShouldWaitForImagesToLoadProperty); }
            set { SetValue(ShouldWaitForImagesToLoadProperty, value); }
        }
        #endregion 

        public int BackStackDepth
        {
            get
            {
                return BackStack.Count;
            }
        }

        public int ForwardStackDepth
        {
            get
            {
                return ForwardStack.Count;
            }
        }
        #endregion

        #region CTOR
        public AlternativeFrame()
        {
            this.BackStack = new Stack<JournalEntry>();
            this.ForwardStack = new Stack<JournalEntry>();
            _preloadedPageCache = new Dictionary<JournalEntry, ContentPresenter>();
            this.DefaultStyleKey = typeof(AlternativeFrame);

            //TODO: Complete this work in progress - support for navigation events when suspending and resuming the app or deactivating the window.
        //    this.Loaded += OnLoaded;
        //    this.Unloaded += OnUnloaded;
        //}

        //private void OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    Application.Current.Suspending += OnAppSuspending;
        //    Application.Current.Resuming += OnAppResuming;
        //}

        //private void OnAppResuming(object sender, object e)
        //{
        //    throw new NotImplementedException();
        //}

        //private async void OnAppSuspending(object sender, SuspendingEventArgs e)
        //{
        //    var currentPage = (AlternativePage)_currentPagePresenter.Content;
        //    var cancelArgs =
        //        new AlternativeNavigatingCancelEventArgs(
        //            NavigationMode.Forward,
        //            null);
        //    await this.OnNavigating(cancelArgs);

        //    await currentPage.OnNavigatingFromInternal(cancelArgs);
        //    var args = new AlternativeNavigationEventArgs(null, NavigationMode.Forward, null, null);
        //    await currentPage.OnNavigatedFromInternal(args);
        //}

        //private void OnUnloaded(object sender, RoutedEventArgs e)
        //{
        //    Application.Current.Suspending -= OnAppSuspending;
        //    this.Loaded -= OnLoaded;
        //    this.Unloaded -= OnUnloaded;
        }
        #endregion

        #region Preload()
        // Allows to preload a page before user navigates to it, so if it does get navigated to - it is quick.
        public async Task<bool> Preload(Type sourcePageType, object parameter)
        {
            var je = new JournalEntry { SourcePageType = sourcePageType, Parameter = parameter };

            if (_preloadedPageCache.ContainsKey(je))
            {
                return true;
            }

            var cp = new ContentPresenter { Style = PagePresenterStyle };
            var newPage = Activator.CreateInstance(sourcePageType) as AlternativePage;

            Debug.Assert(newPage != null, "Pages used in AlternativeFrame need to be of AlternativePage type.");

            if (newPage == null)
            {
                return false;
            }

            newPage.Frame = this;
            cp.Content = newPage;
            cp.Opacity = 0.005;
            Canvas.SetZIndex(cp, int.MinValue);
            _pagePresentersPanel.Children.Insert(0, cp);
            _preloadedPageCache.Add(je, cp);
            await newPage.PreloadInternal(parameter);
            return true;
        } 
        #endregion

        #region UnloadPreloaded()
        public async Task UnloadPreloaded(Type sourcePageType, object parameter)
        {
            var je = new JournalEntry { SourcePageType = sourcePageType, Parameter = parameter };

            if (!_preloadedPageCache.ContainsKey(je))
            {
                return;
            }

            var cp = _preloadedPageCache[je];
            var page = (AlternativePage)cp.Content;
            await page.UnloadPreloadedInternal();

            _pagePresentersPanel.Children.Remove(cp);
            _preloadedPageCache.Remove(je);
        } 
        #endregion

        #region UnloadAllPreloaded()
        public async Task UnloadAllPreloaded()
        {
            foreach (var kvp in _preloadedPageCache)
            {
                _pagePresentersPanel.Children.Remove(kvp.Value);
                var page = (AlternativePage)kvp.Value.Content;
                await page.UnloadPreloadedInternal();
            }

            _preloadedPageCache.Clear();
        }
        #endregion

        #region Navigate()
        public async Task<bool> Navigate(Type sourcePageType)
        {
            return await Navigate(sourcePageType, null);
        } 

        public async Task<bool> Navigate(Type sourcePageType, object parameter)
        {
            if (_isNavigating)
            {
                throw new InvalidOperationException("Navigation already in progress.");
            }

            if (!this.CanNavigate)
            {
                throw new InvalidOperationException("Navigate() call failed. CanNavigate is false.");
            }

            return await NavigateCore(
                sourcePageType,
                parameter,
                NavigationMode.New);
        }
        #endregion

        #region GoBack()
        public async Task<bool> GoBack()
        {
            if (_isNavigating)
            {
                throw new InvalidOperationException("Navigation already in progress.");
            }

            if (!this.CanGoBack)
            {
                throw new InvalidOperationException("GoBack() call failed. CanGoBack is false.");
            }

            if (!this.CanNavigate)
            {
                throw new InvalidOperationException("GoBack() call failed. CanNavigate is false.");
            }

            var backJournalEntry = this.BackStack.Peek();

            return await NavigateCore(
                backJournalEntry.SourcePageType,
                backJournalEntry.Parameter,
                NavigationMode.Back);
        } 
        #endregion

        #region GoForward()
        public async Task<bool> GoForward()
        {
            if (_isNavigating)
            {
                throw new InvalidOperationException("Navigation already in progress.");
            }

            if (!this.CanGoForward)
            {
                throw new InvalidOperationException("GoForward() call failed. CanGoForward is false.");
            }

            if (!this.CanNavigate)
            {
                throw new InvalidOperationException("GoForward() call failed. CanNavigate is false.");
            }

            var forwardJournalEntry = this.ForwardStack.Peek();

            return await NavigateCore(
                forwardJournalEntry.SourcePageType,
                forwardJournalEntry.Parameter,
                NavigationMode.Forward);
        }
        #endregion

        #region SetNavigationState()
        /// <summary>
        /// Reads and restores the navigation history of a Frame from a provided serialization string.
        /// </summary>
        /// <param name="navigationState">
        /// The serialization string that supplies the restore point for navigation history.
        /// </param>
        /// <returns></returns>
        public async Task SetNavigationState(string navigationState)
        {
            try
            {
                Debug.Assert(navigationState[0] == '1');
                Debug.Assert(navigationState[1] == ',');
                int parseIndex = 2;
                var totalPages = ParseNumber(navigationState, ref parseIndex);

                this.BackStack.Clear();
                this.ForwardStack.Clear();
                this.CurrentJournalEntry = null;

                if (_currentPagePresenter != null)
                {
                    _pagePresentersPanel.Children.Remove(_currentPagePresenter);
                    _currentPagePresenter = null;
                }

                if (totalPages == 0)
                {
                    this.CurrentJournalEntry = null;
                    await UnloadAllPreloaded();
                    return;
                }

                var backStackDepth = ParseNumber(navigationState, ref parseIndex);

                var reversedForwardStack = new List<JournalEntry>(totalPages - backStackDepth);

                for (int i = 0; i < totalPages; i++)
                {
                    var je = ParseJournalEntry(navigationState, ref parseIndex);

                    Debug.WriteLine("Type: {0}, Param: {1}", je.SourcePageType, je.Parameter);
                    if (i < backStackDepth)
                    {
                        this.BackStack.Push(je);
                    }
                    else
                    {
                        reversedForwardStack.Add(je);
                    }
                }

                reversedForwardStack.Reverse();

                foreach (var journalEntry in reversedForwardStack)
                {
                    this.ForwardStack.Push(journalEntry);
                }

                var forwardJournalEntry = this.ForwardStack.Peek();

                await NavigateCore(
                    forwardJournalEntry.SourcePageType,
                    forwardJournalEntry.Parameter,
                    NavigationMode.Forward);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not deserialize frame navigation state.", "navigationState", ex);
            }
        }
        #endregion

        #region ParseJournalEntry()
        private JournalEntry ParseJournalEntry(string parsedString, ref int parseIndex)
        {
            var sourcePageTypeLength = ParseNumber(parsedString, ref parseIndex);
            var sourcePageTypeName = parsedString.Substring(parseIndex, sourcePageTypeLength);
            var sourcePageType = Type.GetType(sourcePageTypeName);
            parseIndex += sourcePageTypeLength + 1;
            var parameterTypeCode = ParseNumber(parsedString, ref parseIndex);
            
            if (parameterTypeCode == 0)
            {
                return new JournalEntry {SourcePageType = sourcePageType};
            }

            var parameterStringLength = ParseNumber(parsedString, ref parseIndex);
            var parameterString = parsedString.Substring(parseIndex, parameterStringLength);
            parseIndex += parameterStringLength + 1;
            object parameter;

            switch (parameterTypeCode)
            {
                case 1:
                    parameter = byte.Parse(parameterString);
                    break;
                case 2:
                    parameter = Int16.Parse(parameterString);
                    break;
                case 3:
                    parameter = UInt16.Parse(parameterString);
                    break;
                case 4:
                    parameter = Int32.Parse(parameterString);
                    break;
                case 5:
                    parameter = UInt32.Parse(parameterString);
                    break;
                case 6:
                    parameter = Int64.Parse(parameterString);
                    break;
                case 7:
                    parameter = UInt64.Parse(parameterString);
                    break;
                case 8:
                    parameter = Single.Parse(parameterString);
                    break;
                case 9:
                    parameter = Double.Parse(parameterString);
                    break;
                case 10:
                    parameter = parameterString[0];
                    break;
                case 11:
                    parameter = Boolean.Parse(parameterString);
                    break;
                case 12:
                    parameter = parameterString;
                    break;
                default:
                    throw new ArgumentException("Parsing JournalEntry failed - unknown parameter type.");
            }

            return new JournalEntry
                   {
                       SourcePageType = sourcePageType,
                       Parameter = parameter
                   };
        }
        #endregion

        #region ParseNumber()
        private static int ParseNumber(string parsedString, ref int parseIndex)
        {
            var nextCommaIndex = parsedString.IndexOf(',', parseIndex);
            var delimiterIndex = nextCommaIndex >= 0
                                     ? nextCommaIndex
                                     : parsedString.Length;
            var valueString = parsedString.Substring(parseIndex, delimiterIndex - parseIndex);
            parseIndex = delimiterIndex + 1;
            return int.Parse(valueString);
        }
        #endregion

        #region GetNavigationState()
        /// <summary>
        /// Serializes the Frame navigation history into a string.
        /// </summary>
        /// <remarks>
        /// Serialization is similar to the one used in the matching Frame class method:
        /// 1,TotalPages,BackStackDepth[,SourcePageType_Length,SourcePageType,Parameter_TypeCode,Parameter_Length,Parameter…]
        /// </remarks>
        /// <returns></returns>
        public string GetNavigationState()
        {
            var totalPages = this.BackStack.Count + this.ForwardStack.Count;

            if (this.CurrentJournalEntry != null)
            {
                totalPages++;
            }

            if (totalPages == 0)
            {
                return "1,0";
            }

            var sb = new StringBuilder();
            sb.AppendFormat("1,{0},{1}", totalPages, this.BackStackDepth);

            foreach (var journalEntry in this.BackStack.Reverse())
            {
                AppendEntryToNavigationStateString(sb, journalEntry);
            }

            if (this.CurrentJournalEntry != null)
            {
                AppendEntryToNavigationStateString(sb, this.CurrentJournalEntry);
            }

            foreach (var journalEntry in this.ForwardStack)
            {
                AppendEntryToNavigationStateString(sb, journalEntry);
            }

            return sb.ToString();
        }

        #region AppendEntryToNavigationStateString()
        private void AppendEntryToNavigationStateString(StringBuilder sb, JournalEntry journalEntry)
        {
            var pageTypeString = journalEntry.SourcePageType.AssemblyQualifiedName;
            sb.AppendFormat(",{0},{1}", pageTypeString.Length, pageTypeString);

            if (journalEntry.Parameter == null)
            {
                sb.Append(",0");
                return;
            }

            var valueString = journalEntry.Parameter.ToString();

            if (journalEntry.Parameter is byte)
            {
                AddParameterToNavigationStateString(sb, 1, valueString);
                return;
            }
            if (journalEntry.Parameter is Int16)
            {
                AddParameterToNavigationStateString(sb, 2, valueString);
                return;
            }
            if (journalEntry.Parameter is UInt16)
            {
                AddParameterToNavigationStateString(sb, 3, valueString);
                return;
            }
            if (journalEntry.Parameter is Int32)
            {
                AddParameterToNavigationStateString(sb, 4, valueString);
                return;
            }
            if (journalEntry.Parameter is UInt32)
            {
                AddParameterToNavigationStateString(sb, 5, valueString);
                return;
            }
            if (journalEntry.Parameter is Int64)
            {
                AddParameterToNavigationStateString(sb, 6, valueString);
                return;
            }
            if (journalEntry.Parameter is UInt64)
            {
                AddParameterToNavigationStateString(sb, 7, valueString);
                return;
            }
            if (journalEntry.Parameter is Single)
            {
                AddParameterToNavigationStateString(sb, 8, valueString);
                return;
            }
            if (journalEntry.Parameter is Double)
            {
                AddParameterToNavigationStateString(sb, 9, valueString);
                return;
            }
            if (journalEntry.Parameter is Char)
            {
                AddParameterToNavigationStateString(sb, 10, valueString);
                return;
            }
            if (journalEntry.Parameter is Boolean)
            {
                AddParameterToNavigationStateString(sb, 11, valueString);
                return;
            }
            if (journalEntry.Parameter is String)
            {
                AddParameterToNavigationStateString(sb, 12, valueString);
            }
        } 
        #endregion

        private void AddParameterToNavigationStateString(StringBuilder sb, int typeCode, string valueString)
        {
            var length = valueString.Length;

            if (length == 0)
            {
                sb.AppendFormat(",{0},0", typeCode);
            }
            else
            {
                sb.AppendFormat(",{0},{1},{2}", typeCode, length, valueString);
            }
        }

        #endregion

        #region OnApplyTemplate()
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _pagePresentersPanel = (Panel)GetTemplateChild(PagePresentersPanelName);
            _waitForApplyTemplateTaskSource.SetResult(true);
        } 
        #endregion

        #region NavigateCore()
        private async Task<bool> NavigateCore(
            Type sourcePageType, object parameter, NavigationMode navigationMode)
        {
            _isNavigating = true;
            this.CanNavigate = false;
            this.CanGoBack = false;
            this.CanGoForward = false;
            this.IsHitTestVisible = false;

            // Make sure we run on UI thread
            if (!Dispatcher.HasThreadAccess)
            {
                Task<bool> navigateCoreTask = null;
                await Dispatcher.RunAsync(
                    CoreDispatcherPriority.High,
                    () => navigateCoreTask = NavigateCore(sourcePageType, parameter, navigationMode));
                return await navigateCoreTask;
            }

            try
            {
                await _waitForApplyTemplateTaskSource.Task;
                await this.WaitForLoadedAsync();
                AlternativePage currentPage = null;

                #region OnNavigatingFrom part
                if (_currentPagePresenter != null)
                {
                    currentPage = (AlternativePage)_currentPagePresenter.Content;
                    var cancelArgs =
                        new AlternativeNavigatingCancelEventArgs(
                            navigationMode,
                            sourcePageType);
                    await this.OnNavigating(cancelArgs);

                    if (!cancelArgs.Cancel)
                    {
                        await currentPage.OnNavigatingFromInternal(cancelArgs);
                    }

                    if (cancelArgs.Cancel)
                    {
                        return false;
                    }
                }
                #endregion

                #region Initializing new page instance part
                var je = new JournalEntry { SourcePageType = sourcePageType, Parameter = parameter };
                AlternativePage newPage;
                ContentPresenter newPagePresenter;

                if (_preloadedPageCache.ContainsKey(je))
                {
                    newPagePresenter = _preloadedPageCache[je];
                    newPage = (AlternativePage)newPagePresenter.Content;
                    _preloadedPageCache.Remove(je);
                }
                else
                {
                    newPage = Activator.CreateInstance(je.SourcePageType) as AlternativePage;

                    if (newPage == null)
                    {
                        throw new InvalidOperationException(
                            "Pages used in AlternativeFrame need to be of AlternativePage type.");
                    }

                    newPage.Frame = this;
                    newPagePresenter = new ContentPresenter { Style = PagePresenterStyle };
                    newPagePresenter.Content = newPage;
                    _pagePresentersPanel.Children.Add(newPagePresenter);
                }

                newPagePresenter.Opacity = 0.005;

                await UnloadAllPreloaded();
                #endregion

                #region OnNavigatingTo part
                var args = new AlternativeNavigationEventArgs(
                            newPage.Content, navigationMode, je.Parameter, je.SourcePageType);
                await newPage.OnNavigatingToInternal(args);
                #endregion

                #region Journal Bookeeping part
                switch (navigationMode)
                {
                    case NavigationMode.New:
                        this.ForwardStack.Clear();

                        if (this.CurrentJournalEntry != null)
                        {
                            this.BackStack.Push(this.CurrentJournalEntry);
                        }

                        break;
                    case NavigationMode.Forward:
                        this.ForwardStack.Pop();

                        if (this.CurrentJournalEntry != null)
                        {
                            this.BackStack.Push(this.CurrentJournalEntry);
                        }

                        break;
                    case NavigationMode.Back:
                        this.BackStack.Pop();

                        if (this.CurrentJournalEntry != null)
                        {
                            this.ForwardStack.Push(this.CurrentJournalEntry);
                        }

                        break;
                }

                this.CurrentJournalEntry = je;
                #endregion

                #region OnNavigated~() calls
                await this.OnNavigated(args);

                if (currentPage != null)
                {
                    await currentPage.OnNavigatedFromInternal(args);
                }

                await newPage.OnNavigatedToInternal(args);
                #endregion

                #region Transition part
                await newPagePresenter.WaitForLoadedAsync();
                await newPagePresenter.WaitForNonZeroSizeAsync();

                if (this.ShouldWaitForImagesToLoad == true &&
                    newPage.ShouldWaitForImagesToLoad != false ||
                    newPage.ShouldWaitForImagesToLoad == true &&
                    this.ShouldWaitForImagesToLoad != false)
                {
                    await newPage.WaitForImagesToLoad(WaitForImagesToLoadTimeout);
                }

                newPagePresenter.Opacity = 1.0;

                if (navigationMode == NavigationMode.Back)
                {
                    await TransitionBackward(
                        currentPage,
                        newPage,
                        _currentPagePresenter,
                        newPagePresenter);
                }
                else
                {
                    await TransitionForward(
                        currentPage,
                        newPage,
                        _currentPagePresenter,
                        newPagePresenter);
                }

                if (_currentPagePresenter != null)
                {
                    _pagePresentersPanel.Children.Remove(_currentPagePresenter);
                }

                _currentPagePresenter = newPagePresenter;
                #endregion

                return true;
            }
            finally
            {
                this.IsHitTestVisible = true;
                _isNavigating = false;
                this.CanNavigate = true;
                this.CanGoBack = this.BackStack.Count > 0;
                this.CanGoForward = this.ForwardStack.Count > 0;
                //DC.TraceLocalized(GetNavigationState());
            }
        }
        #endregion

        #region OnNavigating()
        private async Task OnNavigating(AlternativeNavigatingCancelEventArgs args)
        {
            AlternativeNavigatingCancelEventHandler handler = this.Navigating;

            if (handler != null)
            {
                await handler(this, args);
            }
        } 
        #endregion

        #region OnNavigated()
        private async Task OnNavigated(AlternativeNavigationEventArgs args)
        {
            AlternativeNavigationEventHandler handler = this.Navigated;

            if (handler != null)
            {
                await handler(this, args);
            }
        } 
        #endregion

        #region TransitionForward()
        private async Task TransitionForward(
            AlternativePage currentPage,
            AlternativePage newPage,
            ContentPresenter previousPagePresenter,
            ContentPresenter newPagePresenter)
        {
            var transition = newPage != null ? (newPage.PageTransition ?? this.PageTransition) : this.PageTransition;

            if (transition != null)
            {
                if (currentPage != null)
                {
                    await currentPage.OnTransitioningFromInternal();
                }

                if (newPage != null)
                {
                    await newPage.OnTransitioningToInternal();
                }

                await transition.TransitionForward(previousPagePresenter, newPagePresenter);

                if (currentPage != null)
                {
                    await currentPage.OnTransitionedFromInternal();
                }

                if (newPage != null)
                {
                    await newPage.OnTransitionedToInternal();
                }
            }
        }
        #endregion

        #region TransitionBackward()
        private async Task TransitionBackward(
            AlternativePage currentPage,
            AlternativePage newPage,
            ContentPresenter previousPagePresenter,
            ContentPresenter newPagePresenter)
        {
            var transition = currentPage != null ? (currentPage.PageTransition ?? this.PageTransition) : this.PageTransition;

            if (transition != null)
            {
                if (currentPage != null)
                {
                    await currentPage.OnTransitioningFromInternal();
                }

                if (newPage != null)
                {
                    await newPage.OnTransitioningToInternal();
                }

                await transition.TransitionBackward(previousPagePresenter, newPagePresenter);

                if (currentPage != null)
                {
                    await currentPage.OnTransitionedFromInternal();
                }

                if (newPage != null)
                {
                    await newPage.OnTransitionedToInternal();
                }
            }
        }
        #endregion
    }
}
