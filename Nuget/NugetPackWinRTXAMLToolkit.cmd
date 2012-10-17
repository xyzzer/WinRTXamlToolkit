set PATH=%PATH%;C:\Windows\Microsoft.NET\Framework\v4.0.30319
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release ..\WinRTXamlToolkit\WinRTXamlToolkit.csproj
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Debug ..\WinRTXamlToolkit.Debugging\WinRTXamlToolkit.Debugging.csproj
pause

mkdir lib
mkdir lib\winrt45
mkdir lib\winrt45\WinRTXamlToolkit\Controls
mkdir lib\winrt45\WinRTXamlToolkit\Themes
mkdir lib\winrt45\Controls\DataVisualization
mkdir lib\winrt45\Controls\ColorPicker
mkdir lib\winrt45\Controls\WebBrowser
mkdir lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting
mkdir lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Axis
mkdir lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Chart
mkdir lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\DataPoint
mkdir lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Primitives
mkdir lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Series
mkdir lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Legend
mkdir lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Themes
mkdir lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Title
mkdir lib\winrt45\WinRTXamlToolkit.Debugging\Themes
mkdir tools
mkdir content
mkdir content\controllers

@rem copy ..\src\SomeController.cs content
copy ..\WinRTXamlToolkit\bin\Release\WinRTXamlToolkit.* lib\winrt45
copy ..\WinRTXamlToolkit.Debugging\bin\Debug\WinRTXamlToolkit.Debugging.* lib\winrt45
@rem copy ..\WinRTXamlToolkit\bin\Release\resources.pri lib\winrt45
copy ..\WinRTXamlToolkit.Sample\bin\Release\resources.pri lib\winrt45
copy ..\WinRTXamlToolkit.Sample\bin\Release\resources.pri lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization
copy ..\WinRTXamlToolkit\Controls\DataVisualization\Resources.resx lib\winrt45\Controls\DataVisualization
copy ..\WinRTXamlToolkit\Controls\DataVisualization\Resources.resx lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization
copy ..\WinRTXamlToolkit\bin\Release\Controls\*.xaml lib\winrt45\WinRTXamlToolkit\Controls
copy ..\WinRTXamlToolkit\bin\Release\Controls\ColorPicker\*.xaml lib\winrt45\WinRTXamlToolkit\Controls\ColorPicker
copy ..\WinRTXamlToolkit\bin\Release\Controls\WebBrowser\*.xaml lib\winrt45\WinRTXamlToolkit\Controls\WebBrowser
copy ..\WinRTXamlToolkit\bin\Release\Themes\*.xaml lib\winrt45\WinRTXamlToolkit\Themes
@rem copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\DataPointSeriesDragDropTarget.xaml	lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\Axis\AxisLabel.xaml				lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Axis
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\Axis\DateTimeAxisLabel.xaml		lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Axis
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\Axis\DisplayAxis.xaml				lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Axis
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\Axis\NumericAxisLabel.xaml			lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Axis
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\Axis\RangeAxis.xaml				lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Axis
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\Chart\Chart.xaml					lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Chart
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\DataPoint\AreaDataPoint.xaml		lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\DataPoint
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\DataPoint\BarDataPoint.xaml		lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\DataPoint
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\DataPoint\BubbleDataPoint.xaml		lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\DataPoint
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\DataPoint\ColumnDataPoint.xaml		lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\DataPoint
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\DataPoint\LineDataPoint.xaml		lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\DataPoint
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\DataPoint\PieDataPoint.xaml		lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\DataPoint
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\DataPoint\ScatterDataPoint.xaml	lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\DataPoint
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\Primitives\DelegatingListBox.xaml	lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Primitives
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\Series\AreaSeries.xaml				lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Series
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\Series\DataPointSeries.xaml		lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Series
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\Series\DefinitionSeries.xaml		lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Series
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\Series\LegendItem.xaml				lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Series
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\Series\LineSeries.xaml				lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Series
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Charting\Series\PieSeries.xaml				lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Charting\Series
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Legend\Legend.xaml							lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Legend
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Themes\Generic.xaml							lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Themes
copy ..\WinRTXamlToolkit\bin\Release\Controls\DataVisualization\Title\Title.xaml							lib\winrt45\WinRTXamlToolkit\Controls\DataVisualization\Title
copy ..\WinRTXamlToolkit.Debugging\bin\Debug\Themes\*.xaml lib\winrt45\WinRTXamlToolkit.Debugging\Themes
@rem copy ..\src\SomePowershellScript.ps1 tools

nuget pack WinRTXamlToolkit.nuspec
nuget pack WinRTXamlToolkit.Debugging.nuspec