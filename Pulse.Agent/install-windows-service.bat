@echo off
setlocal

REM Check if running as administrator
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo Requesting administrative privileges...
    REM Elevate and maintain the script's directory
    PowerShell -NoProfile -Command "cd '%~dp0'; Start-Process -FilePath '%~f0' -Verb RunAs"
    exit /b
)

REM Now running as administrator - ensure we're in the script's directory
cd /d "%~dp0"

set SERVICE_NAME=PulseAgent
set INSTALL_PATH=C:\Pulse.Agent
set EXE_PATH=%INSTALL_PATH%\Pulse.Agent.exe


echo ========================================
echo Creating installation directory

echo ========================================

if not exist %INSTALL_PATH% (
    mkdir %INSTALL_PATH%
)

echo.
echo ========================================
echo Stopping Windows Service

echo ========================================

sc stop %SERVICE_NAME% >nul 2>&1
sc delete %SERVICE_NAME% >nul 2>&1



echo.
echo ========================================
echo Copying files

echo ========================================

xcopy /E /I /Y publish\win-x64\* %INSTALL_PATH%


echo.
echo ========================================
echo Creating Windows Service

echo ========================================


sc create %SERVICE_NAME% binPath= "%EXE_PATH%" start= auto

if %ERRORLEVEL% neq 0 (
    echo Failed to create service.
    exit /b %ERRORLEVEL%
)


echo.
echo ========================================
echo Starting service

echo ========================================

sc start %SERVICE_NAME%


echo.
echo ========================================
echo Service installed successfully.
echo ========================================

pause