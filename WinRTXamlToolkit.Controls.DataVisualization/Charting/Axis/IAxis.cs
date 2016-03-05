using System.Collections.ObjectModel;

namespace WinRTXamlToolkit.Controls.DataVisualization.Charting
{
    /// <summary>
    /// An axis interface used to determine the plot area coordinate of values.
    /// </summary>
    public interface IAxis
    {
        /// <summary>
        /// Gets or sets the orientation of the axis.
        /// </summary>
        AxisOrientation Orientation { get; set; }

        /// <summary>
        /// This event is raised when the Orientation property is changed.
        /// </summary>
        event RoutedPropertyChangedEventHandler<AxisOrientation> OrientationChanged;
 
        /// <summary>
        /// Returns a value indicating whether the axis can plot a value.
        /// </summary>
        /// <param name="value">The value to plot.</param>
        /// <returns>A value indicating whether the axis can plot a value.
        /// </returns>
        bool CanPlot(object value);

        /// <summary>
        /// The plot area coordinate of a value.
        /// </summary>
        /// <param name="value">The value for which to retrieve the plot area
        /// coordinate.</param>
        /// <returns>The plot area coordinate.</returns>
        UnitValue GetPlotAreaCoordinate(object value);

        /// <summary>
        /// Gets the registered IAxisListeners.
        /// </summary>
        ObservableCollection<IAxisListener> RegisteredListeners { get; }

        /// <summary>
        /// Gets the collection of child axes.
        /// </summary>
        ObservableCollection<IAxis> DependentAxes { get; }
    }
}