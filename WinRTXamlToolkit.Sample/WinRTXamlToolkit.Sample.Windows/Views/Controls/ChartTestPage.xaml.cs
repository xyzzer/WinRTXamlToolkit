using System;
using System.Collections.Generic;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ChartTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public ChartTestPage()
        {
            this.InitializeComponent();

            UpdateCharts();
        }

        private Random _random = new Random();

        private void UpdateCharts()
        {
            List<NameValueItem> items = new List<NameValueItem>();
            items.Add(new NameValueItem { Name = "Test1", Value = _random.Next(10, 100) });
            items.Add(new NameValueItem { Name = "Test2", Value = _random.Next(10, 100) });
            items.Add(new NameValueItem { Name = "Test3", Value = _random.Next(10, 100) });
            items.Add(new NameValueItem { Name = "Test4", Value = _random.Next(10, 100) });
            items.Add(new NameValueItem { Name = "Test5", Value = _random.Next(10, 100) });

            ((ColumnSeries)this.Chart.Series[0]).ItemsSource = items;
            ((BarSeries)this.BarChart.Series[0]).ItemsSource = items;
            ((LineSeries)this.LineChart.Series[0]).ItemsSource = items;
            ((LineSeries)this.LineChart2.Series[0]).ItemsSource = items;
            ((ColumnSeries)this.MixedChart.Series[0]).ItemsSource = items;
            ((LineSeries)this.MixedChart.Series[1]).ItemsSource = items;
            ((AreaSeries)this.AreaChart.Series[0]).ItemsSource = items;
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

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
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
    }
}
