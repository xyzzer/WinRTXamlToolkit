using WinRTXamlToolkit.Debugging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class CustomGridSplitterTestView : UserControl
    {
        public CustomGridSplitterTestView()
        {
            this.InitializeComponent();
            cgs1.DraggingCompleted += cgs1_DraggingCompleted;
            cgs2.DraggingCompleted += cgs2_DraggingCompleted;
            cgs3.DraggingCompleted += cgs3_DraggingCompleted;
            cgs4.DraggingCompleted += cgs4_DraggingCompleted;
        }

        void cgs4_DraggingCompleted(object sender, System.EventArgs e)
        {
            if (cbTrace.IsChecked.Value)
            {
                DC.TraceLocalized();
            }
        }

        void cgs3_DraggingCompleted(object sender, System.EventArgs e)
        {
            if (cbTrace.IsChecked.Value)
            {
                DC.TraceLocalized();
            }
        }

        void cgs2_DraggingCompleted(object sender, System.EventArgs e)
        {
            if (cbTrace.IsChecked.Value)
            {
                DC.TraceLocalized();
            }
        }

        void cgs1_DraggingCompleted(object sender, System.EventArgs e)
        {
            if (cbTrace.IsChecked.Value)
            {
                DC.TraceLocalized();
            }
        }
    }
}
