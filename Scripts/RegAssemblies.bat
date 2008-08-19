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

set InstallUtilDir=%WINDIR%\Microsoft.NET\Framework\v3.5\
set binDir="."
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
@ECHO Common Application Block
@ECHO -----------------------------------------------------------------
@ECHO.
cd ..\Blocks\Common\Src\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Common.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Common.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Common\Tests\Common\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Common.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Common.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Common\Tests\Configuration.Manageability\bin\%buildType%
if Exist EnterpriseLibrary.Common.Configuration.Manageability.Tests.dll installutil %action% /LogToConsole=%logToConsole% EnterpriseLibrary.Common.Configuration.Manageability.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Caching Application Block
@ECHO -----------------------------------------------------------------
@ECHO.

@REM Caching
@REM ------------------------------------------------------------------
cd ..\Blocks\Caching\Src\Caching\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Caching.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Caching.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Caching\Tests\Caching.Tests\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Caching.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Caching.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Caching\Tests\Configuration.Manageability.Tests\bin\%buildType%
if Exist EnterpriseLibrary.Caching.Manageability.Tests.dll installutil %action% /LogToConsole=%logToConsole% EnterpriseLibrary.Caching.Manageability.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

@REM Caching Cryptography
@REM ------------------------------------------------------------------
cd ..\Blocks\Caching\Src\Cryptography\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Caching\Tests\Cryptography.Configuration.Manageability.Tests\bin\%buildType%
if Exist EnterpriseLibrary.Caching.Cryptography.Configuration.Manageability.Tests.dll installutil %action% /LogToConsole=%logToConsole% EnterpriseLibrary.Caching.Cryptography.Configuration.Manageability.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

@REM Caching Database
@REM ------------------------------------------------------------------
cd ..\Blocks\Caching\Src\Database\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Caching.Database.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Caching.Database.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Caching\Tests\Database.Configuration.Manageability.Tests\bin\%buildType%
if Exist EnterpriseLibrary.Caching.Database.Configuration.Manageability.Tests.dll installutil %action% /LogToConsole=%logToConsole% EnterpriseLibrary.Caching.Database.Configuration.Manageability.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%


@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Crypto Application Block
@ECHO -----------------------------------------------------------------
@ECHO.
cd ..\Blocks\Security.Cryptography\Src\Cryptography\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Security.Cryptography\Tests\Cryptography.Tests\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Data Application Block
@ECHO -----------------------------------------------------------------
@ECHO.
cd ..\Blocks\Data\Src\Data\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Data.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Data.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Data\Tests\Data.Tests\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Data.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Data.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Exception Handling Application Block
@ECHO -----------------------------------------------------------------
@ECHO.

@REM Exception Handling
@REM ------------------------------------------------------------------
cd ..\blocks\ExceptionHandling\Src\ExceptionHandling\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\ExceptionHandling\Tests\ExceptionHandling\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

@REM Exception Handling Logging
@REM ------------------------------------------------------------------
cd ..\Blocks\ExceptionHandling\Src\Logging\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\ExceptionHandling\Tests\Logging.Configuration.Manageability\bin\%buildType%
if Exist EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Manageability.Tests.dll installutil %action% /LogToConsole=%logToConsole% EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Manageability.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

@REM Exception Handling WCF
@REM ------------------------------------------------------------------
cd ..\Blocks\ExceptionHandling\Src\WCF\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\ExceptionHandling\Tests\WCF.Configuration.Manageability\bin\%buildType%
if Exist EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Manageability.Tests.dll installutil %action% /LogToConsole=%logToConsole% EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Manageability.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%


@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Logging Application Block
@ECHO -----------------------------------------------------------------
@ECHO.

@REM Logging
@REM ------------------------------------------------------------------
cd ..\Blocks\Logging\Src\Logging\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Logging.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Logging.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Logging\Tests\Logging\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Logging.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Logging.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

@REM Logging Database
@REM ------------------------------------------------------------------
cd ..\Blocks\Logging\Src\DatabaseTraceListener\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Logging.Database.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Logging.Database.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Logging\Tests\Database.Configuration.Manageability\bin\%buildType%
if Exist EnterpriseLibrary.Logging.Database.Configuration.Manageability.Tests.dll installutil %action% /LogToConsole=%logToConsole% EnterpriseLibrary.Logging.Database.Configuration.Manageability.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Logging MSMQ Distributor Service
@ECHO -----------------------------------------------------------------
@ECHO.
cd ..\Blocks\Logging\Src\MsmqDistributor\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Logging\Tests\MsmqDistributor\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Policy Injection Application Block
@ECHO -----------------------------------------------------------------
@ECHO.

@REM Policy Injection
@REM ------------------------------------------------------------------
cd ..\Blocks\PolicyInjection\Src\PolicyInjection\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.PolicyInjection.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.PolicyInjection.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

@REM Policy Injection Call Handlers
@REM ------------------------------------------------------------------
cd ..\Blocks\PolicyInjection\Src\CallHandlers\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\PolicyInjection\Tests\CallHandlers\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Security Application Block
@ECHO -----------------------------------------------------------------
@ECHO.

@REM Security
@REM ------------------------------------------------------------------
cd ..\Blocks\Security\Src\Security\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Security.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Security.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Security\Tests\Security\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Security.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Security.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

@REM Security Cache
@REM ------------------------------------------------------------------
cd ..\Blocks\Security\Src\CachingStore\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Security\Tests\CachingStore\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Security\Tests\CachingStore.Configuration.Manageability\bin\%buildType%
if Exist EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Manageability.Tests.dll installutil %action% /LogToConsole=%logToConsole% EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Manageability.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Validation Application Block
@ECHO -----------------------------------------------------------------
@ECHO.

@REM Validation
@REM ------------------------------------------------------------------
cd ..\Blocks\Validation\Src\Validation\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Validation.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Validation.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%
cd ..\Blocks\Validation\Tests\Validation.Tests\bin\%buildType%
if Exist Microsoft.Practices.EnterpriseLibrary.Validation.Tests.dll installutil %action% /LogToConsole=%logToConsole% Microsoft.Practices.EnterpriseLibrary.Validation.Tests.dll
@if errorlevel 1 goto :error
popd
pushd %binDir%

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