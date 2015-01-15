using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using WinRTXamlToolkit.Input;

namespace WinRTXamlToolkit.Controls
{
    public sealed class ToolBarToggleButton : ToggleButton, IToolStripElement
    {
        private const string PlacedInBarStateName = "PlacedInBar";
        private const string PlacedInDropDownStateName = "PlacedInDropDown";

        private ToggleButtonAutomationPeer peer;
        private KeyGesture keyGesture;
        private KeyGestureRecognizer keyGestureRecognizer;

        #region Shortcut
        /// <summary>
        /// Shortcut Dependency Property
        /// </summary>
        private static readonly DependencyProperty _ShortcutProperty =
            DependencyProperty.Register(
                "Shortcut",
                typeof(string),
                typeof(ToolBarToggleButton),
                new PropertyMetadata(null, OnShortcutChanged));

        /// <summary>
        /// Identifies the Shortcut dependency property.
        /// </summary>
        public static DependencyProperty ShortcutProperty { get { return _ShortcutProperty; } }

        /// <summary>
        /// Gets or sets the keyboard shortcut specified with a string representing a keyboard gesture that KeyGesture can parse.
        /// </summary>
        public string Shortcut
        {
            get { return (string)this.GetValue(ShortcutProperty); }
            set { this.SetValue(ShortcutProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Shortcut property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnShortcutChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolBarToggleButton)d;
            string oldShortcut = (string)e.OldValue;
            string newShortcut = target.Shortcut;
            target.OnShortcutChanged(oldShortcut, newShortcut);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Shortcut property.
        /// </summary>
        /// <param name="oldShortcut">The old Shortcut value</param>
        /// <param name="newShortcut">The new Shortcut value</param>
        private void OnShortcutChanged(
            string oldShortcut, string newShortcut)
        {
            AutomationProperties.SetAcceleratorKey(this, newShortcut);

            if (this.keyGestureRecognizer != null)
            {
                this.keyGestureRecognizer.GestureRecognized -= this.OnKeyGestureRecognized;
                this.keyGestureRecognizer.Dispose();
            }

            this.keyGesture = string.IsNullOrEmpty(newShortcut) ? null : KeyGesture.Parse(newShortcut);

            if (this.keyGesture != null)
            {
                this.keyGestureRecognizer = new KeyGestureRecognizer(this.keyGesture);
                this.keyGestureRecognizer.GestureRecognized += this.OnKeyGestureRecognized;
            }

            this.UpdateToolTip();
        }

        private void OnKeyGestureRecognized(object sender, EventArgs e)
        {
            if (this.IsTabStop)
            {
                this.Focus(FocusState.Programmatic);
            }

            this.Toggle();
        }
        #endregion

        #region IsInDropDown
        private bool _isInDropDown;

        public bool IsInDropDown
        {
            get
            {
                return _isInDropDown;
            }
            set
            {
                if (_isInDropDown != value)
                {
                    _isInDropDown = value;
                    this.UpdateVisualStates(true);
                    this.UpdateToolTip();
                }
            }
        } 
        #endregion

        #region CTOR - ToolBarToggleButton()
        public ToolBarToggleButton()
        {
            this.DefaultStyleKey = typeof(ToolBarToggleButton);

            this.peer = new ToggleButtonAutomationPeer(this);
        } 
        #endregion

        #region UpdateVisualStates()
        private void UpdateVisualStates(bool useTransitions)
        {
            if (this.IsInDropDown)
            {
                VisualStateManager.GoToState(this, PlacedInDropDownStateName, useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, PlacedInBarStateName, useTransitions);
            }
        } 
        #endregion

        #region UpdateToolTip()
        /// <summary>
        /// Updates the tooltip.
        /// </summary>
        private void UpdateToolTip()
        {
            if (this.IsInDropDown || string.IsNullOrEmpty(this.Shortcut))
            {
                ToolTipService.SetToolTip(this, null);
            }
            else
            {
                ToolTipService.SetToolTip(this, string.Format("({0})", this.Shortcut));
            }
        }
        #endregion

        #region OnApplyTemplate()
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.UpdateVisualStates(false);
        }
        #endregion

        #region OnCreateAutomationPeer()
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return peer;
        } 
        #endregion

        #region Toggle()
        internal void Toggle()
        {
            this.peer.Toggle();
        } 
        #endregion
    }
}
