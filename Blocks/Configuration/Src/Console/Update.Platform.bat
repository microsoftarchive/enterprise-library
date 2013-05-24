@rem %1 executable name
@rem %2 CorFlags.exe path
@rem %3 optional flag: clean

SET CORFLAGS=%2CorFlags.exe

@rem check clean

if "%3"=="/clean" goto CLEAN

if not exist "%CORFLAGS%" goto TOOLNOTPRESENT

@rem copy files

@rem Create 32-bit targeted versions
copy "%1.exe" "%1-32.exe"
if errorlevel 1 goto ERROR

%CORFLAGS% "%1-32.exe" /32BIT+ /nologo /force
if errorlevel 1 goto ERROR

copy "%~dp0\App.config" "%1-32.exe.config"
if errorlevel 1 goto ERROR

copy "%1.pdb" "%1-32.pdb"
if errorlevel 1 goto ERROR

goto END

:CLEAN

@rem delete files

@rem delete 32-bit versions
if exist "%1-32.exe" del "%1-32.exe"
if errorlevel 1 goto ERROR

if exist "%1-32.pdb" del "%1-32.pdb"
if errorlevel 1 goto ERROR

if exist "%1-32.exe.config" del "%1-32.exe.config"
if errorlevel 1 goto ERROR

goto :END

:ERROR

@exit errorLevel

:TOOLNOTPRESENT
@echo CorFlags tool is not available at %CORFLAGS%
@echo If you want to generate a 32-bit only version of the tool, download the SDK and place the CorFlags.exe file in the specified location.

:END