using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.WinRTProxy
{
    public static class DC
    {
        public static void Hide()
        {
            DebugConsoleOverlay.Hide();
        }

        public static void Show()
        {
            DebugConsoleOverlay.Show();
        }

        public static void ShowLog()
        {
            DebugConsoleOverlay.ShowLog();
        }

        public static void ShowVisualTree(UIElement element)
        {
            DebugConsoleOverlay.ShowVisualTree(element);
        }

        public static void ShowVisualTree()
        {
            DebugConsoleOverlay.ShowVisualTree(null);
        }

        public static void Expand()
        {
            DebugConsoleOverlay.Expand();
        }

        public static void Collapse()
        {
            DebugConsoleOverlay.Collapse();
        }
    }
}
