set PATH=%PATH%;C:\Windows\Microsoft.NET\Framework\v4.0.30319

if "%1"=="nobuild" (goto CREATE_FOLDER_STRUCTURE)

@echo Building Windows 8.0 projects
set VisualStudioVersion=11.0
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit 8.0\WinRTXamlToolkit 8.0.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Composition 8.0\WinRTXamlToolkit.Composition 8.0.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Calendar 8.0\WinRTXamlToolkit.Controls.Calendar 8.0.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Gauge 8.0\WinRTXamlToolkit.Controls.Gauge 8.0.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\WinRTXamlToolkit.Controls.DataVisualization 8.0.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Sample 8.0\WinRTXamlToolkit.Sample 8.0.csproj"
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Debug "..\WinRTXamlToolkit.Debugging 8.0\WinRTXamlToolkit.Debugging 8.0.csproj"

@echo Building Windows 8.1 projects
set VisualStudioVersion=12.0
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
mkdir tools
mkdir content
mkdir content\controllers

@echo Copying Windows 8.0 build
set NUGET_PLATFORM=netcore45
set FOLDER_SUFFIX= 8.0\
set XAML_EXT=.xaml
@CALL :COPY_FILES

@echo Copying Windows 8.1 build
set NUGET_PLATFORM=netcore451
set FOLDER_SUFFIX=\
set XAML_EXT=.xbf
@CALL :COPY_FILES

@GOTO :PACK_FILES

:COPY_FILES
@rem WinRTXamlToolkit folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\AlternativeFrame"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CameraCaptureControl"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CascadingImageControl"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ColorPicker"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CustomAppBar"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CustomGridSplitter"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\DelayedLoadControl"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ImageButton"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ImageToggleButton"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\InputDialog"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ListItemButt"on
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\NumericUpDown"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\TreeView"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WatermarkPasswordBox"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WatermarkTextBox"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WebBrowser"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Themes"

@rem WinRTXamlToolkit.Composition folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Composition"

@rem WinRTXamlToolkit.Controls.Calendar folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\Controls"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\Controls\Calendar"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\Themes"

@rem WinRTXamlToolkit.Controls.Gauge folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge\Themes"

@rem WinRTXamlToolkit.Controls.DataVisualization folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Chart"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Primitives"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Legend"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Themes"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Title"

@rem WinRTXamlToolkit.Debugging folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Controls\EditableListBox"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Themes"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Views\PropertyEditors"

:COPY_FILES
@rem copy "..\src\SomeController.cs" content
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\WinRTXamlToolkit.*" "lib\%NUGET_PLATFORM%"
copy "..\WinRTXamlToolkit.Composition%FOLDER_SUFFIX%bin\Release\WinRTXamlToolkit.Composition.*" "lib\%NUGET_PLATFORM%"
copy "..\WinRTXamlToolkit.Controls.Calendar%FOLDER_SUFFIX%bin\Release\WinRTXamlToolkit.Controls.Calendar.*" "lib\%NUGET_PLATFORM%"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\WinRTXamlToolkit.Controls.DataVisualization.*" "lib\%NUGET_PLATFORM%"
copy "..\WinRTXamlToolkit.Controls.Gauge%FOLDER_SUFFIX%bin\Release\WinRTXamlToolkit.Controls.Gauge.*" "lib\%NUGET_PLATFORM%"
copy "..\WinRTXamlToolkit.Debugging%FOLDER_SUFFIX%bin\Debug\WinRTXamlToolkit.Debugging.*" "lib\%NUGET_PLATFORM%"

copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\AlternativeFrame\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\AlternativeFrame"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\CameraCaptureControl\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CameraCaptureControl"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\CascadingImageControl\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CascadingImageControl"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\ColorPicker\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ColorPicker"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\CustomAppBar\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CustomAppBar"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\CustomGridSplitter\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CustomGridSplitter"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\DelayedLoadControl\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\DelayedLoadControl"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\ImageButton\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ImageButton"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\ImageToggleButton\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ImageToggleButton"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\InputDialog\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\InputDialog"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\ListItemButton\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ListItemButton"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\NumericUpDown\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\NumericUpDown"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\TreeView\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\TreeView"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\WatermarkPasswordBox\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WatermarkPasswordBox"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\WatermarkTextBox\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WatermarkTextBox"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\WebBrowser\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WebBrowser"
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Themes\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Themes"

copy "..\WinRTXamlToolkit.Controls.Calendar%FOLDER_SUFFIX%bin\Release\Controls\Calendar\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\Controls\Calendar"
copy "..\WinRTXamlToolkit.Controls.Calendar%FOLDER_SUFFIX%bin\Release\Themes\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\Themes"

copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Axis\AxisLabel%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Axis\DateTimeAxisLabel%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Axis\DisplayAxis%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Axis\NumericAxisLabel%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Axis\RangeAxis%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Chart\Chart%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Chart"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\DataPoint\AreaDataPoint%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\DataPoint\BarDataPoint%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\DataPoint\BubbleDataPoint%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\DataPoint\ColumnDataPoint%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\DataPoint\LineDataPoint%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\DataPoint\PieDataPoint%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\DataPoint\ScatterDataPoint%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Primitives\DelegatingListBox%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Primitives"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Series\AreaSeries%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Series\DataPointSeries%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Series\DefinitionSeries%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Series\LegendItem%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Series\LineSeries%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Series\PieSeries%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Legend\Legend%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Legend"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Themes\Generic%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Themes"
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Title\Title%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Title"

copy "..\WinRTXamlToolkit.Controls.Gauge%FOLDER_SUFFIX%bin\Release\Themes\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge\Themes"

copy "..\WinRTXamlToolkit.Debugging%FOLDER_SUFFIX%bin\Debug\Controls\EditableListBox\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Controls\EditableListBox"
copy "..\WinRTXamlToolkit.Debugging%FOLDER_SUFFIX%bin\Debug\Themes\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Themes"
copy "..\WinRTXamlToolkit.Debugging%FOLDER_SUFFIX%bin\Debug\Views\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Views"
copy "..\WinRTXamlToolkit.Debugging%FOLDER_SUFFIX%bin\Debug\Views\PropertyEditors\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Views\PropertyEditors"
@rem copy "..\src\SomePowershellScript.ps1 tools
@GOTO :EOF

:PACK_FILES
@echo Packing NuGets
nuget pack "WinRTXamlToolkit.nuspec"
nuget pack "WinRTXamlToolkit.Composition.nuspec"
nuget pack "WinRTXamlToolkit.Controls.DataVisualization.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Calendar.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Gauge.nuspec"
nuget pack "WinRTXamlToolkit.Debugging.nuspec"
