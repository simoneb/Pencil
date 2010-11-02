@echo off
color
@Build\Pencil.Build.exe Pencil_new.cs %*
goto %ERRORLEVEL%
rem Fail!, paint it red.
:1
	color 4F
	goto done
rem Success, paint it green.
:0
	color 2F
:done
