@echo off
setlocal

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
echo Copying files

echo ========================================

xcopy /E /I /Y publish\win-x64\* %INSTALL_PATH%


echo.
echo ========================================
echo Creating Windows Service

echo ========================================

sc stop %SERVICE_NAME% >nul 2>&1
sc delete %SERVICE_NAME% >nul 2>&1

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