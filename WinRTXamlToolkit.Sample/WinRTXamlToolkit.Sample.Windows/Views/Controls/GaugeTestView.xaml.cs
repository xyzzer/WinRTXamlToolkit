using WinRTXamlToolkit.Sample.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class GaugeTestView : UserControl
    {
        public GaugeTestView()
        {
            this.InitializeComponent();
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
