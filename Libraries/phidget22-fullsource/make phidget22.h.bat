@set OLDPATH=%PATH%
@set OLDINCLUDE=%INCLUDE%
@set OLDLIB=%LIB%
@set CURRENTPATH=%~dp0

@echo OFF

IF "%~1"=="" (
@set BUILDTYPE=Release
) ELSE (
@set BUILDTYPE=%1
)

IF NOT %BUILDTYPE%==Release (
@set EXTRAARGS="/DDEBUG"
)

echo.
echo Making phidget22.h...
echo.

IF EXIST "c:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\" (
echo Setting environment for using Microsoft Visual Studio 2015 tools.
call "c:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\vsvars32.bat"
) ELSE ( 
IF EXIST "c:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools\" (
echo Setting environment for using Microsoft Visual Studio 2013 tools.
call "c:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools\vsvars32.bat"
) ELSE ( 
IF EXIST "c:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\" (
echo Setting environment for using Microsoft Visual Studio 2012 tools.
call "c:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
) ELSE ( 
IF EXIST "c:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\Tools\" (
call "c:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\Tools\vsvars32.bat"
) ELSE ( 
IF EXIST "c:\Program Files (x86)\Microsoft Visual Studio 9.0\Common7\Tools\" (
call "c:\Program Files (x86)\Microsoft Visual Studio 9.0\Common7\Tools\vsvars32.bat"
) ELSE ( 
IF EXIST "c:\Program Files (x86)\Microsoft Visual Studio 8.0\Common7\Tools\" (
call "c:\Program Files (x86)\Microsoft Visual Studio 8.0\Common7\Tools\vsvars32.bat"
) ELSE ( 
IF EXIST "c:\Program Files\Microsoft Visual Studio 10.0\Common7\Tools\" (
call "c:\Program Files\Microsoft Visual Studio 10.0\Common7\Tools\vsvars32.bat"
) ELSE ( 
IF EXIST "c:\Program Files\Microsoft Visual Studio 9.0\Common7\Tools\" (
call "c:\Program Files\Microsoft Visual Studio 9.0\Common7\Tools\vsvars32.bat"
) ELSE ( 
IF EXIST "c:\Program Files\Microsoft Visual Studio 8.0\Common7\Tools\" (
call "c:\Program Files\Microsoft Visual Studio 8.0\Common7\Tools\vsvars32.bat"
) ELSE ( 
echo Couldn't locate a Visual Studio install
exit /b 1
)
)
)
)
)
)
)
)
)

cl /EP /C /D_WINDOWS %EXTRAARGS% /DEXTERNALPROTO /I%CURRENTPATH%src\ext\ "%CURRENTPATH%src\phidget22int.h" > "%CURRENTPATH%phidget221.h"
"%CURRENTPATH%tools\indent\indent" "%CURRENTPATH%phidget221.h" -nbc -l1000 -sob

echo #ifndef PHIDGET22_H > "%CURRENTPATH%phidget22.h"
echo #define PHIDGET22_H >> "%CURRENTPATH%phidget22.h"
type "%CURRENTPATH%cppheader" >> "%CURRENTPATH%phidget22.h"
type "%CURRENTPATH%src\constants.h" >> "%CURRENTPATH%phidget22.h"
type "%CURRENTPATH%src\constants.gen.h" >> "%CURRENTPATH%phidget22.h"
type "%CURRENTPATH%phidget221.h" >> "%CURRENTPATH%phidget22.h"
type "%CURRENTPATH%cppfooter" >> "%CURRENTPATH%phidget22.h"
echo #endif >> "%CURRENTPATH%phidget22.h"

del "%CURRENTPATH%phidget221.*"

@set PATH=%OLDPATH%
@set INCLUDE=%OLDINCLUDE%
@set LIB=%OLDLIB%
