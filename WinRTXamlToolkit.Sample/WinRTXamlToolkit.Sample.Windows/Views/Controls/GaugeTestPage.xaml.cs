using WinRTXamlToolkit.Sample.Common;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class GaugeTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public GaugeTestPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }

    public class GaugePageViewModel : BindableBase
    {
        private int _size = 200;
        private double _value;

        public int Size
        {
            get { return _size; }
            set { this.SetProperty(ref _size, value); }
        }

        public double Value
        {
            get { return _value; }
            set { this.SetProperty(ref _value, value); }
        }
    }
}
