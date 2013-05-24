@echo off
@REM  ---------------------------------------------------------------------------------
@REM  RegAssemblies.bat file
@REM
@REM  This batch file installs/uninstalls all the assemblies for Enterprise Library 
@REM  application blocks.  By default it installs all the assemblies in the debug dirs.
@REM  
@REM  Optional arguments for this batch file:
@REM    1 - /u to unstall. Otherwise it is installed.
@REM    2 - /log to log to console
@REM    
@REM  ----------------------------------------------------------------------------------

echo.
echo ==========================================================================
echo   RegAssemblies.bat                                                    
echo      Installs/uninstalls assemblies for Enterprise Library  
echo ==========================================================================
echo.

set InstallUtilDir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\
set binDir=%~dp0
set action=
set logToConsole=false
set pause=true
set buildType=Debug

if "%1"=="/?" goto HELP

if "%1"=="" goto RUN

@REM  ----------------------------------------------------
@REM  If the first parameter is /q, do not pause
@REM  at the end of execution.
@REM  ----------------------------------------------------
if /i "%1"=="/q" (
 set pause=false
 SHIFT
)

@REM  ----------------------------------------------------
@REM  If the first parameter is /u, uninstall.
@REM  ----------------------------------------------------
if /i "%1"=="/u" (
 set action=%1
 SHIFT
)

@REM  ----------------------------------------------------
@REM  If the first parameter is /log, log to console.
@REM  ----------------------------------------------------
if /i "%1"=="/log" (
 set logToConsole=true
 SHIFT
)

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
if not Exist "%binDir%" goto HELP
pushd %binDir%

@REM  ----------------------------------------
@REM  Register VS.NET environment variables
@REM  (required to call installutil)
@REM  ----------------------------------------
if not Exist "%installUtilDir%" goto HELPFW
@set PATH=%PATH%;%installUtilDir%


@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Common Infrastructure
@ECHO -----------------------------------------------------------------
@ECHO.
pushd %binDir%
cd ..\Blocks\Common\Tests\Common\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Common.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Common.Tests.dll
@if errorlevel 1 goto :error
popd

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Logging Application Block
@ECHO -----------------------------------------------------------------
@ECHO.

@REM Logging
@REM ------------------------------------------------------------------
pushd %binDir%
cd ..\Blocks\Logging\Tests\Logging\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Logging.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Logging.Tests.dll
@if errorlevel 1 goto :error
popd

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Logging MSMQ Distributor Service
@ECHO -----------------------------------------------------------------
@ECHO.
pushd %binDir%
cd ..\Blocks\Logging\Tests\MsmqDistributor\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests.dll
@if errorlevel 1 goto :error
popd

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Policy Injection Application Block
@ECHO -----------------------------------------------------------------
@ECHO.

@REM Policy Injection
@REM ------------------------------------------------------------------
pushd %binDir%

@REM Policy Injection Call Handlers
@REM ------------------------------------------------------------------
cd ..\Blocks\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.PolicyInjection.dll installutil %action% /LogToConsole=%logToConsole% /Category="Call Handler Unit Tests" Microsoft.Practices.EnterpriseLibrary.PolicyInjection.dll 
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\PolicyInjection\Tests\PolicyInjection\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.dll
@if errorlevel 1 goto :error
popd

@ECHO.
@ECHO ----------------------------------------
@ECHO RegAssemblies.bat Completed
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
@ECHO An error occured in RegAssemblies.bat - %errorLevel%
if %pause%==true PAUSE
goto exit

:HELPFW
echo Error: Unable to locate the .NET Framework
echo.
echo InstallServices.bat assumes the .NET Framework has been installed in its default location 
echo ("%visualStudioDir%".) 
echo.
echo If you have installed the .NET Framework v2.0 to a different location, you will need 
echo to update this batch file to reflect that location.
echo.
goto exit

:HELP
echo Usage: RegAssemblies.bat [/q] [/u] [build folder]  
echo.
echo Examples:
echo.
echo	"RegAssemblies" - installs assemblies for Enterprise Library assemblies       
echo	"RegAssemblies /u" - uninstalls assemblies for Enterprise Library assemblies 
echo	"RegAssemblies /q" - installs assemblies, no pause when error occurs (quiet mode)     
echo	"RegAssemblies /log" - logs output to the console 
echo.

@REM  ----------------------------------------
@REM  The exit label
@REM  ----------------------------------------
:exit

popd
set pause=
set binDir=
set prompt=%savedPrompt%
set savedPrompt=
set action=

echo on