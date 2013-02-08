using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Controls.Extensions
{
    public static class ButtonBaseExtensions
    {
        #region ButtonStateEventBehavior
        /// <summary>
        /// ButtonStateEventBehavior Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ButtonStateEventBehaviorProperty =
            DependencyProperty.RegisterAttached(
                "ButtonStateEventBehavior",
                typeof(ButtonStateEventBehavior),
                typeof(ButtonBaseExtensions),
                new PropertyMetadata(null, OnButtonStateEventBehaviorChanged));

        /// <summary>
        /// Gets the ButtonStateEventBehavior property. This dependency property 
        /// indicates the behavior that allows to handle button Up/Down event handling.
        /// </summary>
        public static ButtonStateEventBehavior GetButtonStateEventBehavior(DependencyObject d)
        {
            return (ButtonStateEventBehavior)d.GetValue(ButtonStateEventBehaviorProperty);
        }

        /// <summary>
        /// Sets the ButtonStateEventBehavior property. This dependency property 
        /// indicates the behavior that allows to handle button Up/Down event handling.
        /// </summary>
        public static void SetButtonStateEventBehavior(DependencyObject d, ButtonStateEventBehavior value)
        {
            d.SetValue(ButtonStateEventBehaviorProperty, value);
        }

        /// <summary>
        /// Handles changes to the ButtonStateEventBehavior property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnButtonStateEventBehaviorChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ButtonStateEventBehavior oldButtonStateEventBehavior = (ButtonStateEventBehavior)e.OldValue;
            ButtonStateEventBehavior newButtonStateEventBehavior = (ButtonStateEventBehavior)d.GetValue(ButtonStateEventBehaviorProperty);

            if (oldButtonStateEventBehavior != null)
            {
                oldButtonStateEventBehavior.Detach();
            }
            if (newButtonStateEventBehavior != null)
            {
                newButtonStateEventBehavior.Attach((ButtonBase)d);
            }
        }
        #endregion
    }

    public class ButtonStateEventBehavior : FrameworkElement
    {
        private ButtonBase _button;

        public event EventHandler Up;
        public event EventHandler Down;

        #region IsPressed
        /// <summary>
        /// IsPressed Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsPressedProperty =
            DependencyProperty.Register(
                "IsPressed",
                typeof(bool),
                typeof(ButtonStateEventBehavior),
                new PropertyMetadata(false, OnIsPressedChanged));

        /// <summary>
        /// Gets or sets the IsPressed property. This dependency property 
        /// indicates whether the button is up or down.
        /// </summary>
        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            set { SetValue(IsPressedProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsPressed property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnIsPressedChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ButtonStateEventBehavior)d;
            bool oldIsPressed = (bool)e.OldValue;
            bool newIsPressed = target.IsPressed;
            target.OnIsPressedChanged(oldIsPressed, newIsPressed);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the IsPressed property.
        /// </summary>
        /// <param name="oldIsPressed">The old IsPressed value</param>
        /// <param name="newIsPressed">The new IsPressed value</param>
        private void OnIsPressedChanged(
            bool oldIsPressed, bool newIsPressed)
        {
            if (newIsPressed)
            {
                OnDownInternal();
            }
            else
            {
                OnUpInternal();
            }
        }
        #endregion

        #region UpCommand
        /// <summary>
        /// UpCommand Dependency Property
        /// </summary>
        public static readonly DependencyProperty UpCommandProperty =
            DependencyProperty.Register(
                "UpCommand",
                typeof(ICommand),
                typeof(ButtonStateEventBehavior),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the UpCommand property. This dependency property 
        /// indicates the command to execute when the button gets released.
        /// </summary>
        public ICommand UpCommand
        {
            get { return (ICommand)GetValue(UpCommandProperty); }
            set { SetValue(UpCommandProperty, value); }
        }
        #endregion

        #region UpCommandParameter
        /// <summary>
        /// UpCommandParameter Dependency Property
        /// </summary>
        public static readonly DependencyProperty UpCommandParameterProperty =
            DependencyProperty.Register(
                "UpCommandParameter",
                typeof(object),
                typeof(ButtonStateEventBehavior),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the UpCommandParameter property. This dependency property 
        /// indicates the parameter for UpCommand.
        /// </summary>
        public object UpCommandParameter
        {
            get { return (object)GetValue(UpCommandParameterProperty); }
            set { SetValue(UpCommandParameterProperty, value); }
        }
        #endregion

        #region DownCommand
        /// <summary>
        /// DownCommand Dependency Property
        /// </summary>
        public static readonly DependencyProperty DownCommandProperty =
            DependencyProperty.Register(
                "DownCommand",
                typeof(ICommand),
                typeof(ButtonStateEventBehavior),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the DownCommand property. This dependency property 
        /// indicates the command to execute when the button gets pressed.
        /// </summary>
        public ICommand DownCommand
        {
            get { return (ICommand)GetValue(DownCommandProperty); }
            set { SetValue(DownCommandProperty, value); }
        }
        #endregion

        #region DownCommandParameter
        /// <summary>
        /// DownCommandParameter Dependency Property
        /// </summary>
        public static readonly DependencyProperty DownCommandParameterProperty =
            DependencyProperty.Register(
                "DownCommandParameter",
                typeof(object),
                typeof(ButtonStateEventBehavior),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the DownCommandParameter property. This dependency property 
        /// indicates the parameter to use with DownCommand.
        /// </summary>
        public object DownCommandParameter
        {
            get { return (object)GetValue(DownCommandParameterProperty); }
            set { SetValue(DownCommandParameterProperty, value); }
        }
        #endregion

        public void Attach(ButtonBase button)
        {
            Detach();

            if (button == null)
            {
                return;
            }

            _button = button;
            SetBinding(
                IsPressedProperty,
                new Binding
                {
                    Source = button,
                    Path = new PropertyPath("IsPressed")
                });
        }

        public void Detach()
        {
            if (_button == null)
            {
                return;
            }

            _button = null;

            ClearValue(IsPressedProperty);
        }

        protected virtual void OnUp()
        {
        }

        protected virtual void OnDown()
        {
        }

        private void OnUpInternal()
        {
            OnUp();

            var handler = Up;

            if (handler != null)
            {
                handler(_button, EventArgs.Empty);
            }

            if (UpCommand != null &&
                UpCommand.CanExecute(UpCommandParameter))
            {
                UpCommand.Execute(UpCommandParameter);
            }
        }

        private void OnDownInternal()
        {
            OnDown();

            var handler = Down;

            if (handler != null)
            {
                handler(_button, EventArgs.Empty);
            }

            if (DownCommand != null &&
                DownCommand.CanExecute(DownCommandParameter))
            {
                DownCommand.Execute(DownCommandParameter);
            }
        }
    }
}
