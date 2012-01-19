@echo off
cls

@%~dp0Source\Pencil\bin\Debug\Pencil.exe -r=Tools\OpenFileSystem\OpenFileSystem.dll Pencil.cs merge

if not %ERRORLEVEL% ==0 goto error

copy /y %~dp0merged\Pencil.exe %~dp0Tools

rem Success, paint it green.
	color 2F
	goto done

:error
rem Fail!, paint it red.
	color 4F
	
:done

pause
color