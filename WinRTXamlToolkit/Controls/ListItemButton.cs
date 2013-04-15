using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
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
            get { return (object)GetValue(CommandParameterProperty); }
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
            object oldCommandParameter = (object)e.OldValue;
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

        public event RoutedEventHandler Click;

        public ListItemButton()
        {
            this.DefaultStyleKey = typeof(ListItemButton);
        }

        protected override void OnTapped(Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            base.OnTapped(e);

            if (Click != null)
                Click(this, new RoutedEventArgs());

            if (Command != null &&
                Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
        }

        protected override void OnManipulationStarting(Windows.UI.Xaml.Input.ManipulationStartingRoutedEventArgs e)
        {
            //base.OnManipulationStarting(e);
        }

        protected override void OnManipulationStarted(Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {
            //base.OnManipulationStarted(e);
        }
    }
}
