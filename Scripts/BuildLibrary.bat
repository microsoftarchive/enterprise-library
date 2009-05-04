@echo off
@REM  ----------------------------------------------------------------------------
@REM  BuildLibrary.bat file
@REM
@REM  This batch file builds the Enterprise Library application blocks and tools.
@REM  By default, it builds a Debug build.
@REM  
@REM  Optional arguments for this batch file:
@REM    1 - Build type. Defaults to Debug
@REM  ----------------------------------------------------------------------------

echo.
echo =========================================================
echo   BuildLibrary                                           
echo      Builds Enterprise Library                           
echo =========================================================
echo.

set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v3.5
set solutionDir="..\Blocks\"
set buildType=Debug
set returnErrorCode=true
set pause=true

if "%1"=="/?" goto HELP

if not Exist %solutionDir%\EnterpriseLibrary.sln goto HELP

@REM  ----------------------------------------------------
@REM  If the first parameter is /q, do not pause
@REM  at the end of execution.
@REM  ----------------------------------------------------

if /i "%1"=="/q" (
 set pause=false
 SHIFT
)

@REM  ----------------------------------------------------
@REM  If the first or second parameter is /i, do not 
@REM  return an error code on failure.
@REM  ----------------------------------------------------

if /i "%1"=="/i" (
 set returnErrorCode=false
 SHIFT
)

@REM  ----------------------------------------------------
@REM  User can override default build type by specifiying
@REM  a parameter to batch file (e.g. BuildLibrary Debug).
@REM  ----------------------------------------------------

if not "%1"=="" set buildType=%1

@REM  ------------------------------------------------
@REM  Shorten the command prompt for making the output
@REM  easier to read.
@REM  ------------------------------------------------
set savedPrompt=%prompt%
set prompt=*$g

@ECHO ----------------------------------------
@ECHO BuildLibrary.bat Started
@ECHO ----------------------------------------
@ECHO.

@REM -------------------------------------------------------
@REM Change to the directory where the solution file resides
@REM -------------------------------------------------------

pushd %solutionDir%

@REM -------------------------------------------------------
@REM Change to the directory where the solution file resides
@REM -------------------------------------------------------

if "%DevEnvDir%"=="" (
	@ECHO ------------------------------------------
	@ECHO Setting build environment
	@ECHO ------------------------------------------
	@CALL "%VS90COMNTOOLS%\vsvars32.bat" > NUL 
	@REM Remove LIB env var to work around known VS 2008 bug
	@SET Lib=
)

@ECHO.
@ECHO -------------------------------------------
@ECHO Building the Enterprise Library assemblies
@ECHO -------------------------------------------

call %msBuildDir%\msbuild EnterpriseLibrary.sln /t:Rebuild /p:Configuration=%buildType%
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO BuildLibrary.bat Completed
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
if %returnErrorCode%==false goto exit

@ECHO An error occured in BuildLibrary.bat - %errorLevel%
if %pause%==true PAUSE
@exit errorLevel

:HELP
echo Usage: BuildLibrary [/q] [/i] [build type] 
echo.
echo BuildLibrary is to be executed in the directory where EnterpriseLibrary.sln resides
echo The default build type is Debug.
echo.
echo Examples:
echo.
echo    "BuildLibrary" - builds a Debug build      
echo    "BuildLibrary Release" - builds a Release build
echo.

@REM  ----------------------------------------
@REM  The exit label
@REM  ----------------------------------------
:exit
if %pause%==true PAUSE

popd
set pause=
set solutionDir=
set buildType=
set returnErrorCode=
set prompt=%savedPrompt%
set savedPrompt=

echo on