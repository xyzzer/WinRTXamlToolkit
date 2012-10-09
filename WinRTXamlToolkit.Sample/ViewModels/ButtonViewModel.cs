using System.Windows.Input;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Sample.ViewModels
{
    public class ButtonViewModel : ViewModel
    {
        #region Command
        private ICommand _command;
        /// <summary>
        /// Gets or sets the command executed by a button click.
        /// </summary>
        public ICommand Command
        {
            get { return _command; }
            set { this.SetProperty(ref _command, value); }
        }
        #endregion

        #region CommandParameter
        private object _commandParameter;
        /// <summary>
        /// Gets or sets the command parameter.
        /// </summary>
        public object CommandParameter
        {
            get { return _commandParameter; }
            set { this.SetProperty(ref _commandParameter, value); }
        }
        #endregion

        #region Caption
        private string _caption;
        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        public string Caption
        {
            get { return _caption; }
            set { this.SetProperty(ref _caption, value); }
        }
        #endregion

        #region Image
        private ImageSource _image;
        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        public ImageSource Image
        {
            get { return _image; }
            set { this.SetProperty(ref _image, value); }
        }
        #endregion
    }
}
