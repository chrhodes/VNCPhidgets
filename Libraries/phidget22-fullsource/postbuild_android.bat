SET ProjectDir=%1..
SET OutDir=%2
SET Configuration=%3

@call "%ProjectDir%\make linux phidget22.h.bat" %Configuration%
IF %ERRORLEVEL% EQU 0 goto :eof 

@copy "%ProjectDir%\phidget22.h" "%OutDir%">NUL
	