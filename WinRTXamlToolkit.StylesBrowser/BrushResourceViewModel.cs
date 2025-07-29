using Microsoft.UI.Xaml.Media;

namespace WinRTXamlToolkit.StylesBrowser
{
    public class BrushResourceViewModel
    {
        public Brush Brush { get; private set; }
        public string Key { get; private set; }
        public string Source { get; private set; }

        public BrushResourceViewModel(Brush brush, string key, string source)
        {
            Brush = brush;
            Key = key;
            Source = source;
        }
    }
}
