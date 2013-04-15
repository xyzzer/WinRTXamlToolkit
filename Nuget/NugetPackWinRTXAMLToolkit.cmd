set PATH=%PATH%;C:\Windows\Microsoft.NET\Framework\v4.0.30319
set VisualStudioVersion=11.0

if "%1"=="nobuild" (goto CREATE_FOLDER_STRUCTURE)

msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release ..\WinRTXamlToolkit\WinRTXamlToolkit.csproj
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release ..\WinRTXamlToolkit.Composition\WinRTXamlToolkit.Composition.csproj
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release ..\WinRTXamlToolkit.Controls.DataVisualization\WinRTXamlToolkit.Controls.DataVisualization.csproj
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release ..\WinRTXamlToolkit.Sample\WinRTXamlToolkit.Sample.csproj
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Debug ..\WinRTXamlToolkit.Debugging\WinRTXamlToolkit.Debugging.csproj
pause

:CREATE_FOLDER_STRUCTURE
@rem Base folder structure
mkdir lib
mkdir lib\netcore45
mkdir tools
mkdir content
mkdir content\controllers

@rem WinRTXamlToolkit folders
mkdir lib\netcore45\WinRTXamlToolkit\Controls
mkdir lib\netcore45\WinRTXamlToolkit\Controls\CameraCaptureControl
mkdir lib\netcore45\WinRTXamlToolkit\Controls\CascadingImageControl
mkdir lib\netcore45\WinRTXamlToolkit\Controls\ColorPicker
mkdir lib\netcore45\WinRTXamlToolkit\Controls\CustomAppBar
mkdir lib\netcore45\WinRTXamlToolkit\Controls\DelayedLoadControl
mkdir lib\netcore45\WinRTXamlToolkit\Controls\ImageButton
mkdir lib\netcore45\WinRTXamlToolkit\Controls\ImageToggleButton
mkdir lib\netcore45\WinRTXamlToolkit\Controls\InputDialog
mkdir lib\netcore45\WinRTXamlToolkit\Controls\NumericUpDown
mkdir lib\netcore45\WinRTXamlToolkit\Controls\TreeView
mkdir lib\netcore45\WinRTXamlToolkit\Controls\WatermarkTextBox
mkdir lib\netcore45\WinRTXamlToolkit\Controls\WebBrowser
mkdir lib\netcore45\WinRTXamlToolkit\Themes

@rem WinRTXamlToolkit.Composition folders
mkdir lib\netcore45\WinRTXamlToolkit.Composition

@rem WinRTXamlToolkit.Controls.DataVisualization folders
mkdir lib\netcore45\Properties
mkdir lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization
mkdir lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting
mkdir lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis
mkdir lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Chart
mkdir lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint
mkdir lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Primitives
mkdir lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series
mkdir lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Legend
mkdir lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Properties
mkdir lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Themes
mkdir lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Title

@rem WinRTXamlToolkit.Debugging folders
mkdir lib\netcore45\WinRTXamlToolkit.Debugging\Themes

:COPY_FILES
@rem copy ..\src\SomeController.cs content
copy ..\WinRTXamlToolkit\bin\Release\WinRTXamlToolkit.* lib\netcore45
copy ..\WinRTXamlToolkit.Composition\bin\Release\WinRTXamlToolkit.Composition.* lib\netcore45
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\WinRTXamlToolkit.Controls.DataVisualization.* lib\netcore45
copy ..\WinRTXamlToolkit.Debugging\bin\Debug\WinRTXamlToolkit.Debugging.* lib\netcore45
copy ..\WinRTXamlToolkit\bin\Release\Controls\*.xaml lib\netcore45\WinRTXamlToolkit\Controls
copy ..\WinRTXamlToolkit\bin\Release\Controls\CameraCaptureControl\*.xaml lib\netcore45\WinRTXamlToolkit\Controls\CameraCaptureControl
copy ..\WinRTXamlToolkit\bin\Release\Controls\CascadingImageControl\*.xaml lib\netcore45\WinRTXamlToolkit\Controls\CascadingImageControl
copy ..\WinRTXamlToolkit\bin\Release\Controls\ColorPicker\*.xaml lib\netcore45\WinRTXamlToolkit\Controls\ColorPicker
copy ..\WinRTXamlToolkit\bin\Release\Controls\CustomAppBar\*.xaml lib\netcore45\WinRTXamlToolkit\Controls\CustomAppBar
copy ..\WinRTXamlToolkit\bin\Release\Controls\DelayedLoadControl\*.xaml lib\netcore45\WinRTXamlToolkit\Controls\DelayedLoadControl
copy ..\WinRTXamlToolkit\bin\Release\Controls\ImageButton\*.xaml lib\netcore45\WinRTXamlToolkit\Controls\ImageButton
copy ..\WinRTXamlToolkit\bin\Release\Controls\ImageToggleButton\*.xaml lib\netcore45\WinRTXamlToolkit\Controls\ImageToggleButton
copy ..\WinRTXamlToolkit\bin\Release\Controls\InputDialog\*.xaml lib\netcore45\WinRTXamlToolkit\Controls\InputDialog
copy ..\WinRTXamlToolkit\bin\Release\Controls\NumericUpDown\*.xaml lib\netcore45\WinRTXamlToolkit\Controls\NumericUpDown
copy ..\WinRTXamlToolkit\bin\Release\Controls\TreeView\*.xaml lib\netcore45\WinRTXamlToolkit\Controls\TreeView
copy ..\WinRTXamlToolkit\bin\Release\Controls\WatermarkTextBox\*.xaml lib\netcore45\WinRTXamlToolkit\Controls\WatermarkTextBox
copy ..\WinRTXamlToolkit\bin\Release\Controls\WebBrowser\*.xaml lib\netcore45\WinRTXamlToolkit\Controls\WebBrowser
copy ..\WinRTXamlToolkit\bin\Release\Themes\*.xaml lib\netcore45\WinRTXamlToolkit\Themes
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Axis\AxisLabel.xaml				lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Axis\DateTimeAxisLabel.xaml		lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Axis\DisplayAxis.xaml				lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Axis\NumericAxisLabel.xaml			lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Axis\RangeAxis.xaml				lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Chart\Chart.xaml					lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Chart
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\DataPoint\AreaDataPoint.xaml		lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\DataPoint\BarDataPoint.xaml		lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\DataPoint\BubbleDataPoint.xaml		lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\DataPoint\ColumnDataPoint.xaml		lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\DataPoint\LineDataPoint.xaml		lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\DataPoint\PieDataPoint.xaml		lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\DataPoint\ScatterDataPoint.xaml	lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Primitives\DelegatingListBox.xaml	lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Primitives
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Series\AreaSeries.xaml				lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Series\DataPointSeries.xaml		lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Series\DefinitionSeries.xaml		lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Series\LegendItem.xaml				lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Series\LineSeries.xaml				lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Series\PieSeries.xaml				lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Legend\Legend.xaml							lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Legend
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Themes\Generic.xaml							lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Themes
copy ..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Title\Title.xaml							lib\netcore45\WinRTXamlToolkit.Controls.DataVisualization\Title
copy ..\WinRTXamlToolkit.Debugging\bin\Debug\Themes\*.xaml													lib\netcore45\WinRTXamlToolkit.Debugging\Themes
@rem copy ..\src\SomePowershellScript.ps1 tools

nuget pack WinRTXamlToolkit.nuspec
nuget pack WinRTXamlToolkit.Composition.nuspec
nuget pack WinRTXamlToolkit.Controls.DataVisualization.nuspec
nuget pack WinRTXamlToolkit.Debugging.nuspec
