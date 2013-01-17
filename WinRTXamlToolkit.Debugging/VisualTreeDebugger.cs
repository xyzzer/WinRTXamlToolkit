#if DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Debugging
{
    /// <summary>
    /// The class contains an BreakOnLoaded property that
    /// allows to debug a visual tree from the control
    /// it was applied to up to the visual tree root.
    /// </summary>
    /// <remarks>
    /// Just put a breakpoint in ControlLoaded()
    /// and set VisualTreeDebugger.BreakOnLoaded="True"
    /// on any FrameworkElement/Control.
    /// <para/>
    /// Set BreakOnLoaded if you want to put the breakpoint yourself.
    /// <para/>
    /// DebugVisualTree method has a local path variable
    /// that contains a list of all elements from the visual tree root
    /// up to the debugged control.
    /// <para/>
    /// Debug Output window contains common visual tree layout properties
    /// helpful in debugging layout issues.
    /// </remarks>
    public class VisualTreeDebugger
    {
        #region BreakOnLoaded
        /// <summary>
        /// BreakOnLoaded BreakOnLoaded Dependency Property
        /// </summary>
        public static readonly DependencyProperty BreakOnLoadedProperty =
            DependencyProperty.RegisterAttached(
                "BreakOnLoaded",
                typeof(bool),
                typeof(VisualTreeDebugger),
                new PropertyMetadata(false, OnBreakOnLoadedChanged));

        /// <summary>
        /// Gets the BreakOnLoaded property. This dependency property
        /// indicates whether the debugger should BreakOnLoaded when control is loaded.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <returns>
        /// the boolean value
        /// </returns>
        public static bool GetBreakOnLoaded(DependencyObject d)
        {
            return (bool)d.GetValue(BreakOnLoadedProperty);
        }

        /// <summary>
        /// Sets the BreakOnLoaded property. This dependency property
        /// indicates whether the debugger should BreakOnLoaded when control is loaded.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <param name="value">
        /// If set to <c>true</c> the attached debugger will break when the control
        /// loads.
        /// </param>
        public static void SetBreakOnLoaded(DependencyObject d, bool value)
        {
            d.SetValue(BreakOnLoadedProperty, value);
        }

        /// <summary>
        /// Called when [break on loaded changed].
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <param name="e">
        /// The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void OnBreakOnLoadedChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ((FrameworkElement)d).Loaded += BreakOnControlLoaded;
            }
            else
            {
                ((FrameworkElement)d).Loaded -= BreakOnControlLoaded;
            }
        }
        #endregion

        #region BreakOnTap
        /// <summary>
        /// BreakOnTap Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty BreakOnTapProperty =
            DependencyProperty.RegisterAttached(
                "BreakOnTap",
                typeof(bool),
                typeof(VisualTreeDebugger),
                new PropertyMetadata(false, OnBreakOnTapChanged));

        /// <summary>
        /// Gets the BreakOnTap property. This dependency property
        /// indicates whether the attached debugger should break when
        /// the FrameworkElement on which this property is set is tapped.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <returns>
        /// The boolean value
        /// </returns>
        public static bool GetBreakOnTap(DependencyObject d)
        {
            return (bool)d.GetValue(BreakOnTapProperty);
        }

        /// <summary>
        /// Sets the BreakOnTap property. This dependency property
        /// indicates whether the attached debugger should break when
        /// the FrameworkElement on which this property is set is tapped.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <param name="value">if set to <c>true</c>
        ///  [value].
        /// </param>
        public static void SetBreakOnTap(DependencyObject d, bool value)
        {
            d.SetValue(BreakOnTapProperty, value);
        }

        /// <summary>
        /// Handles changes to the BreakOnTap property.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <param name="e">
        /// The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void OnBreakOnTapChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = d as FrameworkElement;

            Debug.Assert(
                frameworkElement != null,
                "BreakOnTapProperty should only be set on FrameworkElements.");

            if ((bool)e.NewValue)
            {
                ((FrameworkElement)d).Tapped += BreakOnControlTapped;
            }
            else
            {
                ((FrameworkElement)d).Tapped -= BreakOnControlTapped;
            }
        }
        #endregion

        #region BreakOnLayoutUpdated
        /// <summary>
        /// BreakOnLayoutUpdated Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty BreakOnLayoutUpdatedProperty =
            DependencyProperty.RegisterAttached(
                "BreakOnLayoutUpdated",
                typeof(bool),
                typeof(VisualTreeDebugger),
                new PropertyMetadata(false, OnBreakOnLayoutUpdatedChanged));

        /// <summary>
        /// Gets the BreakOnLayoutUpdated property. This dependency property
        /// indicates whether the attached debugger should break when
        /// the FrameworkElement on which this property is set has its layout updated.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <returns>
        /// The boolean value
        /// </returns>
        public static bool GetBreakOnLayoutUpdated(DependencyObject d)
        {
            return (bool)d.GetValue(BreakOnLayoutUpdatedProperty);
        }

        /// <summary>
        /// Sets the BreakOnLayoutUpdated property. This dependency property
        /// indicates whether the attached debugger should break when
        /// the FrameworkElement on which this property is set has its layout updated.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <param name="value">if set to <c>true</c>
        ///  [value].
        /// </param>
        public static void SetBreakOnLayoutUpdated(DependencyObject d, bool value)
        {
            d.SetValue(BreakOnLayoutUpdatedProperty, value);
        }

        /// <summary>
        /// Handles changes to the BreakOnLayoutUpdated property.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <param name="e">
        /// The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void OnBreakOnLayoutUpdatedChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = d as FrameworkElement;

            Debug.Assert(
                frameworkElement != null,
                "BreakOnLayoutUpdatedProperty should only be set on FrameworkElements.");

            frameworkElement.LayoutUpdated += (s, o) =>
            {
                DebugVisualTree(frameworkElement);
            };
        }
        #endregion

        #region BreakDelay
        /// <summary>
        /// BreakDelay Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty BreakDelayProperty =
            DependencyProperty.RegisterAttached(
                "BreakDelay",
                typeof(double),
                typeof(VisualTreeDebugger),
                new PropertyMetadata(0d));

        /// <summary>
        /// Gets the BreakDelay property. This dependency property 
        /// indicates the delay in seconds to wait after the trigger is fired before breaking in debugger.
        /// </summary>
        public static double GetBreakDelay(DependencyObject d)
        {
            return (double)d.GetValue(BreakDelayProperty);
        }

        /// <summary>
        /// Sets the BreakDelay property. This dependency property 
        /// indicates the delay in seconds to wait after the trigger is fired before breaking in debugger.
        /// </summary>
        public static void SetBreakDelay(DependencyObject d, double value)
        {
            d.SetValue(BreakDelayProperty, value);
        }
        #endregion

        #region BreakOnControlLoaded()
        /// <summary>
        /// Called when the control gets loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="RoutedEventArgs"/> instance containing
        /// the event data.</param>
        private static async void BreakOnControlLoaded(object sender, RoutedEventArgs e)
        {
            var startElement = (DependencyObject)sender;
            var delay = GetBreakDelay(startElement);
            if (delay > 0)
            {
                await Task.Delay((int)(delay * 1000));
            }
            DebugVisualTree(startElement);
        }
        #endregion

        #region BreakOnControlTapped()
        /// <summary>
        /// Called when the control gets tapped or clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.GestureEventArgs"/> instance
        /// containing the event data.</param>
        private static async void BreakOnControlTapped(object sender, TappedRoutedEventArgs e)
        {
            var startElement = (DependencyObject)sender;
            var delay = GetBreakDelay(startElement);
            if (delay > 0)
            {
                await Task.Delay((int)(delay * 1000));
            }
            DebugVisualTree(startElement);
        }
        #endregion

        #region BreakOnControlLayoutUpdated()
        /// <summary>
        /// Called when the control's layout updates.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.GestureEventArgs"/> instance
        /// containing the event data.</param>
        private static async void BreakOnControlLayoutUpdated(object sender, object e)
        {
            var startElement = (DependencyObject)sender;
            var delay = GetBreakDelay(startElement);
            if (delay > 0)
            {
                await Task.Delay((int)(delay * 1000));
            }
            DebugVisualTree(startElement);
        }
        #endregion

        #region TraceOnLoaded
        /// <summary>
        /// TraceOnLoaded TraceOnLoaded Dependency Property
        /// </summary>
        public static readonly DependencyProperty TraceOnLoadedProperty =
            DependencyProperty.RegisterAttached(
                "TraceOnLoaded",
                typeof(bool),
                typeof(VisualTreeDebugger),
                new PropertyMetadata(false, OnTraceOnLoadedChanged));

        /// <summary>
        /// Gets the TraceOnLoaded property. This dependency property
        /// indicates whether the debugger should TraceOnLoaded when control is loaded.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <returns>
        /// the boolean value
        /// </returns>
        public static bool GetTraceOnLoaded(DependencyObject d)
        {
            return (bool)d.GetValue(TraceOnLoadedProperty);
        }

        /// <summary>
        /// Sets the TraceOnLoaded property. This dependency property
        /// indicates whether the debugger should TraceOnLoaded when control is loaded.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <param name="value">
        /// If set to <c>true</c> the attached debugger will Trace when the control
        /// loads.
        /// </param>
        public static void SetTraceOnLoaded(DependencyObject d, bool value)
        {
            d.SetValue(TraceOnLoadedProperty, value);
        }

        /// <summary>
        /// Called when [Trace on loaded changed].
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <param name="e">
        /// The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void OnTraceOnLoadedChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ((FrameworkElement)d).Loaded += TraceOnControlLoaded;
            }
            else
            {
                ((FrameworkElement)d).Loaded -= TraceOnControlLoaded;
            }
        }
        #endregion

        #region TraceOnTap
        /// <summary>
        /// TraceOnTap Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty TraceOnTapProperty =
            DependencyProperty.RegisterAttached(
                "TraceOnTap",
                typeof(bool),
                typeof(VisualTreeDebugger),
                new PropertyMetadata(false, OnTraceOnTapChanged));

        /// <summary>
        /// Gets the TraceOnTap property. This dependency property
        /// indicates whether the attached debugger should Trace when
        /// the FrameworkElement on which this property is set is tapped.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <returns>
        /// The boolean value
        /// </returns>
        public static bool GetTraceOnTap(DependencyObject d)
        {
            return (bool)d.GetValue(TraceOnTapProperty);
        }

        /// <summary>
        /// Sets the TraceOnTap property. This dependency property
        /// indicates whether the attached debugger should Trace when
        /// the FrameworkElement on which this property is set is tapped.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <param name="value">if set to <c>true</c>
        ///  [value].
        /// </param>
        public static void SetTraceOnTap(DependencyObject d, bool value)
        {
            d.SetValue(TraceOnTapProperty, value);
        }

        /// <summary>
        /// Handles changes to the TraceOnTap property.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <param name="e">
        /// The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void OnTraceOnTapChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = d as FrameworkElement;

            Debug.Assert(
                frameworkElement != null,
                "TraceOnTapProperty should only be set on FrameworkElements.");

            if ((bool)e.NewValue)
            {
                ((FrameworkElement)d).Tapped += TraceOnControlTapped;
            }
            else
            {
                ((FrameworkElement)d).Tapped -= TraceOnControlTapped;
            }
        }
        #endregion

        #region TraceOnLayoutUpdated
        /// <summary>
        /// TraceOnLayoutUpdated Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty TraceOnLayoutUpdatedProperty =
            DependencyProperty.RegisterAttached(
                "TraceOnLayoutUpdated",
                typeof(bool),
                typeof(VisualTreeDebugger),
                new PropertyMetadata(false, OnTraceOnLayoutUpdatedChanged));

        /// <summary>
        /// Gets the TraceOnLayoutUpdated property. This dependency property
        /// indicates whether the attached debugger should Trace when
        /// the FrameworkElement on which this property is set has its layout updated.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <returns>
        /// The boolean value
        /// </returns>
        public static bool GetTraceOnLayoutUpdated(DependencyObject d)
        {
            return (bool)d.GetValue(TraceOnLayoutUpdatedProperty);
        }

        /// <summary>
        /// Sets the TraceOnLayoutUpdated property. This dependency property
        /// indicates whether the attached debugger should Trace when
        /// the FrameworkElement on which this property is set has its layout updated.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <param name="value">if set to <c>true</c>
        ///  [value].
        /// </param>
        public static void SetTraceOnLayoutUpdated(DependencyObject d, bool value)
        {
            d.SetValue(TraceOnLayoutUpdatedProperty, value);
        }

        /// <summary>
        /// Handles changes to the TraceOnLayoutUpdated property.
        /// </summary>
        /// <param name="d">
        /// The DependencyObject.
        /// </param>
        /// <param name="e">
        /// The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void OnTraceOnLayoutUpdatedChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = d as FrameworkElement;

            Debug.Assert(
                frameworkElement != null,
                "TraceOnLayoutUpdatedProperty should only be set on FrameworkElements.");

            frameworkElement.LayoutUpdated += (s, o) =>
            {
                DebugVisualTree(frameworkElement);
            };
        }
        #endregion

        #region TraceOnControlLoaded()
        /// <summary>
        /// Called when the control gets loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing
        /// the event data.</param>
        private static void TraceOnControlLoaded(object sender, RoutedEventArgs e)
        {
            var startElement = (DependencyObject)sender;
            DebugVisualTree(startElement, false);
        }
        #endregion

        #region TraceOnControlTapped()
        /// <summary>
        /// Called when the control gets tapped or clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.GestureEventArgs"/> instance
        /// containing the event data.</param>
        private static void TraceOnControlTapped(object sender, TappedRoutedEventArgs e)
        {
            var startElement = (DependencyObject)sender;
            DebugVisualTree(startElement, false);
        }
        #endregion

        #region TraceOnControlLayoutUpdated()
        /// <summary>
        /// Called when the control's layout updates.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.GestureEventArgs"/> instance
        /// containing the event data.</param>
        private static void TraceOnControlLayoutUpdated(object sender, object e)
        {
            var startElement = (DependencyObject)sender;
            DebugVisualTree(startElement, false);
        }
        #endregion

        #region DebugVisualTree()
        /// <summary>
        /// Debugs the visual tree.
        /// </summary>
        /// <param name="startElement">
        /// The start element.
        /// </param>
        public static void DebugVisualTree(DependencyObject startElement, bool breakInDebugger = true)
        {
            if (DesignMode.DesignModeEnabled)
                return;

            var path = new List<DependencyObject>();
            var dob = startElement;

            while (dob != null)
            {
                path.Add(dob);
                dob = VisualTreeHelper.GetParent(dob);
            }

            for (int i = path.Count - 1; i >= 0; i--)
            {
                TraceDependencyObject(path[i], i);
            }

            // Put breakpoint here
            Debug.WriteLine(
                string.Format(
                    "Watch path[0] to path[{0}]",
                    path.Count - 1));

            if (breakInDebugger && Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
        #endregion

        #region TraceDependencyObject()
        /// <summary>
        /// Traces the dependency object.
        /// </summary>
        /// <param name="dob">
        /// The dependancy object.
        /// </param>
        /// <param name="i">
        /// The object index.
        /// </param>
        private static void TraceDependencyObject(DependencyObject dob, int i)
        {
            var frameworkElement = dob as FrameworkElement;

            if (frameworkElement == null)
            {
                Debug.WriteLine(
                    "path[{0}] is Dependency Object: {1}",
                    i,
                    dob.GetType());
            }
            else
            {
                var c = frameworkElement as Control;
                var cc = frameworkElement as ContentControl;
                var panel = frameworkElement as Panel;
                var parentGrid = frameworkElement.Parent as Grid;
                var image = frameworkElement as Image;
                var scrollViewer = frameworkElement as ScrollViewer;

                Debug.WriteLine(
                    "path[{0}] is Control: {1}({2}):",
                    i,
                    frameworkElement.GetType(),
                    frameworkElement.Name ?? "<unnamed>");

                // Actual layout information
                Debug.WriteLine(
                    "\tActualWidth={0}\r\n\tActualHeight={1}",
                    frameworkElement.ActualWidth,
                    frameworkElement.ActualHeight);
                var pos =
                    frameworkElement
                        .TransformToVisual(Window.Current.Content)
                        .TransformPoint(new Point());
                var pos2 =
                    frameworkElement
                        .TransformToVisual(Window.Current.Content)
                        .TransformPoint(
                            new Point(
                                frameworkElement.ActualWidth,
                                frameworkElement.ActualHeight));

                Debug.WriteLine(
                    "\tPosition – X={0}, Y={1}, Right={2}, Bottom={3}",
                    pos.X,
                    pos.Y,
                    pos2.X,
                    pos2.Y);

                if (frameworkElement.Opacity < 1.0)
                {
                    Debug.WriteLine("\tOpacity={0}", frameworkElement.Opacity);
                }

                if (frameworkElement.Visibility != Visibility.Visible)
                {
                    Debug.WriteLine("\tVisibility={0}", frameworkElement.Visibility);
                }

                if (frameworkElement.Clip != null)
                {
                    Debug.WriteLine("\tClip={0}", frameworkElement.Clip.Rect);
                }

                // DataContext often turns out to be a surprise
                Debug.WriteLine(
                    "\tDataContext: {0} {1}",
                    frameworkElement.DataContext,
                    frameworkElement.DataContext != null
                        ? "HashCode: " + frameworkElement.DataContext.GetHashCode()
                        : "");

                // List common layout properties
                if (!double.IsNaN(frameworkElement.Width) ||
                    !double.IsNaN(frameworkElement.Height))
                {
                    Debug.WriteLine(
                        "\tWidth={0}\r\n\tHeight={1}",
                        frameworkElement.Width,
                        frameworkElement.Height);
                }

                if (scrollViewer != null)
                {
                    Debug.WriteLine(
                        "\tScrollViewer.HorizontalOffset={0}\r\n\tScrollViewer.ViewportWidth={1}\r\n\tScrollViewer.ExtentWidth={2}\r\n\tScrollViewer.VerticalOffset={3}\r\n\tScrollViewer.ViewportHeight={4}\r\n\tScrollViewer.ExtentHeight={5}",
                        scrollViewer.HorizontalOffset,
                        scrollViewer.ViewportWidth,
                        scrollViewer.ExtentWidth,
                        scrollViewer.VerticalOffset,
                        scrollViewer.ViewportHeight,
                        scrollViewer.ExtentHeight
                        );
                }

                if (frameworkElement.MinWidth > 0 ||
                    frameworkElement.MinHeight > 0 ||
                    !double.IsInfinity(frameworkElement.MaxWidth) ||
                    !double.IsInfinity(frameworkElement.MaxHeight))
                {
                    Debug.WriteLine(
                        "\tMinWidth={0}\r\n\tMaxWidth={1}\r\n\tMinHeight={2}\r\n\tMaxHeight={3}",
                        frameworkElement.MinWidth,
                        frameworkElement.MaxWidth,
                        frameworkElement.MinHeight,
                        frameworkElement.MaxHeight);
                }

                Debug.WriteLine(
                    "\tHorizontalAlignment={0}\r\n\tVerticalAlignment={1}",
                    frameworkElement.HorizontalAlignment,
                    frameworkElement.VerticalAlignment);

                if (cc != null)
                {
                    Debug.WriteLine(
                        "\tHorizontalContentAlignment={0}\r\n\tVerticalContentAlignment={1}\r\n\tContent={2}",
                        cc.HorizontalContentAlignment,
                        cc.VerticalContentAlignment,
                        cc.Content ?? "<null>");
                }

                Debug.WriteLine(
                    "\tMargins={0},{1},{2},{3}",
                    frameworkElement.Margin.Left,
                    frameworkElement.Margin.Top,
                    frameworkElement.Margin.Right,
                    frameworkElement.Margin.Bottom);

                if (cc != null)
                {
                    Debug.WriteLine("\tPadding={0}", cc.Padding);
                }

                if (panel != null)
                {
                    Debug.WriteLine("\tBackground={0}", BrushToString(panel.Background));
                }
                else if (c != null)
                {
                    Debug.WriteLine("\tBackground={0}", BrushToString(c.Background));
                    Debug.WriteLine("\tForeground={0}", BrushToString(c.Foreground));
                }

                if (parentGrid != null)
                {
                    var col = Grid.GetColumn(frameworkElement);
                    var row = Grid.GetRow(frameworkElement);

                    if (parentGrid.ColumnDefinitions.Count != 0 || col != 0)
                    {
                        Debug.Assert(
                            col < parentGrid.ColumnDefinitions.Count,
                            string.Format(
                                "Column {0} not defined on the parent Grid!", col));
                        col = Math.Min(col, parentGrid.ColumnDefinitions.Count - 1);
                        Debug.WriteLine(
                            "\tColumn: {0} ({1})",
                            col,
                            parentGrid.ColumnDefinitions[col].Width);
                    }

                    if (parentGrid.RowDefinitions.Count != 0 || row != 0)
                    {
                        Debug.Assert(
                            row < parentGrid.RowDefinitions.Count,
                            string.Format(
                                "Row {0} not defined on the parent Grid!", row));
                        row = Math.Min(row, parentGrid.RowDefinitions.Count - 1);
                        Debug.WriteLine(
                            "\tRow: {0} ({1})",
                            row,
                            parentGrid.RowDefinitions[row].Height);
                    }
                }

                if (c != null)
                {
                    Debug.WriteLine("\tFontFamily: {0}", c.FontFamily.Source);
                }

                if (image != null)
                {
                    Debug.WriteLine("\tImage\n  Source: {0}", image.Source);

                    var bs = image.Source as BitmapSource;
                    var bi = image.Source as BitmapImage;

                    if (bi != null)
                    {
                        Debug.WriteLine("\t\tSource.UriSource: {0}", bi.UriSource.OriginalString);
                    }

                    if (bs != null)
                    {
                        Debug.WriteLine("\t\tPixelWidth: {0}", bs.PixelWidth);
                        Debug.WriteLine("\t\tPixelHeight: {0}", bs.PixelHeight);
                    }
                }

                if (frameworkElement.Parent is Canvas)
                {
                    var x = Canvas.GetLeft(frameworkElement);
                    var y = Canvas.GetTop(frameworkElement);
                    var zIndex = Canvas.GetZIndex(frameworkElement);

                    Debug.WriteLine(
                        "\tCanvas – X={0}, Y={1}, ZIndex={2}", x, y, zIndex);
                }
            }
        }
        #endregion

        #region BrushToString()
        /// <summary>
        /// Brushes to string.
        /// </summary>
        /// <param name="brush">
        /// The brush.
        /// </param>
        /// <returns>
        /// brush type
        /// </returns>
        private static string BrushToString(Brush brush)
        {
            if (brush == null)
            {
                return "";
            }

            var solidColorBrush = brush as SolidColorBrush;

            if (solidColorBrush != null)
            {
                return string.Format("SolidColorBrush: {0}", solidColorBrush.Color);
            }

            var imageBrush = brush as ImageBrush;

            if (imageBrush != null)
            {
                var bi = imageBrush.ImageSource as BitmapImage;

                if (bi != null)
                {
                    return string.Format(
                        "ImageBrush: {0}, UriSource: {1}",
                        bi,
                        bi.UriSource);
                }

                return string.Format("ImageBrush: {0}", imageBrush.ImageSource);
            }

            return brush.ToString();
        }
        #endregion
    }
}

#endif