set PATH=%PATH%;c:\Program Files (x86)\MSBuild\14.0\Bin;C:\Windows\Microsoft.NET\Framework\v4.0.30319
set MSBUILD="c:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
set NUGET_PLATFORM=uap10.0
set PLATFORM_SUFFIX=.UWP
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
%MSBUILD% /v:%VERBOSITY% /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.UWP\WinRTXamlToolkit.UWP.csproj" || GOTO :REPORT_ERROR
@rem mv msbuild.log msbuild.%DATE:~-4%-%DATE:~4,2%-%DATE:~7,2%_%TIME:~0,2%%TIME:~3,2%_%TIME:~6,2%.%TIME:~-2%.log
%MSBUILD% /v:%VERBOSITY% /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Debugging.UWP\WinRTXamlToolkit.Debugging.UWP.csproj" || GOTO :REPORT_ERROR
@rem mv msbuild.log msbuild.%DATE:~-4%-%DATE:~4,2%-%DATE:~7,2%_%TIME:~0,2%%TIME:~3,2%_%TIME:~6,2%.%TIME:~-2%.log
%MSBUILD% /v:%VERBOSITY% /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Calendar.UWP\WinRTXamlToolkit.Controls.Calendar.UWP.csproj" || GOTO :REPORT_ERROR
@rem mv msbuild.log msbuild.%DATE:~-4%-%DATE:~4,2%-%DATE:~7,2%_%TIME:~0,2%%TIME:~3,2%_%TIME:~6,2%.%TIME:~-2%.log
%MSBUILD% /v:%VERBOSITY% /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.Gauge.UWP\WinRTXamlToolkit.Controls.Gauge.UWP.csproj" || GOTO :REPORT_ERROR
@rem mv msbuild.log msbuild.%DATE:~-4%-%DATE:~4,2%-%DATE:~7,2%_%TIME:~0,2%%TIME:~3,2%_%TIME:~6,2%.%TIME:~-2%.log
%MSBUILD% /v:%VERBOSITY% /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Controls.DataVisualization.UWP\WinRTXamlToolkit.Controls.DataVisualization.UWP.csproj" || GOTO :REPORT_ERROR
@rem mv msbuild.log msbuild.%DATE:~-4%-%DATE:~4,2%-%DATE:~7,2%_%TIME:~0,2%%TIME:~3,2%_%TIME:~6,2%.%TIME:~-2%.log
@rem %MSBUILD% /v:%VERBOSITY% /fl /t:Rebuild /p:Configuration=Release "..\WinRTXamlToolkit.Sample.UWP\WinRTXamlToolkit.Sample.UWP.csproj" || GOTO :REPORT_ERROR
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
@GOTO :EOF

:COPY_FILES
@echo Copying UWP build

copy "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\WinRTXamlToolkit.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.Calendar%PLATFORM_SUFFIX%\bin\Release\WinRTXamlToolkit.Controls.Calendar.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\WinRTXamlToolkit.Controls.DataVisualization.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Controls.Gauge%PLATFORM_SUFFIX%\bin\Release\WinRTXamlToolkit.Controls.Gauge.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR
copy "..\WinRTXamlToolkit.Debugging%PLATFORM_SUFFIX%\bin\Release\WinRTXamlToolkit.Debugging.*" "lib\%NUGET_PLATFORM%" || GOTO :REPORT_ERROR

@rem XBFs are required for all the XAML files
xcopy /E /Y "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Calendar%PLATFORM_SUFFIX%\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Gauge%PLATFORM_SUFFIX%\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge\"
xcopy /E /Y "..\WinRTXamlToolkit.Debugging%PLATFORM_SUFFIX%\bin\Release\*.xbf" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\"

@rem XAML files enable something in VS IIRC (toolbox support, designer support or templates)
xcopy /E /Y "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\*.xaml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Calendar%PLATFORM_SUFFIX%\*.xaml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\*.xaml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Gauge%PLATFORM_SUFFIX%\*.xaml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge\"
xcopy /E /Y "..\WinRTXamlToolkit.Debugging%PLATFORM_SUFFIX%\*.xaml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\"

@rem rd.xml are required to build (for ProjectN/.Net Native Compile)
xcopy /E /Y "..\WinRTXamlToolkit%PLATFORM_SUFFIX%\*.rd.xml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Calendar%PLATFORM_SUFFIX%\*.rd.xml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Calendar\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.DataVisualization%PLATFORM_SUFFIX%\*.rd.xml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.DataVisualization\"
xcopy /E /Y "..\WinRTXamlToolkit.Controls.Gauge%PLATFORM_SUFFIX%\*.rd.xml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Controls.Gauge\"
xcopy /E /Y "..\WinRTXamlToolkit.Debugging%PLATFORM_SUFFIX%\*.rd.xml" "lib\%NUGET_PLATFORM%\WinRTXamlToolkit.Debugging\"
@GOTO :EOF

:PACK_FILES
@echo Packing NuGets
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