set PATH=%PATH%;C:\Windows\Microsoft.NET\Framework\v4.0.30319
set MSBUILD="c:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe"

if "%1"=="nobuild" (@GOTO CREATE_FOLDER_STRUCTURE)

set VisualStudioVersion=12.0

@echo Building Windows 8.1 projects
%MSBUILD% /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit\WinRTXamlToolkit.Windows\WinRTXamlToolkit.Windows.csproj" || GOTO :REPORT_ERROR
%MSBUILD% /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Calendar\WinRTXamlToolkit.Controls.Calendar.Windows\WinRTXamlToolkit.Controls.Calendar.Windows.csproj" || GOTO :REPORT_ERROR
%MSBUILD% /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Gauge\WinRTXamlToolkit.Controls.Gauge.Windows\WinRTXamlToolkit.Controls.Gauge.Windows.csproj" || GOTO :REPORT_ERROR
%MSBUILD% /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.DataVisualization\WinRTXamlToolkit.Controls.DataVisualization.Windows\WinRTXamlToolkit.Controls.DataVisualization.Windows.csproj" || GOTO :REPORT_ERROR
%MSBUILD% /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Debugging\WinRTXamlToolkit.Debugging.Windows\WinRTXamlToolkit.Debugging.Windows.csproj" || GOTO :REPORT_ERROR

@echo Building Windows Phone 8.1 projects
%MSBUILD% /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit\WinRTXamlToolkit.WindowsPhone\WinRTXamlToolkit.WindowsPhone.csproj" || GOTO :REPORT_ERROR
%MSBUILD% /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Calendar\WinRTXamlToolkit.Controls.Calendar.WindowsPhone\WinRTXamlToolkit.Controls.Calendar.WindowsPhone.csproj" || GOTO :REPORT_ERROR
%MSBUILD% /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Gauge\WinRTXamlToolkit.Controls.Gauge.WindowsPhone\WinRTXamlToolkit.Controls.Gauge.WindowsPhone.csproj" || GOTO :REPORT_ERROR
%MSBUILD% /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.DataVisualization\WinRTXamlToolkit.Controls.DataVisualization.WindowsPhone\WinRTXamlToolkit.Controls.DataVisualization.WindowsPhone.csproj" || GOTO :REPORT_ERROR
%MSBUILD% /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Debugging\WinRTXamlToolkit.Debugging.WindowsPhone\WinRTXamlToolkit.Debugging.WindowsPhone.csproj" || GOTO :REPORT_ERROR

if "%1"=="onlybuild" ( & @GOTO :EOF)

:CREATE_FOLDER_STRUCTURE
@rem Base folder structure
rd /Q /S lib
rd /Q /S tools
rd /Q /S content

mkdir lib
mkdir tools
mkdir content
mkdir content\controllers

@echo Copying Windows 8.1 build
set NUGET_PLATFORM=netcore451
set PLATFORM_SUFFIX=.Windows
@CALL :COPY_FILES || GOTO :REPORT_ERROR

@echo Copying Windows Phone 8.1 build
set NUGET_PLATFORM=wpa
set PLATFORM_SUFFIX=.WindowsPhone
@CALL :COPY_FILES || GOTO :REPORT_ERROR

@GOTO :PACK_FILES

:COPY_FILES
@rem WinRTXamlToolkit folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\AlternativeFrame"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\AnimatingContainer"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\CameraCaptureControl"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\CascadingImageControl"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\CascadingTextBlock"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\ColorPicker"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\CountdownControl"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\CustomAppBar"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\CustomGridSplitter"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\DelayedLoadControl"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\FxContentControl"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\ImageButton"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\ImageToggleButton"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\InputDialog"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\ListItemButton"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\NumericUpDown"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\ToolWindow"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\TreeView"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\WatermarkPasswordBox"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\WatermarkTextBox"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\WebBrowser"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Themes"

@rem WinRTXamlToolkit.Controls.Calendar folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar%PLATFORM_SUFFIX%\"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar%PLATFORM_SUFFIX%\Controls"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar%PLATFORM_SUFFIX%\Controls\Calendar"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar%PLATFORM_SUFFIX%\Themes"

@rem WinRTXamlToolkit.Controls.Gauge folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge%PLATFORM_SUFFIX%\"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge%PLATFORM_SUFFIX%\Themes"

@rem WinRTXamlToolkit.Controls.DataVisualization folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\Charting"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\Charting\Axis"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\Charting\Chart"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\Charting\DataPoint"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\Charting\Primitives"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\Charting\Series"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\Legend"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\Themes"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\Title"

@rem WinRTXamlToolkit.Debugging folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging%PLATFORM_SUFFIX%\Controls\EditableListBox"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging%PLATFORM_SUFFIX%\Themes"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging%PLATFORM_SUFFIX%\Views\PropertyEditors"

@rem copy "..\src\SomeController.cs" content
copy "..\WinRTXamlToolkit\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\WinRTXamlToolkit.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.Calendar\WinRTXamlToolkit.Controls.Calendar%PLATFORM_SUFFIX%\bin\Release\WinRTXamlToolkit.Controls.Calendar.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\WinRTXamlToolkit.Controls.DataVisualization.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.Gauge\WinRTXamlToolkit.Controls.Gauge%PLATFORM_SUFFIX%\bin\Release\WinRTXamlToolkit.Controls.Gauge.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Debugging\WinRTXamlToolkit.Debugging%PLATFORM_SUFFIX%\bin\Release\WinRTXamlToolkit.Debugging.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR

xcopy /E /Y "..\WinRTXamlToolkit\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Calendar\WinRTXamlToolkit.Controls.Calendar%PLATFORM_SUFFIX%\bin\Release\Controls\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\Controls"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.DataVisualization\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Controls\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Controls"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Gauge\WinRTXamlToolkit.Controls.Gauge%PLATFORM_SUFFIX%\bin\Release\Controls\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge\Controls"
xcopy /E /Y "..\WinRTXamlToolkit.Debugging\WinRTXamlToolkit.Debugging%PLATFORM_SUFFIX%\bin\Release\Controls\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Controls"

@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls"
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\AlternativeFrame\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\AlternativeFrame" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\AnimatingContainer\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\AnimatingContainer" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\CameraCaptureControl\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CameraCaptureControl" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\CascadingImageControl\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CascadingImageControl" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\CascadingTextBlock\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CascadingTextBlock" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\ColorPicker\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ColorPicker" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\CountdownControl\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CountdownControl" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\CustomAppBar\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CustomAppBar" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\CustomGridSplitter\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CustomGridSplitter" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\DelayedLoadControl\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\DelayedLoadControl" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\FxContentControl\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\FxContentControl" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\ImageButton\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ImageButton" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\ImageToggleButton\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ImageToggleButton" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\InputDialog\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\InputDialog" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\ListItemButton\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ListItemButton" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\NumericUpDown\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\NumericUpDown" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\ToolWindow\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ToolWindow" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\TreeView\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\TreeView" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\WatermarkPasswordBox\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WatermarkPasswordBox" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\WatermarkTextBox\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WatermarkTextBox" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Controls\WebBrowser\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WebBrowser" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\Themes\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Themes" || GOTO :REPORT_ERROR

@rem copy "..\WinRTXamlToolkit.Controls.Calendar%PLATFORM_SUFFIX%\bin\Release\Controls\Calendar\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\Controls\Calendar" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.Calendar%PLATFORM_SUFFIX%\bin\Release\Themes\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\Themes" || GOTO :REPORT_ERROR
@rem 
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\Axis\AxisLabel.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\Axis\DateTimeAxisLabel.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\Axis\DisplayAxis.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\Axis\NumericAxisLabel.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\Axis\RangeAxis.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Axis" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\Chart\Chart.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Chart" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\DataPoint\AreaDataPoint.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\DataPoint\BarDataPoint.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\DataPoint\BubbleDataPoint.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\DataPoint\ColumnDataPoint.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\DataPoint\LineDataPoint.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\DataPoint\PieDataPoint.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\DataPoint\ScatterDataPoint.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\DataPoint" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\Primitives\DelegatingListBox.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Primitives" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\Series\AreaSeries.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\Series\DataPointSeries.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\Series\DefinitionSeries.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\Series\LegendItem.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\Series\LineSeries.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Charting\Series\PieSeries.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Charting\Series" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Legend\Legend.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Legend" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Themes\Generic.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Themes" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\Title\Title.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\Title" || GOTO :REPORT_ERROR
@rem 
@rem copy "..\WinRTXamlToolkit.Controls.Gauge%PLATFORM_SUFFIX%\bin\Release\Themes\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge\Themes" || GOTO :REPORT_ERROR
@rem 
@rem copy "..\WinRTXamlToolkit.Debugging%PLATFORM_SUFFIX%\bin\Release\Controls\EditableListBox\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Controls\EditableListBox" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Debugging%PLATFORM_SUFFIX%\bin\Release\Themes\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Themes" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Debugging%PLATFORM_SUFFIX%\bin\Release\Views\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Views" || GOTO :REPORT_ERROR
@rem copy "..\WinRTXamlToolkit.Debugging%PLATFORM_SUFFIX%\bin\Release\Views\PropertyEditors\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\Views\PropertyEditors" || GOTO :REPORT_ERROR
@rem copy "..\src\SomePowershellScript.ps1 tools || GOTO :REPORT_ERROR
@GOTO :EOF

:PACK_FILES
@echo Packing NuGets
nuget pack "WinRTXamlToolkit.Windows.nuspec"
nuget pack "WinRTXamlToolkit.Controls.DataVisualization.Windows.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Calendar.Windows.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Gauge.Windows.nuspec"
nuget pack "WinRTXamlToolkit.Debugging.Windows.nuspec"

nuget pack "WinRTXamlToolkit.WindowsPhone.nuspec"
nuget pack "WinRTXamlToolkit.Controls.DataVisualization.WindowsPhone.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Calendar.WindowsPhone.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Gauge.WindowsPhone.nuspec"
nuget pack "WinRTXamlToolkit.Debugging.WindowsPhone.nuspec"
@GOTO :EOF

:REPORT_ERROR
@ECHO Error, see the last command executed.

pause
