@echo off
pushd %~dp0
color

@Tools\Pencil.Build.exe Pencil_new.cs %*

if ERRORLEVEL 1 goto error

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