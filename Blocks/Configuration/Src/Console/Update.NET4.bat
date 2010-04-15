@rem %1 executable name
@rem %2 CorFlags.exe path
@rem %3 optional flag: clean

SET CORFLAGS=%2CorFlags.exe

@rem check clean

if "%3"=="/clean" goto CLEAN

@rem copy files

copy "%1.exe" "%1.NET4.exe"
if errorlevel 1 goto ERROR

copy "%1.pdb" "%1.NET4.pdb"
if errorlevel 1 goto ERROR

copy "%~dp0\App.NET4.config" "%1.NET4.exe.config"
if errorlevel 1 goto ERROR

@rem Create 32-bit targeted versions for .Net 3.5
copy "%1.exe" "%1-32.exe"
if errorlevel 1 goto ERROR

%CORFLAGS% "%1-32.exe" /32BIT+ /nologo /force
if errorlevel 1 goto ERROR

copy "%~dp0\App.config" "%1-32.exe.config"
if errorlevel 1 goto ERROR

copy "%1.pdb" "%1-32.pdb"
if errorlevel 1 goto ERROR

@rem Create 32-bit targeted versions for .Net 4
copy "%1.exe" "%1.NET4-32.exe"
if errorlevel 1 goto ERROR

%CORFLAGS% "%1.NET4-32.exe" /32BIT+ /nologo /force
REM if errorlevel 1 goto ERROR

copy "%1.pdb" "%1.NET4-32.pdb"
if errorlevel 1 goto ERROR

copy "%~dp0\App.NET4.config" "%1.NET4-32.exe.config"
if errorlevel 1 goto ERROR

goto END

:CLEAN

@rem delete files

@rem delete 32-bit versions of .Net 3.5
if exist "%1-32.exe" del "%1-32.exe"
if errorlevel 1 goto ERROR

if exist "%1-32.pdb" del "%1-32.pdb"
if errorlevel 1 goto ERROR

if exist "%1-32.exe.config" del "%1-32.exe.config"
if errorlevel 1 goto ERROR

@rem delete .Net 4 versions
if exist "%1.NET4.exe" del "%1.NET4.exe"
if errorlevel 1 goto ERROR

if exist "%1.NET4.pdb" del "%1.NET4.pdb"
if errorlevel 1 goto ERROR

if exist "%1.NET4.exe.config" del "%1.NET4.exe.config"
if errorlevel 1 goto ERROR

@rem delete .Net4 32-bit
if exist "%1.NET4-32.exe" del "%1.NET4-32.exe"
if errorlevel 1 goto ERROR

if exist "%1.NET4-32.pdb" del "%1.NET4-32.pdb"
if errorlevel 1 goto ERROR

if exist "%1.NET4-32.exe.config" del "%1.NET4-32.exe.config"
if errorlevel 1 goto ERROR

goto :END

:ERROR

@exit errorLevel

:END