using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class WatermarkPasswordBoxTestView : UserControl
    {
        public WatermarkPasswordBoxTestView()
        {
            this.InitializeComponent();
            ////wpb.DataContext = new WatermarkPasswordBoxTestViewModel();
            ////wpb.SetBinding(
            ////    WatermarkPasswordBox.PasswordProperty,
            ////    new Binding
            ////    {
            ////        Path = new PropertyPath("Password"),
            ////        Mode = BindingMode.TwoWay
            ////    });
            ////wpb.SetBinding(
            ////    WatermarkPasswordBox.IsPasswordRevealButtonEnabledProperty,
            ////    new Binding
            ////    {
            ////        Path = new PropertyPath("IsPasswordRevealButtonEnabled"),
            ////        Mode = BindingMode.TwoWay
            ////    });
        }
    }

    ////public class WatermarkPasswordBoxTestViewModel : BindableBase
    ////{
    ////    #region Password
    ////    private string _password = string.Empty;
    ////    public string Password
    ////    {
    ////        get { return _password; }
    ////        set { this.SetProperty(ref _password, value); }
    ////    }
    ////    #endregion

    ////    #region PasswordChar
    ////    private string _passwordChar;
    ////    public string PasswordChar
    ////    {
    ////        get { return _passwordChar; }
    ////        set { this.SetProperty(ref _passwordChar, value); }
    ////    }
    ////    #endregion

    ////    #region IsPasswordRevealButtonEnabled
    ////    private bool _isPasswordRevealButtonEnabled = true;
    ////    public bool IsPasswordRevealButtonEnabled
    ////    {
    ////        get { return _isPasswordRevealButtonEnabled; }
    ////        set { this.SetProperty(ref _isPasswordRevealButtonEnabled, value); }
    ////    }
    ////    #endregion

    ////    #region MaxLength
    ////    private int _maxLength;
    ////    public int MaxLength
    ////    {
    ////        get { return _maxLength; }
    ////        set { this.SetProperty(ref _maxLength, value); }
    ////    }
    ////    #endregion

        

    ////    public WatermarkPasswordBoxTestViewModel()
    ////    {
    ////        FiddleWithProperties();
    ////    }

    ////    private async void FiddleWithProperties()
    ////    {
    ////        int i = 0;

    ////        while (true)
    ////        {
    ////            await Task.Delay(1000);
    ////            await Window.Current.Dispatcher.RunAsync(
    ////                CoreDispatcherPriority.Normal,
    ////                () =>
    ////                {
    ////                    IsPasswordRevealButtonEnabled = !IsPasswordRevealButtonEnabled;
    ////                    i++;

    ////                    if (i % 5 == 0 &&
    ////                        Password.Length > 0)
    ////                    {
    ////                        Password = Password.Substring(0, Password.Length - 1);
    ////                    }
    ////                });
    ////        }
    ////    }
    ////}
}
