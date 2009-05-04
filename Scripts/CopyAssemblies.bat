@echo off
@REM  ----------------------------------------------------------------------------
@REM  CopyAssemblies.bat file
@REM
@REM  This batch file copies the Enterprise Library assemblies from their build
@REM  location to a common destination folder.
@REM  
@REM  Optional arguments for this batch file:
@REM   1 - The build output folder (Release, Debug, etc. Defaults to Debug)
@REM   2 - The destination folder (i.e. where the build will be dropped.
@REM       Defaults to ..\bin)
@REM  
@REM  ----------------------------------------------------------------------------

echo.
echo =========================================================
echo   CopyAssemblies                                         
echo      Copies EnterpriseLibrary assemblies to a single    
echo      destination directory                              
echo =========================================================
echo.

set solutionDir="..\Blocks"
set buildType=Debug
set binDir="..\bin"
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

@REM  ------------------------------------------------------
@REM  User can override default build type by specifiying
@REM  a parameter to batch file (e.g. CopyAssemblies Release).
@REM  ------------------------------------------------------

if not "%1"=="" set buildType=%1

@REM  ---------------------------------------------------------------
@REM  User can override default destination directory by specifiying
@REM  a parameter to batch file (e.g. CopyAssemblies Debug c:\bin).
@REM  ---------------------------------------------------------------

if not "%2"=="" set binDir=%2

@REM  ----------------------------------------
@REM  Shorten the command prompt for output
@REM  ----------------------------------------
set savedPrompt=%prompt%
set prompt=*$g


@ECHO ----------------------------------------
@ECHO CopyAssemblies.bat Started
@ECHO ----------------------------------------
@ECHO.

@REM -------------------------------------------------------
@REM Change to the directory where the solution file resides
@REM -------------------------------------------------------

pushd %solutionDir%

@ECHO.
@ECHO ----------------------------------------
@ECHO Create destination folder 
@ECHO ----------------------------------------
@ECHO.

if not Exist %binDir% mkdir %binDir%

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy referenced binary files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist ..\Lib\Microsoft.Practices.ObjectBuilder2.dll copy /V ..\Lib\Microsoft.Practices.ObjectBuilder2.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist ..\Lib\Microsoft.Practices.ObjectBuilder2.xml copy /V ..\Lib\Microsoft.Practices.ObjectBuilder2.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist ..\Lib\Microsoft.Practices.Unity.dll copy /V ..\Lib\Microsoft.Practices.Unity.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist ..\Lib\Microsoft.Practices.Unity.xml copy /V ..\Lib\Microsoft.Practices.Unity.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist ..\Lib\Microsoft.Practices.Unity.Configuration.dll copy /V ..\Lib\Microsoft.Practices.Unity.Configuration.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist ..\Lib\Microsoft.Practices.Unity.Configuration.xml copy /V ..\Lib\Microsoft.Practices.Unity.Configuration.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist ..\Lib\Microsoft.Practices.Unity.Interception.dll copy /V ..\Lib\Microsoft.Practices.Unity.Interception.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist ..\Lib\Microsoft.Practices.Unity.Interception.xml copy /V ..\Lib\Microsoft.Practices.Unity.Interception.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist ..\Lib\Microsoft.Practices.Unity.Interception.Configuration.dll copy /V ..\Lib\Microsoft.Practices.Unity.Interception.Configuration.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist ..\Lib\Microsoft.Practices.Unity.Interception.Configuration.xml copy /V ..\Lib\Microsoft.Practices.Unity.Interception.Configuration.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist ..\Lib\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapter.dll copy /V ..\Lib\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapter.dll %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Caching files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Common files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Common.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Common.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Common.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Common.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Configuration files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Data Access files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.xml %binDir%\.
@if errorlevel 1 goto :error


@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Exception Handling files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Logging files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\MSMQDistributor.exe copy /V %solutionDir%\bin\%buildType%\MSMQDistributor.exe %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\MsmqDistributor.exe.config copy /V %solutionDir%\bin\%buildType%\MsmqDistributor.exe.config %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Policy Injection files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Security files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Validation files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.dll copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.xml copy /V %solutionDir%\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO -----------------------------------------------
@ECHO Copy Configuration Tool files with verification
@ECHO -----------------------------------------------
@ECHO.

if Exist %solutionDir%\bin\%buildType%\EntLibConfig.exe copy /V %solutionDir%\bin\%buildType%\EntLibConfig.exe %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\EntLibConfig.exe.config copy /V %solutionDir%\bin\%buildType%\EntLibConfig.exe.config %binDir%\.
@if errorlevel 1 goto :error

if Exist %solutionDir%\bin\%buildType%\MergeConfiguration.exe copy /V %solutionDir%\bin\%buildType%\MergeConfiguration.exe %binDir%\.
@if errorlevel 1 goto :error
if Exist %solutionDir%\bin\%buildType%\MergeConfiguration.exe.config copy /V %solutionDir%\bin\%buildType%\MergeConfiguration.exe.config %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO CopyAssemblies.bat Completed
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
  @ECHO An error occured in CopyAssemblies.bat - %errorLevel%

if %pause%==true PAUSE
@exit errorLevel

:HELP
echo Usage: CopyAssemblies [/q] [build output folder] [destination dir]
echo.
echo CopyAssemblies is to be executed in the directory where EnterpriseLibrary.sln resides
echo The default build output folder is Debug
echo The default destintation directory is bin
echo.
echo Examples:
echo.
echo    "CopyAssemblies" - copies Debug build assemblies to bin      
echo    "CopyAssemblies Release" - copies Release build assemblies to bin
echo    "CopyAssemblies Release C:\temp" - copies Release build assemblies to C:\temp
echo.

@REM  ----------------------------------------
@REM  The exit label
@REM  ----------------------------------------
:exit
popd
set pause=
set solutionDir=
set buildType=
set prompt=%savedPrompt%
set savedPrompt=
echo on