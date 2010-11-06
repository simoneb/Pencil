@echo off
cls
pushd %~dp0

@Tools\Pencil.Build.exe -r:System.dll Pencil_new.cs %*

if NOT ERRORLEVEL = 0 goto error

rem Success, paint it green.
	color 2F
	goto done
:error
rem Fail!, paint it red.
	color 4F
	
:done

	popd
	@pause
	color