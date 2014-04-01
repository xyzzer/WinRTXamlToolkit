set PATH=%PATH%;C:\Windows\Microsoft.NET\Framework\v4.0.30319

if "%1"=="nobuild" (@GOTO CREATE_FOLDER_STRUCTURE)

@echo Building Windows 8.0 projects
set VisualStudioVersion=12.0
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit 8.0\WinRTXamlToolkit 8.0.csproj" || GOTO :REPORT_ERROR
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Composition 8.0\WinRTXamlToolkit.Composition 8.0.csproj" || GOTO :REPORT_ERROR
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Calendar 8.0\WinRTXamlToolkit.Controls.Calendar 8.0.csproj" || GOTO :REPORT_ERROR
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Gauge 8.0\WinRTXamlToolkit.Controls.Gauge 8.0.csproj" || GOTO :REPORT_ERROR
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.DataVisualization 8.0\WinRTXamlToolkit.Controls.DataVisualization 8.0.csproj" || GOTO :REPORT_ERROR
@rem msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Sample 8.0\WinRTXamlToolkit.Sample 8.0.csproj" || GOTO :REPORT_ERROR
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Debugging 8.0\WinRTXamlToolkit.Debugging 8.0.csproj" || GOTO :REPORT_ERROR

@echo Building Windows 8.1 projects
set VisualStudioVersion=12.0
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit\WinRTXamlToolkit.csproj" || GOTO :REPORT_ERROR
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Composition\WinRTXamlToolkit.Composition.csproj" || GOTO :REPORT_ERROR
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Calendar\WinRTXamlToolkit.Controls.Calendar.csproj" || GOTO :REPORT_ERROR
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Gauge\WinRTXamlToolkit.Controls.Gauge.csproj" || GOTO :REPORT_ERROR
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.DataVisualization\WinRTXamlToolkit.Controls.DataVisualization.csproj" || GOTO :REPORT_ERROR
@rem msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Sample\WinRTXamlToolkit.Sample.csproj" || GOTO :REPORT_ERROR
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Debugging\WinRTXamlToolkit.Debugging.csproj" || GOTO :REPORT_ERROR

:CREATE_FOLDER_STRUCTURE
@rem Base folder structure
rd /Q /S lib
rd /Q /S tools
rd /Q /S content

mkdir lib
mkdir tools
mkdir content
mkdir content\controllers

@echo Copying Windows 8.0 build
set NUGET_PLATFORM=netcore45
set FOLDER_SUFFIX= 8.0\
set XAML_EXT=.xaml
@CALL :COPY_FILES || GOTO :REPORT_ERROR

@echo Copying Windows 8.1 build
set NUGET_PLATFORM=netcore451
set FOLDER_SUFFIX=\
set XAML_EXT=.xbf
@CALL :COPY_FILES || GOTO :REPORT_ERROR

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
if "%NUGET_PLATFORM%" NEQ "netcore45" (mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\FxContentControl")
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ImageButton"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ImageToggleButton"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\InputDialog"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ListItemButton"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\NumericUpDown"
if "%NUGET_PLATFORM%" NEQ "netcore45" (mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ToolWindow")
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
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\WinRTXamlToolkit.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Composition%FOLDER_SUFFIX%bin\Release\WinRTXamlToolkit.Composition.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.Calendar%FOLDER_SUFFIX%bin\Release\WinRTXamlToolkit.Controls.Calendar.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\WinRTXamlToolkit.Controls.DataVisualization.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.Gauge%FOLDER_SUFFIX%bin\Release\WinRTXamlToolkit.Controls.Gauge.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Debugging%FOLDER_SUFFIX%bin\Release\WinRTXamlToolkit.Debugging.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR

copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\AlternativeFrame\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\AlternativeFrame" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\CameraCaptureControl\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CameraCaptureControl" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\CascadingImageControl\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CascadingImageControl" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\ColorPicker\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ColorPicker" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\CustomAppBar\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CustomAppBar" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\CustomGridSplitter\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CustomGridSplitter" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\DelayedLoadControl\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\DelayedLoadControl" || GOTO :REPORT_ERROR
if "%NUGET_PLATFORM%" NEQ "netcore45" (copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\FxContentControl\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\FxContentControl" || GOTO :REPORT_ERROR)
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\ImageButton\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ImageButton" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\ImageToggleButton\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ImageToggleButton" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\InputDialog\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\InputDialog" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\ListItemButton\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ListItemButton" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\NumericUpDown\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\NumericUpDown" || GOTO :REPORT_ERROR
if "%NUGET_PLATFORM%" NEQ "netcore45" (copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\ToolWindow\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ToolWindow" || GOTO :REPORT_ERROR)
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\TreeView\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\TreeView" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\WatermarkPasswordBox\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WatermarkPasswordBox" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\WatermarkTextBox\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WatermarkTextBox" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Controls\WebBrowser\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WebBrowser" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit%FOLDER_SUFFIX%bin\Release\Themes\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Themes" || GOTO :REPORT_ERROR

copy "..\WinRTXamlToolkit.Controls.Calendar%FOLDER_SUFFIX%bin\Release\Controls\Calendar\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\Controls\Calendar" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.Calendar%FOLDER_SUFFIX%bin\Release\Themes\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\Themes" || GOTO :REPORT_ERROR

copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Axis\AxisLabel%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Axis\DateTimeAxisLabel%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Axis\DisplayAxis%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Axis\NumericAxisLabel%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Axis\RangeAxis%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Chart\Chart%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Chart" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\DataPoint\AreaDataPoint%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\DataPoint\BarDataPoint%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\DataPoint\BubbleDataPoint%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\DataPoint\ColumnDataPoint%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\DataPoint\LineDataPoint%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\DataPoint\PieDataPoint%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\DataPoint\ScatterDataPoint%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Primitives\DelegatingListBox%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Primitives" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Series\AreaSeries%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Series\DataPointSeries%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Series\DefinitionSeries%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Series\LegendItem%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Series\LineSeries%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Charting\Series\PieSeries%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Legend\Legend%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Legend" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Themes\Generic%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Themes" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%FOLDER_SUFFIX%bin\Release\Title\Title%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Title" || GOTO :REPORT_ERROR

copy "..\WinRTXamlToolkit.Controls.Gauge%FOLDER_SUFFIX%bin\Release\Themes\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge\Themes" || GOTO :REPORT_ERROR

copy "..\WinRTXamlToolkit.Debugging%FOLDER_SUFFIX%bin\Release\Controls\EditableListBox\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Controls\EditableListBox" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Debugging%FOLDER_SUFFIX%bin\Release\Themes\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Themes" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Debugging%FOLDER_SUFFIX%bin\Release\Views\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Views" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Debugging%FOLDER_SUFFIX%bin\Release\Views\PropertyEditors\*%XAML_EXT%" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Views\PropertyEditors" || GOTO :REPORT_ERROR
@rem copy "..\src\SomePowershellScript.ps1 tools || GOTO :REPORT_ERROR
@GOTO :EOF

:PACK_FILES
@echo Packing NuGets
nuget pack "WinRTXamlToolkit.nuspec"
nuget pack "WinRTXamlToolkit.Composition.nuspec"
nuget pack "WinRTXamlToolkit.Controls.DataVisualization.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Calendar.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Gauge.nuspec"
nuget pack "WinRTXamlToolkit.Debugging.nuspec"
@GOTO :EOF

:REPORT_ERROR
@ECHO Error, see the last command executed.
pause