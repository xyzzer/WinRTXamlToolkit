using System;
using System.Collections.Generic;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

        private void UpdateCharts()
        {
            if (!this.isInitialized)
            {
                return;
            }

            List<NameValueItem> items = new List<NameValueItem>();

            for (int i = 0; i < NumberOfItemsSlider.Value; i++)
            {
                items.Add(new NameValueItem { Name = "Test" + i, Value = _random.Next(10, 100) });
            }

            ((ColumnSeries)this.Chart.Series[0]).ItemsSource = items;
            ((BarSeries)this.BarChart.Series[0]).ItemsSource = items;
            ((LineSeries)this.LineChart.Series[0]).ItemsSource = items;
            ((LineSeries)this.LineChart2.Series[0]).ItemsSource = items;
            ((ColumnSeries)this.MixedChart.Series[0]).ItemsSource = items;
            ((LineSeries)this.MixedChart.Series[1]).ItemsSource = items;
            ((AreaSeries)this.AreaChart.Series[0]).ItemsSource = items;
            var series = ((AreaSeries)this.AreaChartWithNoLabels.Series[0]);
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
            ((BubbleSeries)this.BubbleChart.Series[0]).ItemsSource = items;
            ((ScatterSeries)this.ScatterChart.Series[0]).ItemsSource = items;
            ((StackedBarSeries)this.StackedBar.Series[0]).SeriesDefinitions[0].ItemsSource = items;
            ((StackedBarSeries)this.StackedBar.Series[0]).SeriesDefinitions[1].ItemsSource = items;
            ((StackedBarSeries)this.StackedBar.Series[0]).SeriesDefinitions[2].ItemsSource = items;
            ((Stacked100BarSeries)this.StackedBar100.Series[0]).SeriesDefinitions[0].ItemsSource =
                items;
            ((Stacked100BarSeries)this.StackedBar100.Series[0]).SeriesDefinitions[1].ItemsSource =
                items;
            ((Stacked100BarSeries)this.StackedBar100.Series[0]).SeriesDefinitions[2].ItemsSource =
                items;

            ((StackedColumnSeries)this.StackedColumn.Series[0]).SeriesDefinitions[0].ItemsSource =
                items;
            ((StackedColumnSeries)this.StackedColumn.Series[0]).SeriesDefinitions[1].ItemsSource =
                items;
            ((StackedColumnSeries)this.StackedColumn.Series[0]).SeriesDefinitions[2].ItemsSource =
                items;

            ((Stacked100ColumnSeries)this.StackedColumn100.Series[0]).SeriesDefinitions[0]
                .ItemsSource = items;
            ((Stacked100ColumnSeries)this.StackedColumn100.Series[0]).SeriesDefinitions[1]
                .ItemsSource = items;
            ((Stacked100ColumnSeries)this.StackedColumn100.Series[0]).SeriesDefinitions[2]
                .ItemsSource = items;

            ((PieSeries)this.PieChart.Series[0]).ItemsSource = items;
            ((PieSeries)this.PieChartWithCustomDesign.Series[0]).ItemsSource = items;
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
        }

        public class NameValueItem
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        private void OnUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            UpdateCharts();
        }

        private void NumberOfItemsSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            UpdateCharts();
        }
    }
}
