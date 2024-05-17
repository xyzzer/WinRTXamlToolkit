using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using System.Windows.Input;

namespace WinRTXamlToolkit.StylesBrowser.Controls.ListItemButton
{
    /// <summary>
    /// A button replacement for use in lists. Allows items to be selected with touch while still supporting clicks and commands.
    /// </summary>
    public class ListItemButton : ContentControl
    {
        #region Command
        /// <summary>
        /// Command Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(ListItemButton),
                new PropertyMetadata(null, OnCommandChanged));

        /// <summary>
        /// Gets or sets the Command property. This dependency property 
        /// indicates the command to execute when the button gets tapped.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Command property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCommandChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ListItemButton)d;
            ICommand oldCommand = (ICommand)e.OldValue;
            ICommand newCommand = target.Command;
            target.OnCommandChanged(oldCommand, newCommand);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Command property.
        /// </summary>
        /// <param name="oldCommand">The old Command value</param>
        /// <param name="newCommand">The new Command value</param>
        protected virtual void OnCommandChanged(
            ICommand oldCommand, ICommand newCommand)
        {
        }
        #endregion

        #region CommandParameter
        /// <summary>
        /// CommandParameter Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                "CommandParameter",
                typeof(object),
                typeof(ListItemButton),
                new PropertyMetadata(null, OnCommandParameterChanged));

        /// <summary>
        /// Gets or sets the CommandParameter property. This dependency property 
        /// indicates the command parameter.
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CommandParameter property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCommandParameterChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ListItemButton)d;
            object oldCommandParameter = e.OldValue;
            object newCommandParameter = target.CommandParameter;
            target.OnCommandParameterChanged(oldCommandParameter, newCommandParameter);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CommandParameter property.
        /// </summary>
        /// <param name="oldCommandParameter">The old CommandParameter value</param>
        /// <param name="newCommandParameter">The new CommandParameter value</param>
        protected virtual void OnCommandParameterChanged(
            object oldCommandParameter, object newCommandParameter)
        {
        }
        #endregion

        /// <summary>
        /// Occurs when the button is clicked.
        /// </summary>
        public event RoutedEventHandler Click;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListItemButton"/> class.
        /// </summary>
        public ListItemButton()
        {
            DefaultStyleKey = typeof(ListItemButton);
        }

        /// <summary>
        /// Called before the Tapped event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);

            if (FlyoutBase.GetAttachedFlyout(this) != null)
            {
                FlyoutBase.ShowAttachedFlyout(this);
                return;
            }

            if (Click != null)
                Click(this, new RoutedEventArgs());

            if (Command != null &&
                Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
        }

        /// <summary>
        /// Called before the ManipulationStarting event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnManipulationStarting(ManipulationStartingRoutedEventArgs e)
        {
            //base.OnManipulationStarting(e);
        }

        /// <summary>
        /// Called before the ManipulationStarted event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {
            //base.OnManipulationStarted(e);
        }
    }
}
