using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ChartTestView : UserControl
    {
        private bool isInitialized;

        public ChartTestView()
        {
            this.InitializeComponent();
            this.isInitialized = true;
            //UpdateCharts();
        }

        private Random _random = new Random();
        private bool axisLabelsHidden = false;

        private EventThrottler _updateThrottler = new EventThrottler();

        private void RunIfSelected(UIElement element, Action action)
        {
            if (ChartsList.SelectedItem == element)
            {
                action.Invoke();
            }
        }

        private void UpdateCharts()
        {
            if (!this.isInitialized)
            {
                return;
            }

            var items = new List<NameValueItem>();

            for (int i = 0; i < NumberOfIitemsNumericUpDown.Value; i++)
            {
                items.Add(new NameValueItem { Name = "Test" + i, Value = _random.Next(10, 100) });
            }

            RunIfSelected(this.ColumnChart, () => ((ColumnSeries)this.ColumnChart.Series[0]).ItemsSource = items);;
            RunIfSelected(this.BarChart, () => ((BarSeries)this.BarChart.Series[0]).ItemsSource = items);;
            RunIfSelected(this.LineChart, () => ((LineSeries)this.LineChart.Series[0]).ItemsSource = items);;
            RunIfSelected(this.LineChart, () => ((LineSeries)this.LineChart2.Series[0]).ItemsSource = items);;
            RunIfSelected(this.MixedChart, () => ((ColumnSeries)this.MixedChart.Series[0]).ItemsSource = items);;
            RunIfSelected(this.MixedChart, () => ((LineSeries)this.MixedChart.Series[1]).ItemsSource = items);;
            RunIfSelected(this.AreaChart, () => ((AreaSeries)this.AreaChart.Series[0]).ItemsSource = items);;
            RunIfSelected(this.AreaChartWithNoLabels,
                () =>
                {
                    var series = (AreaSeries)this.AreaChartWithNoLabels.Series[0];
                    series.ItemsSource = items;

                    if (!this.axisLabelsHidden)
                    {
                        series.DependentRangeAxis =
                            new LinearAxis
                            {
                                Minimum = 0,
                                Maximum = 100,
                                Orientation = AxisOrientation.Y,
                                Interval = 20,
                                ShowGridLines = false,
                                Width = 0
                            };
                        series.IndependentAxis =
                            new CategoryAxis
                            {
                                Orientation = AxisOrientation.X,
                                Height = 0
                            };
                        this.axisLabelsHidden = true;
                    }
                });

            RunIfSelected(this.BubbleChart, () => ((BubbleSeries)this.BubbleChart.Series[0]).ItemsSource = items);;
            RunIfSelected(this.ScatterChart, () => ((ScatterSeries)this.ScatterChart.Series[0]).ItemsSource = items);;
            RunIfSelected(this.StackedBar, () => ((StackedBarSeries)this.StackedBar.Series[0]).SeriesDefinitions[0].ItemsSource = items);
            RunIfSelected(this.StackedBar, () => ((StackedBarSeries)this.StackedBar.Series[0]).SeriesDefinitions[1].ItemsSource = items);
            RunIfSelected(this.StackedBar, () => ((StackedBarSeries)this.StackedBar.Series[0]).SeriesDefinitions[2].ItemsSource = items);
            RunIfSelected(this.StackedBar100, () => ((Stacked100BarSeries)this.StackedBar100.Series[0]).SeriesDefinitions[0].ItemsSource = items);
            RunIfSelected(this.StackedBar100, () => ((Stacked100BarSeries)this.StackedBar100.Series[0]).SeriesDefinitions[1].ItemsSource = items);
            RunIfSelected(this.StackedBar100, () => ((Stacked100BarSeries)this.StackedBar100.Series[0]).SeriesDefinitions[2].ItemsSource = items);

            RunIfSelected(this.StackedColumn, () => ((StackedColumnSeries)this.StackedColumn.Series[0]).SeriesDefinitions[0].ItemsSource = items);
            RunIfSelected(this.StackedColumn, () => ((StackedColumnSeries)this.StackedColumn.Series[0]).SeriesDefinitions[1].ItemsSource = items);
            RunIfSelected(this.StackedColumn, () => ((StackedColumnSeries)this.StackedColumn.Series[0]).SeriesDefinitions[2].ItemsSource = items);

            RunIfSelected(this.StackedColumn100, () => ((Stacked100ColumnSeries)this.StackedColumn100.Series[0]).SeriesDefinitions[0].ItemsSource = items);
            RunIfSelected(this.StackedColumn100, () => ((Stacked100ColumnSeries)this.StackedColumn100.Series[0]).SeriesDefinitions[1].ItemsSource = items);
            RunIfSelected(this.StackedColumn100, () => ((Stacked100ColumnSeries)this.StackedColumn100.Series[0]).SeriesDefinitions[2].ItemsSource = items);

            RunIfSelected(this.PieChart, () => ((PieSeries)this.PieChart.Series[0]).ItemsSource = items);;
            RunIfSelected(this.PieChartWithCustomDesign, () => ((PieSeries)this.PieChartWithCustomDesign.Series[0]).ItemsSource = items);;
            RunIfSelected(this.LineChartWithAxes,
                () =>
                {
                    ((LineSeries)LineChartWithAxes.Series[0]).ItemsSource = items;
                    ((LineSeries)LineChartWithAxes.Series[0]).DependentRangeAxis =
                        new LinearAxis
                        {
                            Minimum = 0,
                            Maximum = 100,
                            Orientation = AxisOrientation.Y,
                            Interval = 20,
                            ShowGridLines = true
                        };
                });
        }

        public class NameValueItem
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        private void OnUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            this.ThrottledUpdate();
        }

        private void ThrottledUpdate()
        {
            _updateThrottler.Run(
                async () =>
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    this.UpdateCharts();
                    sw.Stop();
                    await Task.Delay(sw.Elapsed);
                });
        }

        private void NumberOfIitemsNumericUpDown_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            this.ThrottledUpdate();
        }

        private void ChartsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ThrottledUpdate();
        }
    }
}
