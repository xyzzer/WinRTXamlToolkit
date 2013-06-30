set PATH=%PATH%;C:\Windows\Microsoft.NET\Framework\v4.0.30319
set VisualStudioVersion=11.0

if "%1"=="nobuild" (goto CREATE_FOLDER_STRUCTURE)

msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit\WinRTXamlToolkit.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Composition\WinRTXamlToolkit.Composition.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Calendar\WinRTXamlToolkit.Controls.Calendar.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Gauge\WinRTXamlToolkit.Controls.Gauge.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.DataVisualization\WinRTXamlToolkit.Controls.DataVisualization.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Sample\WinRTXamlToolkit.Sample.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Debug "..\WinRTXamlToolkit.Debugging\WinRTXamlToolkit.Debugging.csproj"

:CREATE_FOLDER_STRUCTURE
@rem Base folder structure
mkdir lib
mkdir lib\netcore451
mkdir tools
mkdir content
mkdir content\controllers

@rem WinRTXamlToolkit folders
mkdir "lib\netcore451\WinRTXamlToolkit\Controls"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\AlternativeFrame"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\CameraCaptureControl"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\CascadingImageControl"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\ColorPicker"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\CustomAppBar"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\CustomGridSplitter"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\DelayedLoadControl"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\ImageButton"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\ImageToggleButton"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\InputDialog"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\ListItemButt"on
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\NumericUpDown"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\TreeView"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\WatermarkPasswordBox"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\WatermarkTextBox"
mkdir "lib\netcore451\WinRTXamlToolkit\Controls\WebBrowser"
mkdir "lib\netcore451\WinRTXamlToolkit\Themes"

@rem WinRTXamlToolkit.Composition folders
mkdir "lib\netcore451\WinRTXamlToolkit.Composition"

@rem WinRTXamlToolkit.Controls.Calendar folders
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.Calendar"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.Calendar\Controls"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.Calendar\Controls\Calendar"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.Calendar\Themes"

@rem WinRTXamlToolkit.Controls.Gauge folders
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.Gauge"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.Gauge\Themes"

@rem WinRTXamlToolkit.Controls.DataVisualization folders
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Chart"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Primitives"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Legend"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Themes"
mkdir "lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Title"

@rem WinRTXamlToolkit.Debugging folders
mkdir "lib\netcore451\WinRTXamlToolkit.Debugging\Controls\EditableListBox"
mkdir "lib\netcore451\WinRTXamlToolkit.Debugging\Themes"
mkdir "lib\netcore451\WinRTXamlToolkit.Debugging\Views\PropertyEditors"

:COPY_FILES
@rem copy "..\src\SomeController.cs" content
copy "..\WinRTXamlToolkit\bin\Release\WinRTXamlToolkit.*" "lib\netcore451"
copy "..\WinRTXamlToolkit.Composition\bin\Release\WinRTXamlToolkit.Composition.*" "lib\netcore451"
copy "..\WinRTXamlToolkit.Controls.Calendar\bin\Release\WinRTXamlToolkit.Controls.Calendar.*" "lib\netcore451"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\WinRTXamlToolkit.Controls.DataVisualization.*" "lib\netcore451"
copy "..\WinRTXamlToolkit.Controls.Gauge\bin\Release\WinRTXamlToolkit.Controls.Gauge.*" "lib\netcore451"
copy "..\WinRTXamlToolkit.Debugging\bin\Debug\WinRTXamlToolkit.Debugging.*" "lib\netcore451"

copy "..\WinRTXamlToolkit\bin\Release\Controls\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls"
copy "..\WinRTXamlToolkit\bin\Release\Controls\AlternativeFrame\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\AlternativeFrame"
copy "..\WinRTXamlToolkit\bin\Release\Controls\CameraCaptureControl\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\CameraCaptureControl"
copy "..\WinRTXamlToolkit\bin\Release\Controls\CascadingImageControl\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\CascadingImageControl"
copy "..\WinRTXamlToolkit\bin\Release\Controls\ColorPicker\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\ColorPicker"
copy "..\WinRTXamlToolkit\bin\Release\Controls\CustomAppBar\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\CustomAppBar"
copy "..\WinRTXamlToolkit\bin\Release\Controls\CustomGridSplitter\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\CustomGridSplitter"
copy "..\WinRTXamlToolkit\bin\Release\Controls\DelayedLoadControl\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\DelayedLoadControl"
copy "..\WinRTXamlToolkit\bin\Release\Controls\ImageButton\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\ImageButton"
copy "..\WinRTXamlToolkit\bin\Release\Controls\ImageToggleButton\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\ImageToggleButton"
copy "..\WinRTXamlToolkit\bin\Release\Controls\InputDialog\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\InputDialog"
copy "..\WinRTXamlToolkit\bin\Release\Controls\ListItemButton\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\ListItemButton"
copy "..\WinRTXamlToolkit\bin\Release\Controls\NumericUpDown\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\NumericUpDown"
copy "..\WinRTXamlToolkit\bin\Release\Controls\TreeView\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\TreeView"
copy "..\WinRTXamlToolkit\bin\Release\Controls\WatermarkPasswordBox\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\WatermarkPasswordBox"
copy "..\WinRTXamlToolkit\bin\Release\Controls\WatermarkTextBox\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\WatermarkTextBox"
copy "..\WinRTXamlToolkit\bin\Release\Controls\WebBrowser\*.xbf" "lib\netcore451\WinRTXamlToolkit\Controls\WebBrowser"
copy "..\WinRTXamlToolkit\bin\Release\Themes\*.xbf" "lib\netcore451\WinRTXamlToolkit\Themes"

copy "..\WinRTXamlToolkit.Controls.Calendar\bin\Release\Controls\Calendar\*.xbf"								"lib\netcore451\WinRTXamlToolkit.Controls.Calendar\Controls\Calendar"
copy "..\WinRTXamlToolkit.Controls.Calendar\bin\Release\Themes\*.xbf"										"lib\netcore451\WinRTXamlToolkit.Controls.Calendar\Themes"

copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Axis\AxisLabel.xbf"				"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Axis\DateTimeAxisLabel.xbf"		"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Axis\DisplayAxis.xbf"				"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Axis\NumericAxisLabel.xbf"			"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Axis\RangeAxis.xbf"				"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Chart\Chart.xbf"					"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Chart"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\DataPoint\AreaDataPoint.xbf"		"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\DataPoint\BarDataPoint.xbf"		"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\DataPoint\BubbleDataPoint.xbf"		"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\DataPoint\ColumnDataPoint.xbf"		"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\DataPoint\LineDataPoint.xbf"		"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\DataPoint\PieDataPoint.xbf"		"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\DataPoint\ScatterDataPoint.xbf"	"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Primitives\DelegatingListBox.xbf"	"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Primitives"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Series\AreaSeries.xbf"				"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Series\DataPointSeries.xbf"		"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Series\DefinitionSeries.xbf"		"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Series\LegendItem.xbf"				"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Series\LineSeries.xbf"				"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Charting\Series\PieSeries.xbf"				"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Legend\Legend.xbf"							"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Legend"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Themes\Generic.xbf"							"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Themes"
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\Title\Title.xbf"							"lib\netcore451\WinRTXamlToolkit.Controls.DataVisualization\Title"

copy "..\WinRTXamlToolkit.Controls.Gauge\bin\Release\Themes\*.xbf"										"lib\netcore451\WinRTXamlToolkit.Controls.Gauge\Themes"

copy "..\WinRTXamlToolkit.Debugging\bin\Debug\Controls\EditableListBox\*.xbf"								"lib\netcore451\WinRTXamlToolkit.Debugging\Controls\EditableListBox"
copy "..\WinRTXamlToolkit.Debugging\bin\Debug\Themes\*.xbf"													"lib\netcore451\WinRTXamlToolkit.Debugging\Themes"
copy "..\WinRTXamlToolkit.Debugging\bin\Debug\Views\*.xbf"													"lib\netcore451\WinRTXamlToolkit.Debugging\Views"
copy "..\WinRTXamlToolkit.Debugging\bin\Debug\Views\PropertyEditors\*.xbf"									"lib\netcore451\WinRTXamlToolkit.Debugging\Views\PropertyEditors"

@rem copy "..\src\SomePowershellScript.ps1 tools

nuget pack "WinRTXamlToolkit.nuspec"
nuget pack "WinRTXamlToolkit.Composition.nuspec"
nuget pack "WinRTXamlToolkit.Controls.DataVisualization.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Calendar.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Gauge.nuspec"
nuget pack "WinRTXamlToolkit.Debugging.nuspec"
