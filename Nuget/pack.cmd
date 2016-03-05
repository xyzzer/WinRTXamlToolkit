set PATH=%PATH%;c:\Program Files (x86)\MSBuild\14.0\Bin;C:\Windows\Microsoft.NET\Framework\v4.0.30319
set MSBUILD="c:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
set NUGET_PLATFORM=uap10.0
set VisualStudioVersion=14.0
set VERBOSITY=minimal

if "%1"=="nobuild" (@GOTO CREATE_FOLDER_STRUCTURE_CALL)

@CALL :BUILD || GOTO :REPORT_ERROR
if "%1"=="onlybuild" ( & @GOTO :EOF)
:CREATE_FOLDER_STRUCTURE_CALL
@CALL :CREATE_FOLDER_STRUCTURE || GOTO :REPORT_ERROR
@CALL :COPY_FILES || GOTO :REPORT_ERROR
@GOTO :PACK_FILES

:BUILD
@echo Building UWP projects
%MSBUILD% /v:%VERBOSITY% /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit\WinRTXamlToolkit.csproj" || GOTO :REPORT_ERROR
@rem mv msbuild.log msbuild.%DATE:~-4%-%DATE:~4,2%-%DATE:~7,2%_%TIME:~0,2%%TIME:~3,2%_%TIME:~6,2%.%TIME:~-2%.log
%MSBUILD% /v:%VERBOSITY% /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Debugging\WinRTXamlToolkit.Debugging.csproj" || GOTO :REPORT_ERROR
@rem mv msbuild.log msbuild.%DATE:~-4%-%DATE:~4,2%-%DATE:~7,2%_%TIME:~0,2%%TIME:~3,2%_%TIME:~6,2%.%TIME:~-2%.log
%MSBUILD% /v:%VERBOSITY% /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Calendar\WinRTXamlToolkit.Controls.Calendar.csproj" || GOTO :REPORT_ERROR
@rem mv msbuild.log msbuild.%DATE:~-4%-%DATE:~4,2%-%DATE:~7,2%_%TIME:~0,2%%TIME:~3,2%_%TIME:~6,2%.%TIME:~-2%.log
%MSBUILD% /v:%VERBOSITY% /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Gauge\WinRTXamlToolkit.Controls.Gauge.csproj" || GOTO :REPORT_ERROR
@rem mv msbuild.log msbuild.%DATE:~-4%-%DATE:~4,2%-%DATE:~7,2%_%TIME:~0,2%%TIME:~3,2%_%TIME:~6,2%.%TIME:~-2%.log
%MSBUILD% /v:%VERBOSITY% /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.DataVisualization\WinRTXamlToolkit.Controls.DataVisualization.csproj" || GOTO :REPORT_ERROR
@rem mv msbuild.log msbuild.%DATE:~-4%-%DATE:~4,2%-%DATE:~7,2%_%TIME:~0,2%%TIME:~3,2%_%TIME:~6,2%.%TIME:~-2%.log
@rem %MSBUILD% /v:%VERBOSITY% /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Sample\WinRTXamlToolkit.Sample.csproj" || GOTO :REPORT_ERROR
@GOTO :EOF

:CREATE_FOLDER_STRUCTURE
@rem Base folder structure
rd /Q /S lib
rd /Q /S tools
rd /Q /S content

@rem Creating base NuGet folders
mkdir lib
mkdir tools
mkdir content
mkdir content\controllers

@rem WinRTXamlToolkit folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\AlternativeFrame"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\AnimatingContainer"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CameraCaptureControl"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CascadingImageControl"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CascadingTextBlock"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ColorPicker"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CountdownControl"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CustomAppBar"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\CustomGridSplitter"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\DelayedLoadControl"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\FocusVisualizer"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\FxContentControl"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ImageButton"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ImageToggleButton"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\InputDialog"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ListItemButton"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\NumericUpDown"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ToolStrip"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\ToolWindow"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\TreeView"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WatermarkPasswordBox"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WatermarkTextBox"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Controls\WebBrowser"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\Themes"

@rem WinRTXamlToolkit.Controls.Calendar folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\Controls"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\Controls\Calendar"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\Themes"

@rem WinRTXamlToolkit.Controls.Gauge folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge\"
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge\Themes"

@rem WinRTXamlToolkit.Controls.DataVisualization folders
mkdir "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\"
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
@GOTO :EOF

:COPY_FILES
@echo Copying UWP build

copy "..\WinRTXamlToolkit\bin\Release\WinRTXamlToolkit.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.Calendar\bin\Release\WinRTXamlToolkit.Controls.Calendar.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\WinRTXamlToolkit.Controls.DataVisualization.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.Gauge\bin\Release\WinRTXamlToolkit.Controls.Gauge.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Debugging\bin\Release\WinRTXamlToolkit.Debugging.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR

@rem XBFs are required for all the XAML files
xcopy /E /Y "..\WinRTXamlToolkit\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Calendar\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.DataVisualization\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Gauge\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge\"
xcopy /E /Y "..\WinRTXamlToolkit.Debugging\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\"

@rem XAML files enable something in VS IIRC (toolbox support, designer support or templates)
xcopy /E /Y "..\WinRTXamlToolkit\*.xaml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Calendar\*.xaml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.DataVisualization\*.xaml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Gauge\*.xaml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge\"
xcopy /E /Y "..\WinRTXamlToolkit.Debugging\*.xaml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\"

@rem rd.xml are required to build (for ProjectN/.Net Native Compile)
xcopy /E /Y "..\WinRTXamlToolkit\*.rd.xml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Calendar\*.rd.xml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.DataVisualization\*.rd.xml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Gauge\*.rd.xml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge\"
xcopy /E /Y "..\WinRTXamlToolkit.Debugging\*.rd.xml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\"
@GOTO :EOF

:PACK_FILES
@echo Packing NuGets
nuget pack "WinRTXamlToolkit.nuspec"
nuget pack "WinRTXamlToolkit.Controls.DataVisualization.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Calendar.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Gauge.nuspec"
nuget pack "WinRTXamlToolkit.Debugging.nuspec"

nuget pack "WinRTXamlToolkit.UWP.nuspec"
nuget pack "WinRTXamlToolkit.Controls.DataVisualization.UWP.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Calendar.UWP.nuspec"
nuget pack "WinRTXamlToolkit.Controls.Gauge.UWP.nuspec"
nuget pack "WinRTXamlToolkit.Debugging.UWP.nuspec"
@GOTO :EOF

:REPORT_ERROR
@ECHO Error, see the last command executed.

pause