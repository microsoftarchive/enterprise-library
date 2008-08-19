@echo off
@REM  ----------------------------------------------------------------------------
@REM  SetUpQuickStartsDB.bat file
@REM
@REM  This batch file calls the SQL script to create the tables required for the
@REM  Data Access QuickStart. The tables are created in the database named
@REM  EntLibQuickStarts. If the database does not yet exist, the SQL script
@REM  will create it. If the EntLibQuickStarts database already exists, the
@REM  script will add the tables required for the Data Access QuickStart to the
@REM  existing EntLibQuickStarts database.
@REM  
@REM  ----------------------------------------------------------------------------

echo.
echo =========================================================
echo   SetUpQuickStartsDB
echo      Creates the tables required for the Data Access
echo      QuickStart. The tables are created in the database
echo      EntLibQuickStarts. The script will create the 
echo      database if it does not exist.                           
echo =========================================================
echo.

set pause=true

if "%1"=="/?" goto HELP

if not Exist DataAccessQuickStarts.sql goto HELPSCRIPT

@REM  ----------------------------------------------------
@REM  If the first parameter is /q, do not pause
@REM  at the end of execution.
@REM  ----------------------------------------------------

if /i "%1"=="/q" (
 set pause=false
)

@REM  ------------------------------------------------
@REM  Shorten the command prompt for making the output
@REM  easier to read.
@REM  ------------------------------------------------
set savedPrompt=%prompt%
set prompt=*$g

@ECHO ----------------------------------------
@ECHO SetUpQuickStartsDB.bat Started
@ECHO ----------------------------------------
@ECHO.

OSQL -S (local)\SQLEXPRESS -E -i DataAccessQuickStarts.sql

@ECHO.
@ECHO ----------------------------------------
@ECHO SetUpQuickStartsDB.bat Completed
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
@ECHO An error occured in SetUpQuickStartsDB.bat - %errorLevel%
if %pause%==true PAUSE
@exit errorLevel

:HELPSCRIPT
echo Error: Unable to locate the required SQL script DataAccessQuickStarts.sql
echo.
goto exit

:HELP
echo Usage: SetUpQuickStartsDB.bat 
echo.

@REM  ----------------------------------------
@REM  The exit label
@REM  ----------------------------------------
:exit
if %pause%==true PAUSE

set pause=
set prompt=%savedPrompt%
set savedPrompt=

echo on

