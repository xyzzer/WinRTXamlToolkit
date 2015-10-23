using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Debugging.Views;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using WinRTXamlToolkit.IO.Extensions;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Debugging
{
    /// <summary>
    /// Provides helpers for debugging WinRT XAML applications - for tracing and visual tree debugging.
    /// </summary>
    public class DebugConsoleOverlay
    {
        #region Instance
        private static DebugConsoleOverlay _instance;
        private static DebugConsoleOverlay Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DebugConsoleOverlay();
                    _instance.Initialize();
                    _instance._popup.Opacity = 0;
                    _instance._popup.IsHitTestVisible = false;
                }

                return _instance;
            }
        }
        #endregion

        internal static FrameworkElement View
        {
            get
            {
                return Instance._debugConsoleView;
            }
        }

        private DebugConsoleView _debugConsoleView;
        private Popup _popup;

        private Thickness _instanceMargin;
        private Thickness InstanceMargin
        {
            get
            {
                return _instanceMargin;
            }
            set
            {
                _instanceMargin = value;

                if (_instanceMargin == new Thickness())
                {
                    FrameworkElementExtensions.SetClipToBounds(_debugConsoleView, false);
                }
                else
                {
                    FrameworkElementExtensions.SetClipToBounds(_debugConsoleView, true);
                }

                this.UpdateLayout();
            }
        }

        public static Thickness Margin
        {
            get
            {
                return Instance.InstanceMargin;
            }
            set
            {
                Instance.InstanceMargin = value;
            }
        }

        private void Initialize()
        {
            _debugConsoleView = new DebugConsoleView();
            _popup = new Popup
            {
                Child = _debugConsoleView,
            };

            var panel = Window.Current.Content.GetFirstDescendantOfType<Panel>();

            if (panel != null)
            {
                panel.Children.Add(_popup);
            }

            _popup.IsOpen = true;

            UpdateLayout();
            Window.Current.SizeChanged += this.OnWindowSizeChanged;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="DebugConsoleOverlay"/> class from being created.
        /// </summary>
        private DebugConsoleOverlay()
        {
        }

        public static void Show()
        {
            Instance._popup.IsOpen = true;
            Instance._popup.Opacity = 1;
            Instance._popup.IsHitTestVisible = true;
        }

        public static void ShowLog()
        {
            Show();
            _instance._debugConsoleView.ShowLog();
        }

        public static async Task ShowVisualTreeAsync(UIElement element = null)
        {
            Show();
            await _instance._debugConsoleView.Expand();
            await _instance._debugConsoleView.ShowVisualTreeAsync(element);
        }

        //public static void Collapse()
        //{
        //    Instance._popup.IsOpen = false;
        //}

        public static void Hide()
        {
            Instance._popup.Opacity = 0;
            Instance._popup.IsHitTestVisible = false;
        }

        public static async void Expand()
        {
            await Instance._debugConsoleView.Expand();
        }

        public static async void Collapse()
        {
            await Instance._debugConsoleView.Collapse();
        }

        private void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs windowSizeChangedEventArgs)
        {
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            _popup.HorizontalOffset = _instanceMargin.Left;
            _popup.VerticalOffset = _instanceMargin.Top;
            _debugConsoleView.Width = Window.Current.Bounds.Width - _instanceMargin.Left - _instanceMargin.Right;
            _debugConsoleView.Height = Window.Current.Bounds.Height - _instanceMargin.Top - _instanceMargin.Bottom;
        }

        public static void Trace(string format, params object[] args)
        {
            Instance.TraceInternal(format, args);
        }

        public static void Trace(string message)
        {
            Instance.TraceInternal(message);
        }

        public static void Clear()
        {
            Instance.ClearInternal();
        }

        private void TraceInternal(string format, object[] args)
        {
            try
            {
                var line =
                    string.Format(
                        "{0} - {1}\n",
                        DateTime.Now.ToString("HH:mm:ss"),
                        string.Format(
                            format,
                            args));
                _debugConsoleView.Append(line);
            }
            catch (FormatException)
            {
                var line =
                    string.Format(
                        "{0} - {1}\n",
                        DateTime.Now.ToString("HH:mm:ss"),
                        string.Format(
                            format.Replace("{", "{{").Replace("}", "}}"),
                            args));
                _debugConsoleView.Append(line);
            }
        }

        private void TraceInternal(string message)
        {
            var line =
                string.Format(
                    "{0} - {1}\n",
                    DateTime.Now.ToString("HH:mm:ss"),
                    message);
            _debugConsoleView.Append(line);
        }

        private void ClearInternal()
        {
            _debugConsoleView.Clear();
        }
    }

    /// <summary>
    /// Essentially an alias for DebugConsoleOverlay
    /// </summary>
    public static class DC
    {
        private static DateTime _previousTraceIntervalTimeStamp = DateTime.Now;

        public static Thickness Margin
        {
            get
            {
                return DebugConsoleOverlay.Margin;
            }
            set
            {
                DebugConsoleOverlay.Margin = value;
            }
        }

        public static void TraceLocalized(
            string message = "Checkpoint",
            [CallerMemberName] string methodName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            DebugConsoleOverlay.Trace(
                string.Format(
                    "File: {0},\tMethod: {1},\tLine: {2} - {3}",
                    filePath,
                    methodName,
                    lineNumber,
                    message));
        }

        public static void TraceLocalized(
            object value,
            [CallerMemberName] string methodName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            DebugConsoleOverlay.Trace(
                string.Format(
                    "File: {0},\tMethod: {1},\tLine: {2} - {3}",
                    filePath,
                    methodName,
                    lineNumber,
                    (value ?? "<null>").ToString()));
        }

        public static void TraceMemoryCheckPoint(
            string message,
            [CallerMemberName] string methodName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            GC.Collect();
            string memoryUsage = System.GC.GetTotalMemory(true).GetSizeString();
            DebugConsoleOverlay.Trace(
                string.Format(
                    "{4} - Memory: {3} at File: {0},\tMethod: {1},\tLine: {2}",
                    filePath,
                    methodName,
                    lineNumber,
                    memoryUsage,
                    message));
        }

        public static void Trace(string format, params object[] args)
        {
            DebugConsoleOverlay.Trace(format, args);
        }

        public static void Trace(string message)
        {
            DebugConsoleOverlay.Trace(message);
        }

        public static void Trace(object value)
        {
            DebugConsoleOverlay.Trace((value ?? "<null>").ToString());
        }

        public static void TraceInterval(string message = null)
        {
            var now = DateTime.Now;
            var delay = now - _previousTraceIntervalTimeStamp;
            _previousTraceIntervalTimeStamp = now;

            if (message == null)
            {
                Trace(
                    string.Format(
                        "{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                        delay.Hours,
                        delay.Minutes,
                        delay.Seconds,
                        delay.Milliseconds));
            }
            else
            {
                Trace(
                    "{0} - {1}",
                    string.Format(
                        "{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                        delay.Hours,
                        delay.Minutes,
                        delay.Seconds,
                        delay.Milliseconds),
                    message);
            }
        }

        public static void Clear()
        {
            DebugConsoleOverlay.Clear();
        }

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

        public static Task ShowVisualTreeAsync(UIElement element = null)
        {
            return DebugConsoleOverlay.ShowVisualTreeAsync(element);
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
