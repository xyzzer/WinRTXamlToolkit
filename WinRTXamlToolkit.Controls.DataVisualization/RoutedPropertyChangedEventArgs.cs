using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls.DataVisualization.Charting
{
    class RoutedPropertyChangedEventArgs:RoutedEventArgs
    {
        public object OldValue;
        public object NewValue;

        public RoutedPropertyChangedEventArgs(object OldValue, object NewValue)
        {
            this.OldValue = OldValue;
            this.NewValue = NewValue;
        }
    }
}
