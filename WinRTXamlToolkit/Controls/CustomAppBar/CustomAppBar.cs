using System;
using System.Diagnostics;
using System.Linq;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.Devices.Input;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// AppBar control replacement. To be used anywhere in the visual tree.
    /// The standard AppBar does not do the slide transitions
    /// when not placed in the Page.Bottom/Top-AppBar properties,
    /// which makes it less useful when used e.g. with the AlternativeFrame control.
    /// There are also other cases when it just makes sense to put it elsewhere on screen.
    /// Contains improvements such properties that lock it in opened or closed states,
    /// apart from simply giving you the source code you can tweak to your liking.
    /// </summary>
    [TemplateVisualState(GroupName=FloatingStatesGroupName, Name=FloatingVisibleStateName)]
    [TemplateVisualState(GroupName=FloatingStatesGroupName, Name=FloatingHiddenStateName)]
    [TemplatePart(Name=LayoutRootPartName)]
    [TemplatePart(Name=TransitionTransformPartName, Type=typeof(CompositeTransform))]
    [TemplatePart(Name=FloatingVisibleHorizontalTransitionPartName, Type=typeof(DoubleAnimation))]
    [TemplatePart(Name=FloatingVisibleVerticalTransitionPartName, Type=typeof(DoubleAnimation))]
    [TemplatePart(Name=FloatingHiddenHorizontalTransitionPartName, Type=typeof(DoubleAnimation))]
    [TemplatePart(Name=FloatingHiddenVerticalTransitionPartName, Type=typeof(DoubleAnimation))]
    public class CustomAppBar : ContentControl
    {
        #region Template Part and Visual State names
        private const string FloatingStatesGroupName = "FloatingStates";
        private const string FloatingVisibleStateName = "FloatingVisible";
        private const string FloatingHiddenStateName = "FloatingHidden";
        private const string LayoutRootPartName = "PART_LayoutRoot";
        private const string TransitionTransformPartName = "PART_TransitionTransform";
        private const string FloatingVisibleHorizontalTransitionPartName = "PART_FloatingVisibleHorizontalTransition";
        private const string FloatingVisibleVerticalTransitionPartName = "PART_FloatingVisibleVerticalTransition";
        private const string FloatingHiddenHorizontalTransitionPartName = "PART_FloatingHiddenHorizontalTransition";
        private const string FloatingHiddenVerticalTransitionPartName = "PART_FloatingHiddenVerticalTransition"; 
        #endregion

        private bool _rightMouseButtonPressed;
        private bool _isLoaded;

        private DoubleAnimation _floatingVisibleHorizontalTransition;
        private DoubleAnimation _floatingVisibleVerticalTransition;
        private DoubleAnimation _floatingHiddenHorizontalTransition;
        private DoubleAnimation _floatingHiddenVerticalTransition;
        private CompositeTransform _transitionTransform;
        private FrameworkElement _layoutRoot;

        public event EventHandler<object> Opened;
        public event EventHandler<object> Closed;

        #region IsOpen
        /// <summary>
        /// IsOpen Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(
                "IsOpen",
                typeof(bool),
                typeof(CustomAppBar),
                new PropertyMetadata(false, OnIsOpenChanged));

        /// <summary>
        /// Gets or sets the IsOpen property. This dependency property 
        /// indicates whether the AppBar is visible.
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsOpen property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnIsOpenChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var appBar = (CustomAppBar)d;
            bool oldIsOpen = (bool)e.OldValue;
            bool newIsOpen = appBar.IsOpen;
            appBar.OnIsOpenChanged(oldIsOpen, newIsOpen);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the IsOpen property.
        /// </summary>
        /// <param name="oldIsOpen">The old IsOpen value</param>
        /// <param name="newIsOpen">The new IsOpen value</param>
        protected virtual void OnIsOpenChanged(
            bool oldIsOpen, bool newIsOpen)
        {
            if (!_isLoaded)
            {
                return;
            }

            if (newIsOpen)
            {
                DebugAssertAppBarIsVisible();
                OnOpenedInternal(true);
            }
            else
            {
                OnClosedInternal(true);
            }
        }
        #endregion

        #region DebugAssertAppBarIsVisible()
        [Conditional("DEBUG")]
        private void DebugAssertAppBarIsVisible()
        {
            if (this.Visibility != Visibility.Visible ||
                this.Opacity == 0)
            {
                Debug.Assert(false, "It should not be allowed to toggle an invisible app bar! Use CanOpen property instead of Visibility/Opacity.");

                //if (Debugger.IsAttached)
                //{
                //    Debugger.Break();
                //}
            }

            foreach (var ancestor in this.GetAncestorsOfType<FrameworkElement>())
            {

                if (ancestor.Visibility != Visibility.Visible ||
                    ancestor.Opacity == 0)
                {
                    Debug.Assert(false, "It should not be allowed to toggle an invisible app bar! Use CanOpen property instead of Visibility/Opacity.");

                    //if (Debugger.IsAttached)
                    //{
                    //    Debugger.Break();
                    //}
                }
            }
        }
        #endregion

        #region CanOpen
        /// <summary>
        /// CanOpen Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanOpenProperty =
            DependencyProperty.Register(
                "CanOpen",
                typeof(bool),
                typeof(CustomAppBar),
                new PropertyMetadata(true, OnCanOpenChanged));

        /// <summary>
        /// Gets or sets the CanOpen property. This dependency property 
        /// indicates whether the AppBar can open using the standard gestures.
        /// </summary>
        public bool CanOpen
        {
            get { return (bool)GetValue(CanOpenProperty); }
            set { SetValue(CanOpenProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CanOpen property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCanOpenChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var appBar = (CustomAppBar)d;
            bool oldCanOpen = (bool)e.OldValue;
            bool newCanOpen = appBar.CanOpen;
            appBar.OnCanOpenChanged(oldCanOpen, newCanOpen);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CanOpen property.
        /// </summary>
        /// <param name="oldCanOpen">The old CanOpen value</param>
        /// <param name="newCanOpen">The new CanOpen value</param>
        protected virtual void OnCanOpenChanged(
            bool oldCanOpen, bool newCanOpen)
        {
            if (!newCanOpen)
            {
                IsOpen = false;
            }
        }
        #endregion

        #region CanDismiss
        /// <summary>
        /// CanDismiss Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanDismissProperty =
            DependencyProperty.Register(
                "CanDismiss",
                typeof(bool),
                typeof(CustomAppBar),
                new PropertyMetadata(true, OnCanDismissChanged));

        /// <summary>
        /// Gets or sets the CanDismiss property. This dependency property 
        /// indicates whether the AppBar can be dismissed.
        /// </summary>
        public bool CanDismiss
        {
            get { return (bool)GetValue(CanDismissProperty); }
            set { SetValue(CanDismissProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CanDismiss property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCanDismissChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var appBar = (CustomAppBar)d;
            bool oldCanDismiss = (bool)e.OldValue;
            bool newCanDismiss = appBar.CanDismiss;
            appBar.OnCanDismissChanged(oldCanDismiss, newCanDismiss);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CanDismiss property.
        /// </summary>
        /// <param name="oldCanDismiss">The old CanDismiss value</param>
        /// <param name="newCanDismiss">The new CanDismiss value</param>
        protected virtual void OnCanDismissChanged(
            bool oldCanDismiss, bool newCanDismiss)
        {
            if (!newCanDismiss)
            {
                IsOpen = true;
            }
        }
        #endregion

        #region CanOpenInSnappedView
        /// <summary>
        /// CanOpenInSnappedView Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanOpenInSnappedViewProperty =
            DependencyProperty.Register(
                "CanOpenInSnappedView",
                typeof(bool),
                typeof(CustomAppBar),
                new PropertyMetadata(true, OnCanOpenInSnappedViewChanged));

        /// <summary>
        /// Gets or sets the CanOpenInSnappedView property. This dependency property 
        /// indicates whether the AppBar can be opened in snapped view.
        /// </summary>
        public bool CanOpenInSnappedView
        {
            get { return (bool)GetValue(CanOpenInSnappedViewProperty); }
            set { SetValue(CanOpenInSnappedViewProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CanOpenInSnappedView property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCanOpenInSnappedViewChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var customAppBar = (CustomAppBar)d;
            bool oldCanOpenInSnappedView = (bool)e.OldValue;
            bool newCanOpenInSnappedView = customAppBar.CanOpenInSnappedView;
            customAppBar.OnCanOpenInSnappedViewChanged(oldCanOpenInSnappedView, newCanOpenInSnappedView);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CanOpenInSnappedView property.
        /// </summary>
        /// <param name="oldCanOpenInSnappedView">The old CanOpenInSnappedView value</param>
        /// <param name="newCanOpenInSnappedView">The new CanOpenInSnappedView value</param>
        protected virtual void OnCanOpenInSnappedViewChanged(
            bool oldCanOpenInSnappedView, bool newCanOpenInSnappedView)
        {
            if (!newCanOpenInSnappedView &&
                ApplicationView.Value == ApplicationViewState.Snapped)
            {
                IsOpen = false;
            }
        }
        #endregion

        #region IsLightDismissEnabled
        /// <summary>
        /// IsLightDismissEnabled Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsLightDismissEnabledProperty =
            DependencyProperty.Register(
                "IsLightDismissEnabled",
                typeof(bool),
                typeof(CustomAppBar),
                new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the IsLightDismissEnabled property. This dependency property 
        /// indicates whether the app bar can be dismissed by tapping anywhere outside of the control.
        /// </summary>
        public bool IsLightDismissEnabled
        {
            get { return (bool)GetValue(IsLightDismissEnabledProperty); }
            set { SetValue(IsLightDismissEnabledProperty, value); }
        }
        #endregion

        #region CTOR
        public CustomAppBar()
        {
            this.DefaultStyleKey = typeof(CustomAppBar);
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        } 
        #endregion

        #region OnLoaded()
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;

            //if (IsOpen)
            //{
            //    OnOpenedInternal(true);
            //}

            EdgeGesture.GetForCurrentView().Completed += OnEdgeGestureCompleted;
            Window.Current.SizeChanged += WindowSizeChanged;
            Window.Current.CoreWindow.PointerPressed += OnCoreWindowPointerPressed;
            Window.Current.CoreWindow.PointerReleased += OnCoreWindowPointerReleased;

            if (!IsOpen)
            {
                SetAppBarPositionOutsideClipBounds();
            }
        }
        #endregion

        #region SetAppBarPositionOutsideClipBounds()
        private void SetAppBarPositionOutsideClipBounds()
        {
            if (_transitionTransform == null)
                return;

            if (this.VerticalAlignment == VerticalAlignment.Bottom)
            {
                _transitionTransform.TranslateY = this.ActualHeight;
            }
            else if (this.VerticalAlignment == VerticalAlignment.Top)
            {
                _transitionTransform.TranslateY = -this.ActualHeight;
            }
            if (this.HorizontalAlignment == HorizontalAlignment.Left)
            {
                _transitionTransform.TranslateX = -this.ActualWidth;
            }
            else if (this.HorizontalAlignment == HorizontalAlignment.Right)
            {
                _transitionTransform.TranslateX = this.ActualWidth;
            }
        } 
        #endregion

        #region OnUnloaded()
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = false;
            //IsOpen = false; // TODO: Find a better solution for this workaround for an issue with the light dismiss popup staying visible after the user navigates to another page by clicking a button on the app bar
            //if (IsOpen)
            //{
            //    OnClosedInternal(true);
            //}

            EdgeGesture.GetForCurrentView().Completed -= OnEdgeGestureCompleted;
            Window.Current.SizeChanged -= WindowSizeChanged;
            Window.Current.CoreWindow.PointerPressed -= OnCoreWindowPointerPressed;
            Window.Current.CoreWindow.PointerReleased -= OnCoreWindowPointerReleased;
        } 
        #endregion

        #region OnClosedInternal()
        private void OnClosedInternal(bool useTransitions)
        {
            GoToFloatingHiddenVisualState(useTransitions);

            OnClosed(null);

            if (this.Closed != null)
            {
                this.Closed(this, null);
            }
        } 
        #endregion

        #region OnOpenedInternal()
        internal void OnOpenedInternal(bool useTransitions)
        {
            GoToFloatingVisibleVisualState(useTransitions);

            OnOpened(null);

            if (this.Opened != null)
                this.Opened(this, null);
        }
        #endregion

        #region OnClosed()
        protected virtual void OnClosed(object e)
        {
        } 
        #endregion

        #region OnOpened()
        protected virtual void OnOpened(object e)
        {
        } 
        #endregion

        #region OnEdgeGestureCompleted()
        private void OnEdgeGestureCompleted(EdgeGesture sender, EdgeGestureEventArgs args)
        {
            OnSwitchGesture();
        } 
        #endregion

        #region WindowSizeChanged()
        private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            if (ApplicationView.Value == ApplicationViewState.Snapped &&
                !CanOpenInSnappedView)
            {
                this.IsOpen = false;
            }
        } 
        #endregion

        #region OnCoreWindowPointerPressed()
        private void OnCoreWindowPointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            if (this.IsLightDismissEnabled &&
                this.CanDismiss && 
                this.IsOpen &&
                Window.Current != null &&
                Window.Current.Content != null)
            {
                var windowToAppBarTransform = Window.Current.Content.TransformToVisual(this);
                var appBarPosition = windowToAppBarTransform.TransformPoint(args.CurrentPoint.Position);
                var appBarBounds = this.GetBoundingRect(this);

                if (!appBarBounds.Contains(appBarPosition))
                {
                    this.IsOpen = false;
                    return;
                }
            }

            if (args.CurrentPoint.PointerDevice.PointerDeviceType == PointerDeviceType.Mouse)
            {
                _rightMouseButtonPressed =
                    args.CurrentPoint.Properties.IsRightButtonPressed &&
                    !args.CurrentPoint.Properties.IsLeftButtonPressed &&
                    !args.CurrentPoint.Properties.IsMiddleButtonPressed;

                if (_rightMouseButtonPressed)
                {
                    args.Handled = true;
                }
            }
        } 
        #endregion

        #region OnCoreWindowPointerReleased()
        private void OnCoreWindowPointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            if (args.CurrentPoint.PointerDevice.PointerDeviceType == PointerDeviceType.Mouse &&
                !args.CurrentPoint.Properties.IsLeftButtonPressed &&
                !args.CurrentPoint.Properties.IsMiddleButtonPressed &&
                _rightMouseButtonPressed)
            {
                OnSwitchGesture();
                args.Handled = true;
            }

            _rightMouseButtonPressed = false;
        } 
        #endregion

        #region OnApplyTemplate()
        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _transitionTransform = GetTemplateChild(TransitionTransformPartName) as CompositeTransform;
            _layoutRoot = GetTemplateChild(LayoutRootPartName) as FrameworkElement;

            if (_layoutRoot == null)
                return;

            var visualStateGroups = VisualStateManager.GetVisualStateGroups(_layoutRoot);

            if (visualStateGroups == null)
                return;

            var floatingStatesGroup = visualStateGroups.FirstOrDefault(group => group.Name == FloatingStatesGroupName);

            if (floatingStatesGroup == null)
                return;

            var floatingVisibleState =
                floatingStatesGroup.States.FirstOrDefault(
                    state => state.Name == FloatingVisibleStateName);

            if (floatingVisibleState != null &&
                floatingVisibleState.Storyboard != null)
            {
                _floatingVisibleHorizontalTransition =
                    floatingVisibleState.Storyboard.Children.FirstOrDefault(
                        timeline =>
                            Storyboard.GetTargetName(timeline) == TransitionTransformPartName &&
                            Storyboard.GetTargetProperty(timeline) == "TranslateX") as DoubleAnimation;
                _floatingVisibleVerticalTransition =
                    floatingVisibleState.Storyboard.Children.FirstOrDefault(
                        timeline =>
                            Storyboard.GetTargetName(timeline) == TransitionTransformPartName &&
                            Storyboard.GetTargetProperty(timeline) == "TranslateY") as DoubleAnimation;
            }

            var floatingHiddenState =
                floatingStatesGroup.States.FirstOrDefault(
                    state => state.Name == FloatingHiddenStateName);

            if (floatingHiddenState != null &&
                floatingHiddenState.Storyboard != null)
            {
                _floatingHiddenHorizontalTransition =
                    floatingHiddenState.Storyboard.Children.FirstOrDefault(
                        timeline =>
                            Storyboard.GetTargetName(timeline) == TransitionTransformPartName &&
                            Storyboard.GetTargetProperty(timeline) == "TranslateX") as DoubleAnimation;
                _floatingHiddenVerticalTransition =
                    floatingHiddenState.Storyboard.Children.FirstOrDefault(
                        timeline =>
                            Storyboard.GetTargetName(timeline) == TransitionTransformPartName &&
                            Storyboard.GetTargetProperty(timeline) == "TranslateY") as DoubleAnimation;
            }

            //this.FloatingVisibleHorizontalTransition = GetTemplateChild(FloatingVisibleHorizontalTransitionPartName) as DoubleAnimation;
            //this.FloatingVisibleVerticalTransition = GetTemplateChild(FloatingVisibleVerticalTransitionPartName) as DoubleAnimation;
            //this.FloatingHiddenHorizontalTransition = GetTemplateChild(FloatingVisibleHorizontalTransitionPartName) as DoubleAnimation;
            //this.FloatingHiddenVerticalTransition = GetTemplateChild(FloatingVisibleVerticalTransitionPartName) as DoubleAnimation;

            if (IsOpen)
            {
                OnOpenedInternal(false);
            }
            else
            {
                OnClosedInternal(false);
            }
        } 
        #endregion

        #region OnSwitchGesture()
        private void OnSwitchGesture()
        {
            if (IsOpen)
            {
                if (CanDismiss)
                {
                    IsOpen = false;
                }
            }
            else
            {
                if (CanOpen &&
                    (CanOpenInSnappedView || ApplicationView.Value != ApplicationViewState.Snapped))
                {
                    IsOpen = true;
                }
            }
        } 
        #endregion

        #region GoToFloatingHiddenVisualState()
        private void GoToFloatingHiddenVisualState(bool useTransitions)
        {
            if (this.VerticalAlignment == VerticalAlignment.Bottom &&
                _floatingHiddenVerticalTransition != null)
            {
                _floatingHiddenVerticalTransition.To = this.ActualHeight;
            }
            else if (this.VerticalAlignment == VerticalAlignment.Top &&
                _floatingHiddenVerticalTransition != null)
            {
                _floatingHiddenVerticalTransition.To = -this.ActualHeight;
            }
            else if (this.HorizontalAlignment == HorizontalAlignment.Left &&
                _floatingHiddenHorizontalTransition != null)
            {
                _floatingHiddenHorizontalTransition.To = -this.ActualWidth;
            }
            else if (this.HorizontalAlignment == HorizontalAlignment.Right &&
                _floatingHiddenHorizontalTransition != null)
            {
                _floatingHiddenHorizontalTransition.To = this.ActualWidth;
            }

            VisualStateManager.GoToState(this, FloatingHiddenStateName, useTransitions);
        } 
        #endregion

        #region GoToFloatingVisibleVisualState()
        private void GoToFloatingVisibleVisualState(bool useTransitions)
        {
            SetAppBarPositionOutsideClipBounds();

            if (_floatingVisibleVerticalTransition != null)
            {
                _floatingVisibleVerticalTransition.To = 0;
            }

            if (_floatingVisibleHorizontalTransition != null)
            {
                _floatingVisibleHorizontalTransition.To = 0;
            }

            VisualStateManager.GoToState(this, FloatingVisibleStateName, useTransitions);
        } 
        #endregion
    }
}
