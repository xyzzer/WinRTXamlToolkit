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
@rem %MSBUILD% /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Debugging\WinRTXamlToolkit.Debugging.WinRTProxy.Windows\WinRTXamlToolkit.Debugging.WinRTProxy.Windows.csproj" || GOTO :REPORT_ERROR

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
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\FocusVisualizer"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\FxContentControl"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\ImageButton"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\ImageToggleButton"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\InputDialog"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\ListItemButton"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\NumericUpDown"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit%PLATFORM_SUFFIX%\Controls\ToolStrip"
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
@rem if [%PLATFORM_SUFFIX%]==[.Windows] (copy "..\WinRTXamlToolkit.Debugging\WinRTXamlToolkit.Debugging.WinRTProxy%PLATFORM_SUFFIX%\bin\Release\WinRTXamlToolkit.Debugging.WinRTProxy.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR)


xcopy /E /Y "..\WinRTXamlToolkit\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Calendar\WinRTXamlToolkit.Controls.Calendar%PLATFORM_SUFFIX%\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.DataVisualization\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Gauge\WinRTXamlToolkit.Controls.Gauge%PLATFORM_SUFFIX%\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge\"
xcopy /E /Y "..\WinRTXamlToolkit.Debugging\WinRTXamlToolkit.Debugging%PLATFORM_SUFFIX%\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\"
@rem if [%PLATFORM_SUFFIX%]==[.Windows] (xcopy /E /Y "..\WinRTXamlToolkit.Debugging\WinRTXamlToolkit.Debugging.WinRTProxy%PLATFORM_SUFFIX%\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging.WinRTProxy\")

@rem copy "..\src\SomePowershellScript.ps1 tools || GOTO :REPORT_ERROR
@GOTO :EOF

:PACK_FILES
@echo Packing NuGets
nuget pack "WinRTXamlToolkit.Windows.nuspec"
nuget pack "WinRTXamlToolkit.Controls.DataVisualization.Windows.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Calendar.Windows.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Gauge.Windows.nuspec"
nuget pack "WinRTXamlToolkit.Debugging.Windows.nuspec"
@rem nuget pack "WinRTXamlToolkit.Debugging.WinRTProxy.Windows.nuspec"

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
