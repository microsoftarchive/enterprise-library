@echo off
@REM  ---------------------------------------------------------------------------------
@REM  InstallDbs.bat file
@REM
@REM  This batch file installs the databases needed to run the unit tests.
@REM    
@REM  ----------------------------------------------------------------------------------

echo.
echo ==========================================================================
echo   InstallDbs.bat                                                    
echo      Installs Databases for Enterprise Library  
echo ==========================================================================
echo.

set binDir="."

:RUN
@REM  ------------------------------------------------
@REM  Shorten the command prompt for making the output
@REM  easier to read.
@REM  ------------------------------------------------
set savedPrompt=%prompt%
set prompt=*$g

@REM -------------------------------------------------------
@REM Set the current directory
@REM -------------------------------------------------------
pushd %binDir%

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Caching Database
@ECHO -----------------------------------------------------------------
@ECHO.
cd ..\Blocks\Caching\Src\Database\Scripts
if Exist CreateCachingDb.cmd Call CreateCachingDb.cmd
@if errorlevel 1 goto :error
popd
pushd %binDir%

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Logging
@ECHO -----------------------------------------------------------------
@ECHO.
cd ..\Blocks\Logging\Src\DatabaseTraceListener\Scripts
if Exist CreateLoggingDb.cmd Call CreateLoggingDb.cmd
@if errorlevel 1 goto :error
popd
pushd %binDir%

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Northwind
@ECHO -----------------------------------------------------------------
@ECHO.
if Exist CreateNorthwindDb.cmd Call CreateNorthwindDb.cmd
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO InstallDbs.bat Completed
@ECHO ----------------------------------------
@ECHO.

@REM  ----------------------------------------
@REM  Restore the command prompt and exit
@REM  ----------------------------------------
@goto :exit

@REM  -------------------------------------------
@REM  Handle errors
@REM
@REM  Use the following after any call to exit
@REM  and return an error code when errors occur
@REM
@REM  if errorlevel 1 goto :error	
@REM  -------------------------------------------
:error
@ECHO An error occured in InstallDbs.bat - %errorLevel%
goto exit


@REM  ----------------------------------------
@REM  The exit label
@REM  ----------------------------------------
:exit

popd
set binDir=
set prompt=%savedPrompt%
set savedPrompt=

echo on
