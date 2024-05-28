SET ProjectDir=%1..
SET OutDir=%2
SET Configuration=%3

@call "%ProjectDir%\make phidget22.h.bat" %Configuration%
@fc "%ProjectDir%\phidget22.h" "%OutDir%phidget22.h">NUL 2>&1
IF %ERRORLEVEL% EQU 0 goto :eof 

@copy "%ProjectDir%\phidget22.h" "%OutDir%">NUL

IF NOT EXIST %OutDir%mos mkdir %OutDir%mos
IF NOT EXIST %OutDir%mos\kv mkdir %OutDir%mos\kv

@copy "%ProjectDir%\src\ext\mos\*.h" "%OutDir%mos">NUL
@copy "%ProjectDir%\src\ext\mos\kv\kv.h" "%OutDir%mos\kv">NUL
	