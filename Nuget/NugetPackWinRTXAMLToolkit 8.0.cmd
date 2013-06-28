set PATH=%PATH%;C:\Windows\Microsoft.NET\Framework\v4.0.30319
set VisualStudioVersion=11.0

if "%1"=="nobuild" (goto CREATE_FOLDER_STRUCTURE)

msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit 8.0\WinRTXamlToolkit 8.0.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Composition 8.0\WinRTXamlToolkit.Composition 8.0.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Calendar 8.0\WinRTXamlToolkit.Controls.Calendar 8.0.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Gauge 8.0\WinRTXamlToolkit.Controls.Gauge 8.0.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\WinRTXamlToolkit.Controls.DataVisualization 8.0.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Sample 8.0\WinRTXamlToolkit.Sample 8.0.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Debug "..\WinRTXamlToolkit.Debugging 8.0\WinRTXamlToolkit.Debugging 8.0.csproj"
pause

:CREATE_FOLDER_STRUCTURE
@rem Base folder structure
mkdir lib
mkdir lib\netcore451
mkdir tools
mkdir content
mkdir content\controllers

@rem WinRTXamlToolkit folders
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\AlternativeFrame"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\CameraCaptureControl"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\CascadingImageControl"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\ColorPicker"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\CustomAppBar"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\CustomGridSplitter"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\DelayedLoadControl"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\ImageButton"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\ImageToggleButton"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\InputDialog"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\ListItemButt"on
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\NumericUpDown"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\TreeView"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\WatermarkPasswordBox"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\WatermarkTextBox"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Controls\WebBrowser"
mkdir "lib\netcore451\WinRTXamlToolkit 8.0\Themes"

@rem WinRTXamlToolkit.Composition folders
mkdir "lib\netcore451\WinRTXamlToolkit.Composition 8.0"

@rem WinRTXamlToolkit.Controls.Calendar folders
mkdir lib\netcore451\Properties
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.Calendar 8.0"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.Calendar 8.0\Controls"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.Calendar 8.0\Controls\Calendar"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.Calendar 8.0\Themes"

@rem WinRTXamlToolkit.Controls.Gauge folders
mkdir lib\netcore451\Properties
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.Gauge 8.0"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.Gauge 8.0\Themes"

@rem WinRTXamlToolkit.Controls.DataVisualization folders
mkdir lib\netcore451\Properties
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Axis
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Chart
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\DataPoint
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Primitives
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Series
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Legend
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Properties
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Themes
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Title

@rem WinRTXamlToolkit.Debugging folders
mkdir "lib\netcore451\WinRTXamlToolkit.Debugging 8.0\Controls\EditableListBox"
mkdir "lib\netcore451\WinRTXamlToolkit.Debugging 8.0\Themes"
mkdir "lib\netcore451\WinRTXamlToolkit.Debugging 8.0\Views\PropertyEditors"

:COPY_FILES
@rem copy "..\src\SomeController.cs" content
copy "..\WinRTXamlToolkit 8.0\bin\Release\WinRTXamlToolkit.*" lib\netcore451
copy "..\WinRTXamlToolkit.Composition 8.0\bin\Release\WinRTXamlToolkit.Composition.*" lib\netcore451
copy "..\WinRTXamlToolkit.Controls.Calendar 8.0\bin\Release\WinRTXamlToolkit.Controls.Calendar.*" lib\netcore451
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\WinRTXamlToolkit.Controls.DataVisualization.*" lib\netcore451
copy "..\WinRTXamlToolkit.Controls.Gauge 8.0\bin\Release\WinRTXamlToolkit.Controls.Gauge.*" lib\netcore451
copy "..\WinRTXamlToolkit.Debugging 8.0\bin\Debug\WinRTXamlToolkit.Debugging.*" lib\netcore451

copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls"
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\AlternativeFrame\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\AlternativeFrame
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\CameraCaptureControl\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\CameraCaptureControl
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\CascadingImageControl\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\CascadingImageControl
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\ColorPicker\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\ColorPicker
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\CustomAppBar\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\CustomAppBar
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\CustomGridSplitter\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\CustomGridSplitter
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\DelayedLoadControl\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\DelayedLoadControl
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\ImageButton\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\ImageButton
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\ImageToggleButton\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\ImageToggleButton
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\InputDialog\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\InputDialog
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\ListItemButton\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\ListItemButton
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\NumericUpDown\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\NumericUpDown
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\TreeView\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\TreeView
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\WatermarkPasswordBox\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\WatermarkPasswordBox
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\WatermarkTextBox\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\WatermarkTextBox
copy "..\WinRTXamlToolkit 8.0\bin\Release\Controls\WebBrowser\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Controls\WebBrowser
copy "..\WinRTXamlToolkit 8.0\bin\Release\Themes\*.xaml lib\netcore451\WinRTXamlToolkit 8.0\Themes

copy "..\WinRTXamlToolkit.Controls.Calendar 8.0\bin\Release\Controls\Calendar\*.xaml								lib\netcore451\WinRTXamlToolkit.Controls.Calendar 8.0\Controls\Calendar
copy "..\WinRTXamlToolkit.Controls.Calendar 8.0\bin\Release\Themes\*.xaml										lib\netcore451\WinRTXamlToolkit.Controls.Calendar 8.0\Themes

copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\Axis\AxisLabel.xaml				lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Axis
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\Axis\DateTimeAxisLabel.xaml		lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Axis
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\Axis\DisplayAxis.xaml				lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Axis
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\Axis\NumericAxisLabel.xaml			lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Axis
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\Axis\RangeAxis.xaml				lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Axis
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\Chart\Chart.xaml					lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Chart
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\DataPoint\AreaDataPoint.xaml		lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\DataPoint
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\DataPoint\BarDataPoint.xaml		lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\DataPoint
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\DataPoint\BubbleDataPoint.xaml		lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\DataPoint
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\DataPoint\ColumnDataPoint.xaml		lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\DataPoint
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\DataPoint\LineDataPoint.xaml		lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\DataPoint
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\DataPoint\PieDataPoint.xaml		lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\DataPoint
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\DataPoint\ScatterDataPoint.xaml	lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\DataPoint
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\Primitives\DelegatingListBox.xaml	lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Primitives
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\Series\AreaSeries.xaml				lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Series
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\Series\DataPointSeries.xaml		lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Series
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\Series\DefinitionSeries.xaml		lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Series
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\Series\LegendItem.xaml				lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Series
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\Series\LineSeries.xaml				lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Series
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Charting\Series\PieSeries.xaml				lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Charting\Series
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Legend\Legend.xaml							lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Legend
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Themes\Generic.xaml							lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Themes
copy "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\bin\Release\Title\Title.xaml							lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization 8.0\Title

copy "..\WinRTXamlToolkit.Controls.Gauge 8.0\bin\Release\Themes\*.xaml										lib\netcore451\WinRTXamlToolkit.Controls.Gauge 8.0\Themes

copy "..\WinRTXamlToolkit.Debugging 8.0\bin\Debug\Controls\EditableListBox\*.xaml								lib\netcore451\WinRTXamlToolkit.Debugging 8.0\Controls\EditableListBox
copy "..\WinRTXamlToolkit.Debugging 8.0\bin\Debug\Themes\*.xaml													lib\netcore451\WinRTXamlToolkit.Debugging 8.0\Themes
copy "..\WinRTXamlToolkit.Debugging 8.0\bin\Debug\Views\*.xaml													lib\netcore451\WinRTXamlToolkit.Debugging 8.0\Views
copy "..\WinRTXamlToolkit.Debugging 8.0\bin\Debug\Views\PropertyEditors\*.xaml									lib\netcore451\WinRTXamlToolkit.Debugging 8.0\Views\PropertyEditors

@rem copy "..\src\SomePowershellScript.ps1 tools

nuget pack WinRTXamlToolkit.nuspec
nuget pack WinRTXamlToolkit.Composition.nuspec
nuget pack WinRTXamlToolkit.Controls.DataVisualization.nuspec
nuget pack WinRTXamlToolkit.Controls.Calendar.nuspec
nuget pack WinRTXamlToolkit.Controls.Gauge.nuspec
nuget pack WinRTXamlToolkit.Debugging.nuspec
