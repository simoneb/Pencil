@echo off
cls

@%~dp0Tools\Pencil.exe %~dp0Pencil.cs %*

if not %ERRORLEVEL% ==0 goto error

rem Success, paint it green.
  color 2F
	goto done

:error
rem Fail!, paint it red.
	color 4F
	
:done
	pause
	color
