using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Controls.Common;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Tools;
using FrameworkElementExtensions = WinRTXamlToolkit.Controls.Extensions.FrameworkElementExtensions;

namespace WinRTXamlToolkit.Controls
{
    #region Enums
    /// <summary>
    /// Defines options for where a window should show up.
    /// </summary>
    public enum WindowStartupLocation
    {
        /// <summary>
        /// The startup location of a ToolWindow is set from code, or defers to the default Windows location.
        /// </summary>
        Manual,
        /// <summary>
        /// The startup location of a ToolWindow is the center of the ToolWindow that owns it, as specified by the ToolWindow.Owner property.
        /// </summary>
        CenterOwner,
        /// <summary>
        /// The startup location of a ToolWindow is the center of the screen that contains the mouse cursor.
        /// </summary>
        CenterScreen
    }

    /// <summary>
    /// Options for snapping the window to the edges of its movable area.
    /// </summary>
    public enum WindowEdgeSnapBehavior
    {
        /// <summary>
        /// The window should not snap to the edges.
        /// </summary>
        None,
        /// <summary>
        /// The window snaps straight to its movable area edge with no rotation.
        /// </summary>
        Straight,
        /// <summary>
        /// The window snaps straight to its movable area edge showing just the title bar.
        /// </summary>
        StraightToTitleBar,
        /// <summary>
        /// The window snaps straight to its movable area edge rotating to show the title bar.
        /// </summary>
        ToTitleBarWithRotation
    }

    /// <summary>
    /// Options for selecting what is the movable area for the window.
    /// </summary>
    public enum WindowMovableArea
    {
        /// <summary>
        /// Use parent's boundaries as the movable area,
        /// where parent doesn't necessarily mean direct parent in the visual tree,
        /// but rather the first ancestor of non-zero size.
        /// </summary>
        UseParentBounds,
        /// <summary>
        /// Use application window boundaries as movable area.
        /// </summary>
        UseAppWindowBounds,
    }

    /// <summary>
    /// Defines possible window states.
    /// </summary>
    public enum WindowStates
    {
        /// <summary>
        /// The normal state.
        /// </summary>
        Normal,
        /// <summary>
        /// The window is minimized.
        /// </summary>
        Minimized,
        /// <summary>
        /// The window is maximized.
        /// </summary>
        Maximized,
        /// <summary>
        /// The window is snapped to the edge of its movable area.
        /// </summary>
        Snapped
    }
    #endregion

    #region struct DoublePoint
    /// <summary>
    /// Defines a point specified with double precision coordinates.
    /// Note that a Point struct specifies its dimensions using double precision X and Y coordinates,
    /// but the backing native struct that is projected to CLR is using single precision float values.
    /// </summary>
    public struct DoublePoint
    {
        /// <summary>
        /// The X coordinate.
        /// </summary>
        public double X;
        /// <summary>
        /// The Y coordinate.
        /// </summary>
        public double Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoublePoint"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public DoublePoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
    #endregion

    #region delegate CancelEventHandler
    /// <summary>
    /// The handler for events that support cancellation.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event arguments.</param>
    public delegate void CancelEventHandler(object sender, CancelEventArgs e);
    #endregion

    /// <summary>
    /// Defines a draggable window control that can be hosted in the XAML visual tree.
    /// </summary>
    #region Template parts
    [TemplatePart(Name = LayoutGridName, Type = typeof(Grid))]
    [TemplatePart(Name = SizingGridName, Type = typeof(Grid))]
    [TemplatePart(Name = TitleGridName, Type = typeof(Grid))]
    [TemplatePart(Name = BorderName, Type = typeof(Border))]
    [TemplatePart(Name = ButtonsPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = CloseButtonName, Type = typeof(Button))]
    [TemplatePart(Name = SnapButtonName, Type = typeof(Button))]
    [TemplatePart(Name = RestoreButtonName, Type = typeof(Button))]
    [TemplatePart(Name = MinimizeButtonName, Type = typeof(Button))]
    [TemplatePart(Name = MaximizeButtonName, Type = typeof(Button))]
    [TemplateVisualState(GroupName = "SnapButtonStates", Name = "SnapButtonVisible")]
    [TemplateVisualState(GroupName = "SnapButtonStates", Name = "SnapButtonCollapsed")]
    [TemplateVisualState(GroupName = "RestoreButtonStates", Name = "RestoreButtonVisible")]
    [TemplateVisualState(GroupName = "RestoreButtonStates", Name = "RestoreButtonCollapsed")]
    //[TemplateVisualState(GroupName = "MinimizeButtonStates", Name = "MinimizeButtonVisible")]
    //[TemplateVisualState(GroupName = "MinimizeButtonStates", Name = "MinimizeButtonCollapsed")]
    [TemplateVisualState(GroupName = "MaximizeButtonStates", Name = "MaximizeButtonVisible")]
    [TemplateVisualState(GroupName = "MaximizeButtonStates", Name = "MaximizeButtonCollapsed")]
    [TemplateVisualState(GroupName = "CloseButtonStates", Name = "CloseButtonVisible")]
    [TemplateVisualState(GroupName = "CloseButtonStates", Name = "CloseButtonCollapsed")]
    #endregion
    public class ToolWindow : ContentControl
    {
        #region Fields
        private const string LayoutGridName = "PART_LayoutGrid";
        private const string SizingGridName = "PART_SizingGrid";
        private const string TitleGridName = "PART_TitleGrid";
        private const string BorderName = "PART_Border";
        private const string ButtonsPanelName = "PART_ButtonsPanel";
        private const string CloseButtonName = "PART_CloseButton";
        private const string SnapButtonName = "PART_SnapButton";
        private const string RestoreButtonName = "PART_RestoreButton";
        private const string MinimizeButtonName = "PART_MinimizeButton";
        private const string MaximizeButtonName = "PART_MaximizeButton";

        private Grid _layoutGrid;
        private Grid _sizingGrid;
        private Grid _titleGrid;
        private Border _border;
        private StackPanel _buttonsPanel;
        private Button _closeButton;
        private Button _snapButton;
        private Button _restoreButton;
        private Button _minimizeButton;
        private Button _maximizeButton;
        private CompositeTransform _layoutGridTransform;
        private CompositeTransform _titleTransform;
        private Storyboard _currentSnapStoryboard;
        private Rectangle _topLeftSizingThumb;
        private Rectangle _topCenterSizingThumb;
        private Rectangle _topRightSizingThumb;
        private Rectangle _centerLeftSizingThumb;
        private Rectangle _centerRightSizingThumb;
        private Rectangle _bottomLeftSizingThumb;
        private Rectangle _bottomCenterSizingThumb;
        private Rectangle _bottomRightSizingThumb;

        private FrameworkElement _parent;

        private bool _isAdjustedFlick;
        private bool _isFlickTooLong;
        private bool _isDraggingFromSnapped;

        private double _flickStartX;
        private double _flickStartY;
        private double _flickStartCumulativeX;
        private double _flickStartCumulativeY;
        private double _flickStartAngle;
        private double _naturalFlickDisplacementX;
        private double _naturalFlickDisplacementY;
        private double _flickAdjustedEndX;
        private double _flickAdjustedEndY;
        private double _flickAdjustedEndAngle;
        private DoublePoint _lastWindowPosition;
        private double _lastWindowWidth;
        private double _lastWindowHeight;
        private Edges _lastSnapEdge;
        private bool _restorePositionOnStateChange = true;
        #endregion

        #region Events
        /// <summary>
        /// Occurs directly after ToolWindow.Close() is called, and can be handled to cancel window closure.
        /// </summary>
        public event CancelEventHandler Closing;

        /// <summary>
        /// Raises the closing event.
        /// </summary>
        /// <returns>True if cancellation was requested.</returns>
        private bool RaiseClosing()
        {
            var handler = this.Closing;

            if (handler != null)
            {
                var args = new CancelEventArgs(false);
                handler(this, args);
                return args.Cancel;
            }

            return false;
        }

        /// <summary>
        /// Occurs when the window is about to close.
        /// </summary>
        public event EventHandler Closed;

        private void RaiseClosed()
        {
            var handler = this.Closed;

            if (handler != null)
            {
                var args = EventArgs.Empty;
                handler(this, args);
            }
        }
        #endregion

        #region Properties
        #region Title
        /// <summary>
        /// Title Dependency Property
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(object),
                typeof(ToolWindow),
                new PropertyMetadata(null, OnTitleChanged));

        /// <summary>
        /// Gets or sets the Title shown in the title bar.
        /// </summary>
        public object Title
        {
            get { return (object)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Title property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnTitleChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolWindow)d;
            object oldTitle = (object)e.OldValue;
            object newTitle = target.Title;
            target.OnTitleChanged(oldTitle, newTitle);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Title property.
        /// </summary>
        /// <param name="oldTitle">The old Title value</param>
        /// <param name="newTitle">The new Title value</param>
        protected virtual void OnTitleChanged(
            object oldTitle, object newTitle)
        {
        }
        #endregion

        #region TitleTemplate
        /// <summary>
        /// TitleTemplate Dependency Property
        /// </summary>
        public static readonly DependencyProperty TitleTemplateProperty =
            DependencyProperty.Register(
                "TitleTemplate",
                typeof(DataTemplate),
                typeof(ToolWindow),
                new PropertyMetadata(null, OnTitleTemplateChanged));

        /// <summary>
        /// Gets or sets the Title template.
        /// </summary>
        public DataTemplate TitleTemplate
        {
            get { return (DataTemplate)GetValue(TitleTemplateProperty); }
            set { SetValue(TitleTemplateProperty, value); }
        }

        /// <summary>
        /// Handles changes to the TitleTemplate property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnTitleTemplateChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolWindow)d;
            DataTemplate oldTitleTemplate = (DataTemplate)e.OldValue;
            DataTemplate newTitleTemplate = target.TitleTemplate;
            target.OnTitleTemplateChanged(oldTitleTemplate, newTitleTemplate);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the TitleTemplate property.
        /// </summary>
        /// <param name="oldTitleTemplate">The old TitleTemplate value</param>
        /// <param name="newTitleTemplate">The new TitleTemplate value</param>
        protected virtual void OnTitleTemplateChanged(
            DataTemplate oldTitleTemplate, DataTemplate newTitleTemplate)
        {
        }
        #endregion

        #region TitleBackgroundBrush
        /// <summary>
        /// TitleBackgroundBrush Dependency Property
        /// </summary>
        public static readonly DependencyProperty TitleBackgroundBrushProperty =
            DependencyProperty.Register(
                "TitleBackgroundBrush",
                typeof(Brush),
                typeof(ToolWindow),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the TitleBackgroundBrush property. This dependency property 
        /// indicates the brush used in the background of the title area of the window.
        /// </summary>
        public Brush TitleBackgroundBrush
        {
            get { return (Brush)GetValue(TitleBackgroundBrushProperty); }
            set { this.SetValue(TitleBackgroundBrushProperty, value); }
        }
        #endregion

        #region X
        /// <summary>
        /// X Dependency Property
        /// </summary>
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(
                "X",
                typeof(double),
                typeof(ToolWindow),
                new PropertyMetadata(0d, OnXChanged));

        /// <summary>
        /// Gets or sets the X coordinate of the Window.
        /// </summary>
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        /// <summary>
        /// Handles changes to the X property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnXChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolWindow)d;
            double oldX = (double)e.OldValue;
            double newX = target.X;
            target.OnXChanged(oldX, newX);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the X property.
        /// </summary>
        /// <param name="oldX">The old X value</param>
        /// <param name="newX">The new X value</param>
        protected virtual void OnXChanged(
            double oldX, double newX)
        {
            if (_layoutGridTransform != null)
            {
                _layoutGridTransform.TranslateX = newX;
            }
        }
        #endregion

        #region Y
        /// <summary>
        /// Y Dependency Property
        /// </summary>
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register(
                "Y",
                typeof(double),
                typeof(ToolWindow),
                new PropertyMetadata(0d, OnYChanged));

        /// <summary>
        /// Gets or sets the Y coordinate of the window.
        /// </summary>
        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Y property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnYChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolWindow)d;
            double oldY = (double)e.OldValue;
            double newY = target.Y;
            target.OnYChanged(oldY, newY);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Y property.
        /// </summary>
        /// <param name="oldY">The old Y value</param>
        /// <param name="newY">The new Y value</param>
        protected virtual void OnYChanged(
            double oldY, double newY)
        {
            if (_layoutGridTransform != null)
            {
                _layoutGridTransform.TranslateY = newY;
            }
        }
        #endregion

        #region WindowStartupLocation
        /// <summary>
        /// WindowStartupLocation Dependency Property
        /// </summary>
        public static readonly DependencyProperty WindowStartupLocationProperty =
            DependencyProperty.Register(
                "WindowStartupLocation",
                typeof(WindowStartupLocation),
                typeof(ToolWindow),
                new PropertyMetadata(WindowStartupLocation.Manual, OnWindowStartupLocationChanged));

        /// <summary>
        /// Gets or sets the WindowStartupLocation value that specifies the top/left
        /// position of a window when first shown. The default is WindowStartupLocation.Manual.
        /// </summary>
        public WindowStartupLocation WindowStartupLocation
        {
            get { return (WindowStartupLocation)GetValue(WindowStartupLocationProperty); }
            set { SetValue(WindowStartupLocationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the WindowStartupLocation property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnWindowStartupLocationChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolWindow)d;
            WindowStartupLocation oldWindowStartupLocation = (WindowStartupLocation)e.OldValue;
            WindowStartupLocation newWindowStartupLocation = target.WindowStartupLocation;
            target.OnWindowStartupLocationChanged(oldWindowStartupLocation, newWindowStartupLocation);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the WindowStartupLocation property.
        /// </summary>
        /// <param name="oldWindowStartupLocation">The old WindowStartupLocation value</param>
        /// <param name="newWindowStartupLocation">The new WindowStartupLocation value</param>
        protected virtual void OnWindowStartupLocationChanged(
            WindowStartupLocation oldWindowStartupLocation, WindowStartupLocation newWindowStartupLocation)
        {
        }
        #endregion

        #region WindowMovableArea
        /// <summary>
        /// WindowMovableArea Dependency Property
        /// </summary>
        public static readonly DependencyProperty WindowMovableAreaProperty =
            DependencyProperty.Register(
                "WindowMovableArea",
                typeof(WindowMovableArea),
                typeof(ToolWindow),
                new PropertyMetadata(WindowMovableArea.UseParentBounds, OnWindowMovableAreaChanged));

        /// <summary>
        /// Gets or sets the movable area for the window.
        /// </summary>
        public WindowMovableArea WindowMovableArea
        {
            get { return (WindowMovableArea)GetValue(WindowMovableAreaProperty); }
            set { SetValue(WindowMovableAreaProperty, value); }
        }

        /// <summary>
        /// Handles changes to the WindowMovableArea property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnWindowMovableAreaChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolWindow)d;
            WindowMovableArea oldWindowMovableArea = (WindowMovableArea)e.OldValue;
            WindowMovableArea newWindowMovableArea = target.WindowMovableArea;
            target.OnWindowMovableAreaChanged(oldWindowMovableArea, newWindowMovableArea);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the WindowMovableArea property.
        /// </summary>
        /// <param name="oldWindowMovableArea">The old WindowMovableArea value</param>
        /// <param name="newWindowMovableArea">The new WindowMovableArea value</param>
        protected virtual void OnWindowMovableAreaChanged(
            WindowMovableArea oldWindowMovableArea, WindowMovableArea newWindowMovableArea)
        {
            this.SnapToEdgeIfNecessary();
        }
        #endregion

        #region WindowMovableAreaEdgeThickness
        /// <summary>
        /// WindowMovableAreaEdgeThickness Dependency Property
        /// </summary>
        public static readonly DependencyProperty WindowMovableAreaEdgeThicknessProperty =
            DependencyProperty.Register(
                "WindowMovableAreaEdgeThickness",
                typeof(Thickness),
                typeof(ToolWindow),
                new PropertyMetadata(new Thickness(0, 0, 0, 0), OnWindowMovableAreaEdgeThicknessChanged));

        /// <summary>
        /// Gets or sets the thickness of the movable area frame.
        /// It determines how far the movable area should extend beyond the parent's or app window's boundaries.
        /// The default is zero.
        /// </summary>
        public Thickness WindowMovableAreaEdgeThickness
        {
            get { return (Thickness)GetValue(WindowMovableAreaEdgeThicknessProperty); }
            set { SetValue(WindowMovableAreaEdgeThicknessProperty, value); }
        }

        /// <summary>
        /// Handles changes to the WindowMovableAreaEdgeThickness property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnWindowMovableAreaEdgeThicknessChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolWindow)d;
            Thickness oldWindowMovableAreaEdgeThickness = (Thickness)e.OldValue;
            Thickness newWindowMovableAreaEdgeThickness = target.WindowMovableAreaEdgeThickness;
            target.OnWindowMovableAreaEdgeThicknessChanged(oldWindowMovableAreaEdgeThickness, newWindowMovableAreaEdgeThickness);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the WindowMovableAreaEdgeThickness property.
        /// </summary>
        /// <param name="oldWindowMovableAreaEdgeThickness">The old WindowMovableAreaEdgeThickness value</param>
        /// <param name="newWindowMovableAreaEdgeThickness">The new WindowMovableAreaEdgeThickness value</param>
        protected virtual void OnWindowMovableAreaEdgeThicknessChanged(
            Thickness oldWindowMovableAreaEdgeThickness, Thickness newWindowMovableAreaEdgeThickness)
        {
        }
        #endregion

        #region WindowEdgeSnapBehavior
        /// <summary>
        /// WindowEdgeSnapBehavior Dependency Property
        /// </summary>
        public static readonly DependencyProperty WindowEdgeSnapBehaviorProperty =
            DependencyProperty.Register(
                "WindowEdgeSnapBehavior",
                typeof(WindowEdgeSnapBehavior),
                typeof(ToolWindow),
                new PropertyMetadata(WindowEdgeSnapBehavior.ToTitleBarWithRotation, OnWindowEdgeSnapBehaviorChanged));

        /// <summary>
        /// Gets or sets a value indicating how the window snaps to edges of its movable area.
        /// </summary>
        public WindowEdgeSnapBehavior WindowEdgeSnapBehavior
        {
            get { return (WindowEdgeSnapBehavior)GetValue(WindowEdgeSnapBehaviorProperty); }
            set { SetValue(WindowEdgeSnapBehaviorProperty, value); }
        }

        /// <summary>
        /// Handles changes to the WindowEdgeSnapBehavior property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnWindowEdgeSnapBehaviorChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolWindow)d;
            WindowEdgeSnapBehavior oldWindowEdgeSnapBehavior = (WindowEdgeSnapBehavior)e.OldValue;
            WindowEdgeSnapBehavior newWindowEdgeSnapBehavior = target.WindowEdgeSnapBehavior;
            target.OnWindowEdgeSnapBehaviorChanged(oldWindowEdgeSnapBehavior, newWindowEdgeSnapBehavior);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the WindowEdgeSnapBehavior property.
        /// </summary>
        /// <param name="oldWindowEdgeSnapBehavior">The old WindowEdgeSnapBehavior value</param>
        /// <param name="newWindowEdgeSnapBehavior">The new WindowEdgeSnapBehavior value</param>
        protected virtual void OnWindowEdgeSnapBehaviorChanged(
            WindowEdgeSnapBehavior oldWindowEdgeSnapBehavior, WindowEdgeSnapBehavior newWindowEdgeSnapBehavior)
        {
            UIElement dragSource;

            if (this.CanDragAnywhere)
            {
                dragSource = _border;
            }
            else
            {
                dragSource = _titleGrid;
            }

            if (dragSource != null)
            {
                if (this.WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.None)
                {
                    dragSource.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                }
                else
                {
                    dragSource.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY |
                                                ManipulationModes.TranslateInertia;
                }
            }

            this.SnapToEdgeIfNecessary();
        }
        #endregion

        #region CanSnapToEdge
        /// <summary>
        /// CanSnapToEdge Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanSnapToEdgeProperty =
            DependencyProperty.Register(
                "CanSnapToEdge",
                typeof(bool),
                typeof(ToolWindow),
                new PropertyMetadata(true, OnCanSnapToEdgeChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the window snaps to edges of the movable area.
        /// </summary>
        public bool CanSnapToEdge
        {
            get { return (bool)GetValue(CanSnapToEdgeProperty); }
            set { SetValue(CanSnapToEdgeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CanSnapToEdge property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCanSnapToEdgeChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolWindow)d;
            bool oldCanSnapToEdge = (bool)e.OldValue;
            bool newCanSnapToEdge = target.CanSnapToEdge;
            target.OnCanSnapToEdgeChanged(oldCanSnapToEdge, newCanSnapToEdge);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CanSnapToEdge property.
        /// </summary>
        /// <param name="oldCanSnapToEdge">The old CanSnapToEdge value</param>
        /// <param name="newCanSnapToEdge">The new CanSnapToEdge value</param>
        protected virtual void OnCanSnapToEdgeChanged(
            bool oldCanSnapToEdge, bool newCanSnapToEdge)
        {
            this.UpdateVisualStates(true);
        }
        #endregion

        #region CanClose
        /// <summary>
        /// CanClose Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanCloseProperty =
            DependencyProperty.Register(
                "CanClose",
                typeof(bool),
                typeof(ToolWindow),
                new PropertyMetadata(true, OnCanCloseChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the window can be closed by user.
        /// </summary>
        public bool CanClose
        {
            get { return (bool)GetValue(CanCloseProperty); }
            set { SetValue(CanCloseProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CanClose property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCanCloseChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolWindow)d;
            bool oldCanClose = (bool)e.OldValue;
            bool newCanClose = target.CanClose;
            target.OnCanCloseChanged(oldCanClose, newCanClose);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CanClose property.
        /// </summary>
        /// <param name="oldCanClose">The old CanClose value</param>
        /// <param name="newCanClose">The new CanClose value</param>
        protected virtual void OnCanCloseChanged(
            bool oldCanClose, bool newCanClose)
        {
            UpdateVisualStates(true);
        }
        #endregion

        #region CanResize
        /// <summary>
        /// CanResize Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanResizeProperty =
            DependencyProperty.Register(
                "CanResize",
                typeof(bool),
                typeof(ToolWindow),
                new PropertyMetadata(true, OnCanResizeChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the window can be resized.
        /// </summary>
        public bool CanResize
        {
            get { return (bool)GetValue(CanResizeProperty); }
            set { SetValue(CanResizeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CanResize property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCanResizeChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolWindow)d;
            bool oldCanResize = (bool)e.OldValue;
            bool newCanResize = target.CanResize;
            target.OnCanResizeChanged(oldCanResize, newCanResize);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CanResize property.
        /// </summary>
        /// <param name="oldCanResize">The old CanResize value</param>
        /// <param name="newCanResize">The new CanResize value</param>
        protected virtual void OnCanResizeChanged(
            bool oldCanResize, bool newCanResize)
        {
            this.UpdateVisualStates(true);
        }
        #endregion

        #region CanMaximize
        /// <summary>
        /// CanMaximize Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanMaximizeProperty =
            DependencyProperty.Register(
                "CanMaximize",
                typeof(bool),
                typeof(ToolWindow),
                new PropertyMetadata(true, OnCanMaximizeChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the window can be maximized.
        /// </summary>
        public bool CanMaximize
        {
            get { return (bool)GetValue(CanMaximizeProperty); }
            set { SetValue(CanMaximizeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CanMaximize property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCanMaximizeChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolWindow)d;
            bool oldCanMaximize = (bool)e.OldValue;
            bool newCanMaximize = target.CanMaximize;
            target.OnCanMaximizeChanged(oldCanMaximize, newCanMaximize);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CanMaximize property.
        /// </summary>
        /// <param name="oldCanMaximize">The old CanMaximize value</param>
        /// <param name="newCanMaximize">The new CanMaximize value</param>
        protected virtual void OnCanMaximizeChanged(
            bool oldCanMaximize, bool newCanMaximize)
        {
            this.UpdateVisualStates(true);
        }
        #endregion

        #region CanDragAnywhere
        /// <summary>
        /// CanDragAnywhere Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanDragAnywhereProperty =
            DependencyProperty.Register(
                "CanDragAnywhere",
                typeof(bool),
                typeof(ToolWindow),
                new PropertyMetadata(false, OnCanDragAnywhereChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the window can be dragged by starting anywhere - not only the title bar, but also the client area.
        /// </summary>
        public bool CanDragAnywhere
        {
            get { return (bool)GetValue(CanDragAnywhereProperty); }
            set { SetValue(CanDragAnywhereProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CanDragAnywhere property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCanDragAnywhereChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolWindow)d;
            bool oldCanDragAnywhere = (bool)e.OldValue;
            bool newCanDragAnywhere = target.CanDragAnywhere;
            target.OnCanDragAnywhereChanged(oldCanDragAnywhere, newCanDragAnywhere);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CanDragAnywhere property.
        /// </summary>
        /// <param name="oldCanDragAnywhere">The old CanDragAnywhere value</param>
        /// <param name="newCanDragAnywhere">The new CanDragAnywhere value</param>
        protected virtual void OnCanDragAnywhereChanged(
            bool oldCanDragAnywhere, bool newCanDragAnywhere)
        {
            UnhookEvents();
            HookUpEvents();
        }
        #endregion

        #region WindowState
        /// <summary>
        /// WindowState Dependency Property
        /// </summary>
        public static readonly DependencyProperty WindowStateProperty =
            DependencyProperty.Register(
                "WindowState",
                typeof(WindowStates),
                typeof(ToolWindow),
                new PropertyMetadata(WindowStates.Normal, OnWindowStateChanged));


        /// <summary>
        /// Gets or sets a value indicating the state of the window.
        /// </summary>
        public WindowStates WindowState
        {
            get { return (WindowStates)GetValue(WindowStateProperty); }
            set { SetValue(WindowStateProperty, value); }
        }

        /// <summary>
        /// Handles changes to the WindowState property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnWindowStateChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToolWindow)d;
            var oldWindowState = (WindowStates)e.OldValue;
            var newWindowState = target.WindowState;
            target.OnWindowStateChanged(oldWindowState, newWindowState);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the WindowState property.
        /// </summary>
        /// <param name="oldWindowState">The old WindowState value</param>
        /// <param name="newWindowState">The new WindowState value</param>
        protected virtual void OnWindowStateChanged(
            WindowStates oldWindowState, WindowStates newWindowState)
        {
            if (oldWindowState == newWindowState)
            {
                return;
            }

            if (_restorePositionOnStateChange)
            {
                if (oldWindowState == WindowStates.Snapped)
                {
                    this.StopCurrentSnapStoryboard();

                    if (!_isDraggingFromSnapped)
                    {
#pragma warning disable 4014
                        this.AnimateStraightSnapAsync(_lastWindowPosition.X, _lastWindowPosition.Y);
#pragma warning restore 4014
                    }
                }
                else if (oldWindowState == WindowStates.Maximized)
                {
                    if (!_isDraggingFromSnapped)
                    {
                        this.X = _lastWindowPosition.X;
                        this.Y = _lastWindowPosition.Y;
                    }

                    if (_lastWindowWidth != 0)
                    {
                        this.Width = _lastWindowWidth;
                        this.Height = _lastWindowHeight;
                    }
                }
            }

            this.UpdateVisualStates(true);
        }
        #endregion
        #endregion

        #region CTOR
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindow"/> class.
        /// </summary>
        public ToolWindow()
        {
            this.DefaultStyleKey = typeof(ToolWindow);
            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
            new PropertyChangeEventSource<Thickness>(this, "BorderThickness").ValueChanged += OnBorderThicknessChanged;
        }
        #endregion

        #region Public methods
        #region ActivateAsync()
        /// <summary>
        /// Activates the window.
        /// The method can be awaited to wait for the unsnap animation to complete.
        /// </summary>
        /// <returns></returns>
        public async Task ActivateAsync()
        {
            var maxZIndex = this.GetSiblings().Aggregate(0, (z, dob) => Math.Max(z, Canvas.GetZIndex((UIElement)dob)));
            Canvas.SetZIndex(this, maxZIndex + 1);

            if (_layoutGridTransform != null &&
                _layoutGridTransform.Rotation != 0)
            {
                await AnimateToStraighten();
            }
        }
        #endregion

        #region Close()
        /// <summary>
        /// Closes the window.
        /// </summary>
        public void Close()
        {
            if (!this.CanClose)
            {
                return;
            }

            if (RaiseClosing())
            {
                return;
            }

            var parentPanel = this.Parent as Panel;

            if (parentPanel != null)
            {
                parentPanel.Children.Remove(this);
            }
            else
            {
                var parentContentControl = this.Parent as ContentControl;

                if (parentContentControl != null)
                {
                    parentContentControl.Content = null;
                }
                else
                {
                    this.Hide();
                }
            }

            CleanupSnapProperties();
            RaiseClosed();
        }
        #endregion

        #region Hide()
        /// <summary>
        /// Hides the window.
        /// </summary>
        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Show()
        /// <summary>
        /// Shows the window.
        /// </summary>
        public void Show()
        {
            this.Visibility = Visibility.Visible;
            this.SnapToEdgeIfNecessary();
        }
        #endregion

        #region SnapToEdgeAsync()
        /// <summary>
        /// Snaps to nearest edge.
        /// </summary>
        public async Task SnapToEdgeAsync()
        {
            if (WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.None)
            {
                return;
            }

            if (this.Parent == null)
            {
                await this.WaitForLoadedAsync();
            }

            if (WindowState == WindowStates.Normal)
            {
                _lastWindowPosition.X = this.X;
                _lastWindowPosition.Y = this.Y;
            }

            double angle;
            var movableAreaBoundaries = this.GetMovableArea();

            double distanceToLeft = this.X.Distance(movableAreaBoundaries.Left);
            double distanceToTop = this.Y.Distance(movableAreaBoundaries.Top);
            double distanceToRight = this.X.Distance(movableAreaBoundaries.Right);
            double distanceToBottom = this.Y.Distance(movableAreaBoundaries.Bottom);
            double minDistance = distanceToLeft;
            var x = movableAreaBoundaries.Left - 1;
            var y = this.Y;

            if (distanceToTop < minDistance)
            {
                minDistance = distanceToTop;
                x = this.X;
                y = movableAreaBoundaries.Top - 1;
            }

            if (distanceToRight < minDistance)
            {
                minDistance = distanceToRight;
                x = movableAreaBoundaries.Right + 1;
                y = this.Y;
            }

            if (distanceToBottom < minDistance)
            {
                x = this.X;
                y = movableAreaBoundaries.Bottom + 1;
            }

            var desiredPosition = this.GetDesiredPosition(x, y, out angle, out _lastSnapEdge);
            x = desiredPosition.X;
            y = desiredPosition.Y;

            _restorePositionOnStateChange = false;
            this.WindowState = WindowStates.Snapped;
            _restorePositionOnStateChange = true;

            if (WindowEdgeSnapBehavior != WindowEdgeSnapBehavior.ToTitleBarWithRotation)
            {
                await AnimateStraightSnapAsync(x, y);
            }
            else
            {
                await AnimateRotatedSnapAsync(x, y, angle);
            }
        }
        #endregion

        #region RestoreAsync()
        /// <summary>
        /// Restores the window
        /// </summary>
        public async Task RestoreAsync()
        {
            this.Visibility = Visibility.Visible;
            this.WindowState = WindowStates.Normal;
            await this.ActivateAsync();
        }
        #endregion

        #region MinimizeAsync()
        /// <summary>
        /// Minimizes the window.
        /// </summary>
        public async Task MinimizeAsync()
        {
            await this.SnapToEdgeAsync();
        }
        #endregion

        #region Maximize()
        /// <summary>
        /// Maximizes the window
        /// </summary>
        public void Maximize()
        {
            if (WindowState == WindowStates.Normal)
            {
                _lastWindowPosition.X = this.X;
                _lastWindowPosition.Y = this.Y;
                _lastWindowWidth = this.ActualWidth;
                _lastWindowHeight = this.ActualHeight;
            }

            this.X = - this.BorderThickness.Left;
            this.Y = - this.BorderThickness.Top;

            if (_parent != null)
            {
                this.Width = _parent.ActualWidth + this.BorderThickness.Left + this.BorderThickness.Right;
                this.Height = _parent.ActualHeight + this.BorderThickness.Top + this.BorderThickness.Bottom;
            }

            this.UpdateVisualStates(true);
            this.WindowState = WindowStates.Maximized;
        }
        #endregion
        #endregion

        #region Event handlers and overrides
        #region OnLoaded()
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.InitializeStartupLocation();
            this.SnapToEdgeIfNecessary();

            _parent = this.Parent as FrameworkElement;
            if (_parent != null)
                _parent.SizeChanged += OnParentSizeChanged;
        }
        #endregion

        #region OnUnloaded()
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_parent != null)
                _parent.SizeChanged -= OnParentSizeChanged;
            _parent = null;
        } 
        #endregion

        #region OnParentSizeChanged()
        private void OnParentSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.SnapToEdgeIfNecessary();
        } 
        #endregion

        #region OnApplyTemplate()
        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.UnhookEvents();

            _layoutGrid = this.GetTemplateChild(LayoutGridName) as Grid;
            _sizingGrid = this.GetTemplateChild(SizingGridName) as Grid;
            _titleGrid = this.GetTemplateChild(TitleGridName) as Grid;
            _border = this.GetTemplateChild(BorderName) as Border;
            _buttonsPanel = this.GetTemplateChild(ButtonsPanelName) as StackPanel;
            _closeButton = this.GetTemplateChild(CloseButtonName) as Button;
            _snapButton = this.GetTemplateChild(SnapButtonName) as Button;
            _restoreButton = this.GetTemplateChild(RestoreButtonName) as Button;
            _minimizeButton = this.GetTemplateChild(MinimizeButtonName) as Button;
            _maximizeButton = this.GetTemplateChild(MaximizeButtonName) as Button;

            this.HookUpEvents();

            UpdateVisualStates(false);
        }
        #endregion

        #region OnBorderThicknessChanged()
        private void OnBorderThicknessChanged(object sender, Thickness thickness)
        {
            this.UpdateSizingThumbSizes();
        }
        #endregion

        #region OnBorderPointerPressed()
        private void OnBorderPointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }
        #endregion

        #region OnBorderManipulationStarting()
        private async void OnBorderManipulationStarting(object sender, ManipulationStartedRoutedEventArgs e)
        {
            if (this.WindowState == WindowStates.Normal)
            {
                _lastWindowPosition.X = this.X;
                _lastWindowPosition.Y = this.Y;
            }
            else if (this.WindowState == WindowStates.Snapped)
            {
                _isDraggingFromSnapped = true;
            }

            //if (_lastWindowWidth != 0)
            //{
            //    this.Width = _lastWindowWidth;
            //    this.Height = _lastWindowHeight;
            //}

            _restorePositionOnStateChange = false;
            this.WindowState = WindowStates.Normal;
            _restorePositionOnStateChange = true;
            await this.ActivateAsync(); 
            
            _isAdjustedFlick = false;
        }
        #endregion

        #region OnBorderManipulationDelta()
        private void OnBorderManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (_isFlickTooLong)
            {
                e.Complete();
                return;
            }

            if (!this._isAdjustedFlick)
            {
                this.X += e.Delta.Translation.X;
                this.Y += e.Delta.Translation.Y;
            }
            else
            {
                var dx = e.Cumulative.Translation.X - _flickStartCumulativeX;
                var dy = e.Cumulative.Translation.Y - _flickStartCumulativeY;
                double i;

                if (Math.Abs(dx) > Math.Abs(dy))
                {
                    i = dx / _naturalFlickDisplacementX;
                }
                else
                {
                    i = dy / _naturalFlickDisplacementY;
                }

                this.X = MathEx.Lerp(_flickStartX + e.Cumulative.Translation.X - _flickStartCumulativeX, MathEx.Lerp(_flickStartX, _flickAdjustedEndX, i), i);
                this.Y = MathEx.Lerp(_flickStartY + e.Cumulative.Translation.Y - _flickStartCumulativeY, MathEx.Lerp(_flickStartY, _flickAdjustedEndY, i), i);

                if (_layoutGridTransform != null)
                {
                    _layoutGridTransform.Rotation = MathEx.Lerp(_flickStartAngle, _flickAdjustedEndAngle, i);
                }
            }
        }
        #endregion

        #region OnBorderManipulationInertiaStarting()
        private void OnBorderManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            _flickStartX = this.X;
            _flickStartY = this.Y;
            _flickStartCumulativeX = e.Cumulative.Translation.X;
            _flickStartCumulativeY = e.Cumulative.Translation.Y;

            if (_isDraggingFromSnapped)
            {
                // TODO: Prevent snapping if flicking towards center of screen
            }

            if (_layoutGridTransform != null)
            {
                _flickStartAngle = _layoutGridTransform.Rotation;
            }

            var naturalFlickDisplacement = e.GetExpectedDisplacement();
            e.TranslationBehavior.DesiredDisplacement = naturalFlickDisplacement;
            _naturalFlickDisplacementX = e.GetExpectedDisplacementX();
            _naturalFlickDisplacementY = e.GetExpectedDisplacementY();

            Edges desiredSnapEdge;
            var desiredPosition = this.GetDesiredPosition(_flickStartX + _naturalFlickDisplacementX, _flickStartY + _naturalFlickDisplacementY, out _flickAdjustedEndAngle, out desiredSnapEdge);
            _flickAdjustedEndX = desiredPosition.X;
            _flickAdjustedEndY = desiredPosition.Y;

            if (desiredSnapEdge == Edges.None)
            {
                return;
            }

            _lastSnapEdge = desiredSnapEdge;
            _isAdjustedFlick = true;

            if (_flickAdjustedEndX != _naturalFlickDisplacementX && (_flickAdjustedEndY == _naturalFlickDisplacementY ||
                _flickAdjustedEndX.Distance(_naturalFlickDisplacementX) > _flickAdjustedEndY.Distance(_naturalFlickDisplacementY)))
            {
                // snap to left or right side
                e.SetDesiredDisplacementX(_flickAdjustedEndX - _flickStartX);
                _naturalFlickDisplacementX = e.GetExpectedDisplacementX();
                _naturalFlickDisplacementY = e.GetExpectedDisplacementY();
            }
            else if (_flickAdjustedEndY != _naturalFlickDisplacementY && (_flickAdjustedEndX == _naturalFlickDisplacementX ||
                _flickAdjustedEndX.Distance(_naturalFlickDisplacementY) > _flickAdjustedEndY.Distance(_naturalFlickDisplacementX)))
            {
                // snap to left or right side
                e.SetDesiredDisplacementY(_flickAdjustedEndY - _flickStartY);
                _naturalFlickDisplacementX = e.GetExpectedDisplacementX();
                _naturalFlickDisplacementY = e.GetExpectedDisplacementY();
            }

            if (e.GetExpectedDisplacementDuration() > 0.5)
            {
                _isFlickTooLong = true;
                _restorePositionOnStateChange = false;
                this.WindowState = WindowStates.Snapped;
                _restorePositionOnStateChange = true;

                if (WindowEdgeSnapBehavior != WindowEdgeSnapBehavior.ToTitleBarWithRotation)
                {
#pragma warning disable 4014
                    AnimateStraightSnapAsync(_flickAdjustedEndX, _flickAdjustedEndY);
#pragma warning restore 4014
                }
                else
                {
#pragma warning disable 4014
                    AnimateRotatedSnapAsync(_flickAdjustedEndX, _flickAdjustedEndY, _flickAdjustedEndAngle);
#pragma warning restore 4014
                }
            }
        }
        #endregion

        #region OnBorderManipulationCompleted()
        private void OnBorderManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (_isFlickTooLong)
            {
                _isFlickTooLong = false;
                return;
            }

            //if (!_isDraggingFromSnapped
            //
            //    )
            {
                // TODO: Snap if really necessary...
                this.SnapToEdgeIfNecessary();
            }

            _isDraggingFromSnapped = false;
        }
        #endregion

        #region SnapToEdgeIfNecessary()
        //TODO: Remove if not needed.
        private enum Edges
        {
            None,
            Top,
            Right,
            Bottom,
            Left
        }

        private void SnapToEdgeIfNecessary()
        {
            if (WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.None)
            {
                return;
            }

            double angle;
            var desiredPosition = GetDesiredPosition(this.X, this.Y, out angle, out _lastSnapEdge);
            var x = desiredPosition.X;
            var y = desiredPosition.Y;

            if (_layoutGridTransform == null || (
                x == this.X &&
                y == this.Y &&
                angle == _layoutGridTransform.Rotation))
            {
                return;
            }

            _restorePositionOnStateChange = false;
            this.WindowState = WindowStates.Snapped;
            _restorePositionOnStateChange = true;

            if (WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.Straight ||
                WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.StraightToTitleBar)
            {
#pragma warning disable 4014
                AnimateStraightSnapAsync(x, y);
#pragma warning restore 4014
            }
            else
            {
#pragma warning disable 4014
                AnimateRotatedSnapAsync(x, y, angle);
#pragma warning restore 4014
            }
        }

        /// <summary>
        /// Calculates the desired position, rotation and snapping edge given the position of the window.
        /// </summary>
        /// <param name="startx"></param>
        /// <param name="starty"></param>
        /// <param name="angle"></param>
        /// <param name="edge"></param>
        /// <returns></returns>
        private DoublePoint GetDesiredPosition(double startx, double starty, out double angle, out Edges edge)
        {
            angle = 0d;
            edge = Edges.None;

            if (WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.None)
            {
                _lastWindowPosition.X = startx;
                _lastWindowPosition.Y = starty;
            }

            var movableAreaBoundaries = this.GetMovableArea();

            if (WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.Straight ||
                WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.StraightToTitleBar)
            {
                var x = startx.Max(movableAreaBoundaries.Left).Min(movableAreaBoundaries.Right);
                var y = starty.Max(movableAreaBoundaries.Top).Min(movableAreaBoundaries.Bottom);
                DoublePoint ret;
                ret.X = x;
                ret.Y = y;

                return ret;
            }
            else// if (WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.ToTitleBarWithRotation)
            {
                var x = startx.Max(movableAreaBoundaries.Left).Min(movableAreaBoundaries.Right);
                var y = starty.Max(movableAreaBoundaries.Top).Min(movableAreaBoundaries.Bottom);

                if (x != startx || y != starty)
                {
                    // title height
                    var t = _titleGrid == null ? 0 : _titleGrid.ActualHeight + (_border == null ? 0 : _border.BorderThickness.Top);
                    var h = this.ActualHeight;
                    var w = this.ActualWidth;

                    if (x != startx && (y == starty || x.Distance(startx) > y.Distance(starty)))
                    {
                        if (startx < x)
                        {
                            edge = Edges.Left;
                            x += t - w / 2 - h / 2;
                            angle = 90;
                        }
                        else
                        {
                            edge = Edges.Right;
                            x -= t - w / 2 - h / 2;
                            angle = -90;
                        }

                        var minY = movableAreaBoundaries.Top + this.ActualWidth / 2 - this.ActualHeight / 2;
                        var maxY = movableAreaBoundaries.Bottom - this.ActualWidth / 2 + this.ActualHeight / 2;

                        y = starty.Max(minY).Min(maxY);
                    }
                    else if (y != starty && (x == startx || y.Distance(starty) > x.Distance(startx)))
                    {
                        if (starty < y)
                        {
                            edge = Edges.Top;
                            angle = 180;
                            y += t - h;
                        }
                        else
                        {
                            edge = Edges.Bottom;
                            angle = 0;
                            y -= t - h;
                        }
                    }
                }

                DoublePoint ret;
                ret.X = x;
                ret.Y = y;
                return ret;
            }
        }
        #endregion

        #region OnCloseButtonClick()
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region OnSnapButtonClick()
        private void OnSnapButtonClick(object sender, RoutedEventArgs e)
        {
#pragma warning disable 4014
            SnapToEdgeAsync();
#pragma warning restore 4014
        }
        #endregion

        #region OnRestoreButtonClick()
        private void OnRestoreButtonClick(object sender, RoutedEventArgs e)
        {
#pragma warning disable 4014
            this.RestoreAsync();
#pragma warning restore 4014
        }
        #endregion

        #region OnMinimizeButtonClick()
        private async void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            await MinimizeAsync();
        }
        #endregion

        #region OnMaximizeButtonClick()
        private void OnMaximizeButtonClick(object sender, RoutedEventArgs e)
        {
            Maximize();
        }
        #endregion
        #endregion

        #region Filtering out manipulation events from window state buttons.
        #region FilterOutManipulations()
        private void FilterOutManipulations(UIElement element)
        {
            element.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            element.ManipulationStarting += OnFilteredOutManipulationStarting;
            element.ManipulationStarted += OnFilteredOutManipulationStarted;
        }
        #endregion

        #region UnFilterOutManipulations()
        private void UnFilterOutManipulations(UIElement element)
        {
            element.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            element.ManipulationStarting -= OnFilteredOutManipulationStarting;
            element.ManipulationStarted -= OnFilteredOutManipulationStarted;
        }
        #endregion

        #region OnFilteredOutManipulationStarting()
        private void OnFilteredOutManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            e.Handled = true;
        }
        #endregion

        #region OnFilteredOutManipulationStarted()
        private void OnFilteredOutManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            e.Handled = true;
            e.Complete();
        }
        #endregion
        #endregion

        #region Private methods
        #region UpdateVisualStates()
        /// <summary>
        /// Updates the visual states.
        /// </summary>
        private void UpdateVisualStates(bool useTransitions)
        {
            if (this.CanClose)
            {
                VisualStateManager.GoToState(this, "CloseButtonVisible", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "CloseButtonCollapsed", useTransitions);
            }

            switch (this.WindowState)
            {
                case WindowStates.Normal:
                    if (this.CanSnapToEdge)
                    {
                        VisualStateManager.GoToState(this, "SnapButtonVisible", useTransitions);
                    }
                    else
                    {
                        VisualStateManager.GoToState(this, "SnapButtonCollapsed", useTransitions);
                    }

                    if (this.CanMaximize)
                    {
                        VisualStateManager.GoToState(this, "MaximizeButtonVisible", useTransitions);
                    }
                    else
                    {
                        VisualStateManager.GoToState(this, "MaximizeButtonCollapsed", useTransitions);
                    }

                    VisualStateManager.GoToState(this, "RestoreButtonCollapsed", useTransitions);

                    if (_sizingGrid != null)
                    {
                        _sizingGrid.Visibility = this.CanResize ? Visibility.Visible : Visibility.Collapsed;
                    }

                    //MaximizeButtonVisible
                    break;
                case WindowStates.Snapped:
                    VisualStateManager.GoToState(this, "SnapButtonCollapsed", useTransitions);
                    VisualStateManager.GoToState(this, "RestoreButtonVisible", useTransitions);
                    VisualStateManager.GoToState(this, "MaximizeButtonCollapsed", useTransitions);

                    if (_sizingGrid != null)
                    {
                        _sizingGrid.Visibility = Visibility.Collapsed;
                    }
                    break;
                case WindowStates.Maximized:
                    if (this.CanSnapToEdge)
                    {
                        VisualStateManager.GoToState(this, "SnapButtonVisible", useTransitions);
                    }
                    else
                    {
                        VisualStateManager.GoToState(this, "SnapButtonCollapsed", useTransitions);
                    }

                    VisualStateManager.GoToState(this, "RestoreButtonVisible", useTransitions);
                    VisualStateManager.GoToState(this, "MaximizeButtonCollapsed", useTransitions);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region InitializeStartupLocation()
        private void InitializeStartupLocation()
        {
            var parent = this.Parent as FrameworkElement;

            if (parent == null)
            {
                return;
            }

            switch (WindowStartupLocation)
            {
                case WindowStartupLocation.CenterOwner:
                    this.X = Math.Round(parent.ActualWidth / 2 - this.ActualWidth / 2);
                    this.Y = Math.Round(parent.ActualHeight / 2 - this.ActualHeight / 2);

                    break;
                case WindowStartupLocation.CenterScreen:
                    var parentPosition = parent.GetPosition();
                    var root = Window.Current.Content as FrameworkElement;
                    if (root != null)
                    {
                        this.X = Math.Round(root.ActualWidth / 2 - this.ActualWidth / 2 - parentPosition.X);
                        this.Y = Math.Round(root.ActualHeight / 2 - this.ActualHeight / 2 - parentPosition.Y);
                    }
                    break;
            }

            if (this.WindowState == WindowStates.Normal)
            {
                _lastWindowPosition.X = this.X;
                _lastWindowPosition.Y = this.Y;
                _lastWindowWidth = this.ActualWidth;
                _lastWindowHeight = this.ActualHeight;
            }
        }
        #endregion

        #region HookUpEvents()

        private void HookUpEvents()
        {
            if (_layoutGrid != null)
            {

                _layoutGrid.RenderTransformOrigin = new Point(0.5, 0.5);
                _layoutGrid.RenderTransform = _layoutGridTransform = new CompositeTransform();
                _layoutGridTransform.TranslateX = this.X;
                _layoutGridTransform.TranslateY = this.Y;
            }

            UIElement dragSource;
            if (CanDragAnywhere)
            {
                dragSource = _border;
            }
            else
            {
                dragSource = _titleGrid;
            }


            if (dragSource != null)
            {
                dragSource.PointerPressed += OnBorderPointerPressed;

                if (this.WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.None)
                {
                    dragSource.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                }
                else
                {
                    dragSource.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.TranslateInertia;
                }

                dragSource.ManipulationStarted += OnBorderManipulationStarting;
                dragSource.ManipulationDelta += OnBorderManipulationDelta;
                dragSource.ManipulationInertiaStarting += OnBorderManipulationInertiaStarting;
                dragSource.ManipulationCompleted += OnBorderManipulationCompleted;
            }

            if (_titleGrid != null)
            {
                _titleGrid.RenderTransformOrigin = new Point(0.5, 0.5);
                _titleGrid.RenderTransform = _titleTransform = new CompositeTransform();
                _titleGrid.SizeChanged += OnTitleGridSizeChanged;
            }

            if (_closeButton != null)
            {
                _closeButton.Click += OnCloseButtonClick;
                FilterOutManipulations(_closeButton);
            }

            if (_snapButton != null)
            {
                _snapButton.Click += OnSnapButtonClick;
                FilterOutManipulations(_snapButton);
            }

            if (_restoreButton != null)
            {
                _restoreButton.Click += OnRestoreButtonClick;
                FilterOutManipulations(_restoreButton);
            }

            if (_minimizeButton != null)
            {
                _minimizeButton.Click += OnMinimizeButtonClick;
                FilterOutManipulations(_minimizeButton);
            }

            if (_maximizeButton != null)
            {
                _maximizeButton.Click += OnMaximizeButtonClick;
                FilterOutManipulations(_maximizeButton);
            }

            if (_sizingGrid != null)
            {
                _topLeftSizingThumb = new Rectangle
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Fill = new SolidColorBrush(Colors.Transparent),
                    ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY
                };
                _sizingGrid.Children.Add(_topLeftSizingThumb);
                _topCenterSizingThumb = new Rectangle
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Top,
                    Fill = new SolidColorBrush(Colors.Transparent),
                    ManipulationMode = ManipulationModes.TranslateY
                };
                _sizingGrid.Children.Add(_topCenterSizingThumb);
                _topRightSizingThumb = new Rectangle
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Fill = new SolidColorBrush(Colors.Transparent),
                    ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY
                };
                _sizingGrid.Children.Add(_topRightSizingThumb);
                _centerLeftSizingThumb = new Rectangle
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Fill = new SolidColorBrush(Colors.Transparent),
                    ManipulationMode = ManipulationModes.TranslateX
                };
                _sizingGrid.Children.Add(_centerLeftSizingThumb);
                _centerRightSizingThumb = new Rectangle
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Fill = new SolidColorBrush(Colors.Transparent),
                    ManipulationMode = ManipulationModes.TranslateX
                };
                _sizingGrid.Children.Add(_centerRightSizingThumb);
                _bottomLeftSizingThumb = new Rectangle
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Fill = new SolidColorBrush(Colors.Transparent),
                    ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY
                };
                _sizingGrid.Children.Add(_bottomLeftSizingThumb);
                _bottomCenterSizingThumb = new Rectangle
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Fill = new SolidColorBrush(Colors.Transparent),
                    ManipulationMode = ManipulationModes.TranslateY
                };
                _sizingGrid.Children.Add(_bottomCenterSizingThumb);
                _bottomRightSizingThumb = new Rectangle
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Fill = new SolidColorBrush(Colors.Transparent),
                    ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY
                };
                _sizingGrid.Children.Add(_bottomRightSizingThumb);
# if WINDOWS_APP
                FrameworkElementExtensions.SetSystemCursor(_topLeftSizingThumb, CoreCursorType.SizeNorthwestSoutheast);
                FrameworkElementExtensions.SetSystemCursor(_topCenterSizingThumb, CoreCursorType.SizeNorthSouth);
                FrameworkElementExtensions.SetSystemCursor(_topRightSizingThumb, CoreCursorType.SizeNortheastSouthwest);
                FrameworkElementExtensions.SetSystemCursor(_centerLeftSizingThumb, CoreCursorType.SizeWestEast);
                FrameworkElementExtensions.SetSystemCursor(_centerRightSizingThumb, CoreCursorType.SizeWestEast);
                FrameworkElementExtensions.SetSystemCursor(_bottomLeftSizingThumb, CoreCursorType.SizeNortheastSouthwest);
                FrameworkElementExtensions.SetSystemCursor(_bottomCenterSizingThumb, CoreCursorType.SizeNorthSouth);
                FrameworkElementExtensions.SetSystemCursor(_bottomRightSizingThumb, CoreCursorType.SizeNorthwestSoutheast);
#endif
                this.UpdateSizingThumbSizes();
                HookUpSizingThumbManipulations(_topLeftSizingThumb);
                HookUpSizingThumbManipulations(_topCenterSizingThumb);
                HookUpSizingThumbManipulations(_topRightSizingThumb);
                HookUpSizingThumbManipulations(_centerLeftSizingThumb);
                HookUpSizingThumbManipulations(_centerRightSizingThumb);
                HookUpSizingThumbManipulations(_bottomLeftSizingThumb);
                HookUpSizingThumbManipulations(_bottomCenterSizingThumb);
                HookUpSizingThumbManipulations(_bottomRightSizingThumb);
            }
        }

        private void OnTitleGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowStates.Snapped)
            {
#pragma warning disable 4014
                this.SnapToEdgeAsync();
#pragma warning restore 4014
            }
        }

        private void HookUpSizingThumbManipulations(Rectangle thumb)
        {
            thumb.ManipulationDelta += OnThumbManipulationDelta;
        }

        private void OnThumbManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var x = e.Delta.Translation.X;
            var y = e.Delta.Translation.Y;
            var minWidth = double.IsNaN(this.MinWidth) ? 0 : this.MinWidth;
            var minHeight = double.IsNaN(this.MinHeight) ? 0 : this.MinHeight;
            var maxWidth = this.MaxWidth;
            var maxHeight = this.MaxHeight;

            if (_parent != null)
            {
                maxWidth = maxWidth.Min(_parent.ActualWidth);
                maxHeight = maxHeight.Min(_parent.ActualHeight);
            }

            if (_buttonsPanel != null)
            {
                minWidth = _buttonsPanel.ActualWidth + this.BorderThickness.Left + this.BorderThickness.Right;
            }

            if (_titleGrid != null)
            {
                minHeight = _titleGrid.ActualHeight + this.BorderThickness.Top + this.BorderThickness.Bottom;
            }

            if (sender == _topLeftSizingThumb ||
                sender == _centerLeftSizingThumb ||
                sender == _bottomLeftSizingThumb)
            {
                this.X = this.X + x;
                this.Width = MathEx.Clamp(this.ActualWidth - x, minWidth, maxWidth);
            }
            else if (sender == _topRightSizingThumb ||
                     sender == _centerRightSizingThumb ||
                     sender == _bottomRightSizingThumb)
            {
                this.Width = MathEx.Clamp(this.ActualWidth + x, minWidth, maxWidth);
            }

            if (sender == _topLeftSizingThumb ||
                sender == _topCenterSizingThumb ||
                sender == _topRightSizingThumb)
            {
                this.Y += y;
                this.Height = MathEx.Clamp(this.ActualHeight - y, minHeight, maxHeight);
            }
            else if (sender == _bottomLeftSizingThumb ||
                     sender == _bottomCenterSizingThumb ||
                     sender == _bottomRightSizingThumb)
            {
                this.Height = MathEx.Clamp(this.ActualHeight + y, minHeight, maxHeight);
            }
        }
        #endregion

        #region UpdateSizingThumbSizes()
        /// <summary>
        /// Update the sizes of the sizing thumbs.
        /// </summary>
        private void UpdateSizingThumbSizes()
        {
            double left = this.BorderThickness.Left;
            double top = this.BorderThickness.Top;
            double right = this.BorderThickness.Right;
            double bottom = this.BorderThickness.Bottom;

            if (_topLeftSizingThumb == null)
            {
                return;
            }

            _topLeftSizingThumb.Width = left;
            _topLeftSizingThumb.Height = top;
            _topCenterSizingThumb.Margin = new Thickness(left, 0, right, 0);
            _topCenterSizingThumb.Height = top;
            _topRightSizingThumb.Width = right;
            _topRightSizingThumb.Height = top;
            _centerLeftSizingThumb.Width = left;
            _centerLeftSizingThumb.Margin = new Thickness(0, top, 0, bottom);
            _centerRightSizingThumb.Width = right;
            _centerRightSizingThumb.Margin = new Thickness(0, top, 0, bottom);
            _bottomLeftSizingThumb.Width = left;
            _bottomLeftSizingThumb.Height = bottom;
            _bottomCenterSizingThumb.Margin = new Thickness(left, 0, right, 0);
            _bottomCenterSizingThumb.Height = bottom;
            _bottomRightSizingThumb.Width = left;
            _bottomRightSizingThumb.Height = bottom;
        }
        #endregion

        #region UnhookEvents()
        private void UnhookEvents()
        {
            if (_border != null)
            {
                _border.PointerPressed -= this.OnBorderPointerPressed;
                _border.ManipulationDelta -= this.OnBorderManipulationDelta;
                _border.ManipulationInertiaStarting -= this.OnBorderManipulationInertiaStarting;
                _border.ManipulationCompleted -= this.OnBorderManipulationCompleted;
            }

            if (_closeButton != null)
            {
                _closeButton.Click -= this.OnCloseButtonClick;
            }

            if (_snapButton != null)
            {
                _snapButton.Click -= this.OnSnapButtonClick;
                UnFilterOutManipulations(_snapButton);
            }

            if (_sizingGrid != null)
            {
                _sizingGrid.Children.Remove(_topLeftSizingThumb);
                _sizingGrid.Children.Remove(_topCenterSizingThumb);
                _sizingGrid.Children.Remove(_topRightSizingThumb);
                _sizingGrid.Children.Remove(_centerLeftSizingThumb);
                _sizingGrid.Children.Remove(_centerRightSizingThumb);
                _sizingGrid.Children.Remove(_bottomLeftSizingThumb);
                _sizingGrid.Children.Remove(_bottomCenterSizingThumb);
                _sizingGrid.Children.Remove(_bottomRightSizingThumb);
            }

            if (_titleGrid != null)
            {
                _titleGrid.SizeChanged -= this.OnTitleGridSizeChanged;
            }
        }
        #endregion

        #region CleanupSnapProperties()
        private void CleanupSnapProperties()
        {
            if (_layoutGridTransform != null)
            {
                this.X = Math.Round(_layoutGridTransform.TranslateX);
                this.Y = Math.Round(_layoutGridTransform.TranslateY);
                _layoutGridTransform.Rotation = 0;
            }

            if (_titleTransform != null)
            {
                _titleTransform.TranslateX = 0;
                _titleTransform.TranslateY = 0;
                _titleTransform.Rotation = 0;
            }

            if (_titleGrid != null)
            {
                _titleGrid.Opacity = 1;
            }
        }
        #endregion

        #region AnimateStraightSnapAsync()
        private async Task AnimateStraightSnapAsync(double x, double y)
        {
            if (_layoutGridTransform == null)
            {
                return;
            }

            var sb = new Storyboard();

            var dax = new DoubleAnimation();
            dax.Duration = TimeSpan.FromSeconds(.2);
            Storyboard.SetTarget(dax, _layoutGridTransform);
            Storyboard.SetTargetProperty(dax, "TranslateX");
            dax.To = x;
            sb.Children.Add(dax);

            var day = new DoubleAnimation();
            day.Duration = TimeSpan.FromSeconds(.2);
            Storyboard.SetTarget(day, _layoutGridTransform);
            Storyboard.SetTargetProperty(day, "TranslateY");
            day.To = y;
            sb.Children.Add(day);

            var dar = new DoubleAnimation();
            dar.Duration = TimeSpan.FromSeconds(.2);
            Storyboard.SetTarget(dar, _layoutGridTransform);
            Storyboard.SetTargetProperty(dar, "Rotation");
            dar.To = 0;
            sb.Children.Add(dar);

            this._currentSnapStoryboard = sb;
            await sb.BeginAsync();

            this._currentSnapStoryboard = null;

            if (_layoutGridTransform != null)
            {
                this.X = Math.Round(_layoutGridTransform.TranslateX);
                this.Y = Math.Round(_layoutGridTransform.TranslateY);
            }
        }
        #endregion

        #region AnimateRotatedSnapAsync()
        private async Task AnimateRotatedSnapAsync(double x, double y, double angle)
        {
            if (_layoutGridTransform == null)
            {
                return;
            }

            var sb = new Storyboard();

            var dax = new DoubleAnimation();
            dax.Duration = TimeSpan.FromSeconds(.2);
            Storyboard.SetTarget(dax, _layoutGridTransform);
            Storyboard.SetTargetProperty(dax, "TranslateX");
            dax.To = x;
            sb.Children.Add(dax);

            var day = new DoubleAnimation();
            day.Duration = TimeSpan.FromSeconds(.2);
            Storyboard.SetTarget(day, _layoutGridTransform);
            Storyboard.SetTargetProperty(day, "TranslateY");
            day.To = y;
            sb.Children.Add(day);

            var dar = new DoubleAnimation();
            dar.Duration = TimeSpan.FromSeconds(.2);
            Storyboard.SetTarget(dar, _layoutGridTransform);
            Storyboard.SetTargetProperty(dar, "Rotation");
            dar.To = angle;
            sb.Children.Add(dar);

            if (_titleTransform != null &&
                _titleGrid != null &&
                angle == 180)
            {
                var dato = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(dato, _titleGrid);
                Storyboard.SetTargetProperty(dato, "Opacity");
                dato.KeyFrames.Add(new DiscreteDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(0), Value = 1 });
                dato.KeyFrames.Add(new LinearDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(.1), Value = 0 });
                dato.KeyFrames.Add(new LinearDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(.2), Value = 1 });
                sb.Children.Add(dato);

                var datr = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(datr, _titleTransform);
                Storyboard.SetTargetProperty(datr, "Rotation");
                datr.KeyFrames.Add(new DiscreteDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(.1), Value = -180 });
                sb.Children.Add(datr);

                var datt = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(datt, _titleTransform);
                Storyboard.SetTargetProperty(datt, "TranslateY");
                datt.KeyFrames.Add(new DiscreteDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(.1), Value = -BorderThickness.Top });
                sb.Children.Add(datt);
            }

            this._currentSnapStoryboard = sb;
            await sb.BeginAsync();

            this._currentSnapStoryboard = null;

            if (_layoutGridTransform != null)
            {
                this.X = Math.Round(_layoutGridTransform.TranslateX);
                this.Y = Math.Round(_layoutGridTransform.TranslateY);
            }
        }
        #endregion

        #region StopCurrentSnapStoryboard()
        private void StopCurrentSnapStoryboard()
        {
            if (this._currentSnapStoryboard != null)
            {
                if (_layoutGridTransform != null)
                {
                    this.X = Math.Round(_layoutGridTransform.TranslateX);
                    this.Y = Math.Round(_layoutGridTransform.TranslateY);
                }

                this._currentSnapStoryboard.Stop();
#pragma warning disable 4014
                AnimateToStraighten();
#pragma warning restore 4014
            }
        }
        #endregion

        #region AnimateToStraighten()
        private async Task AnimateToStraighten()
        {
            if (_layoutGridTransform == null)
            {
                return;
            }

            var sb = new Storyboard();
            var dar = new DoubleAnimation();
            dar.Duration = TimeSpan.FromSeconds(.2);
            Storyboard.SetTarget(dar, _layoutGridTransform);
            Storyboard.SetTargetProperty(dar, "Rotation");
            dar.To = 0;
            sb.Children.Add(dar);

            if (_titleTransform != null &&
                _titleGrid != null &&
                _titleTransform.Rotation == -180)
            {
                var dato = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(dato, _titleGrid);
                Storyboard.SetTargetProperty(dato, "Opacity");
                dato.KeyFrames.Add(new DiscreteDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(0), Value = 1 });
                dato.KeyFrames.Add(new LinearDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(.1), Value = 0 });
                dato.KeyFrames.Add(new LinearDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(.2), Value = 1 });
                sb.Children.Add(dato);

                var datr = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(datr, _titleTransform);
                Storyboard.SetTargetProperty(datr, "Rotation");
                datr.KeyFrames.Add(new DiscreteDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(.1), Value = 0 });
                sb.Children.Add(datr);

                var datt = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(datt, _titleTransform);
                Storyboard.SetTargetProperty(datt, "TranslateY");
                datt.KeyFrames.Add(new DiscreteDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(.1), Value = 0 });
                sb.Children.Add(datt);
            }

            await sb.BeginAsync();
            CleanupSnapProperties();
        }
        #endregion

        #region GetMovableArea()
        /// <summary>
        /// Gets the range of X,Y window position values acceptable for the window
        /// </summary>
        /// <returns></returns>
        private Rect GetMovableArea()
        {
            var parent = this.Parent as FrameworkElement;

            if (parent == null)
            {
                return new Rect();
            }

            double h;

            if (_titleGrid != null &&
                this.WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.StraightToTitleBar)
            {
                h = _titleGrid.ActualHeight + (_border == null ? 0 : _border.BorderThickness.Top);
            }
            else
            {
                h = this.ActualHeight;
            }


            if (WindowMovableArea == WindowMovableArea.UseParentBounds)
            {
                return new Rect(
                    -this.WindowMovableAreaEdgeThickness.Left,
                    -this.WindowMovableAreaEdgeThickness.Top,
                    Math.Max(0, parent.ActualWidth + this.WindowMovableAreaEdgeThickness.Left + this.WindowMovableAreaEdgeThickness.Right - this.ActualWidth),
                    Math.Max(0, parent.ActualHeight + this.WindowMovableAreaEdgeThickness.Top + this.WindowMovableAreaEdgeThickness.Bottom - h));
            }
            else
            {
                var parentPosition = parent.GetPosition();
                var root = Window.Current.Content as FrameworkElement;

                if (root == null)
                {
                    return new Rect();
                }

                return new Rect(
                    -this.WindowMovableAreaEdgeThickness.Left - parentPosition.X,
                    -this.WindowMovableAreaEdgeThickness.Top - parentPosition.Y,
                    Math.Max(0, root.ActualWidth + this.WindowMovableAreaEdgeThickness.Left + this.WindowMovableAreaEdgeThickness.Right - this.ActualWidth),
                    Math.Max(0, root.ActualHeight + this.WindowMovableAreaEdgeThickness.Top + this.WindowMovableAreaEdgeThickness.Bottom - h));
            }
        }
        #endregion
        #endregion
    }
}
