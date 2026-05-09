@echo off
setlocal


set CONFIG=Release
set OUTPUT=publish

echo ========================================
echo Publishing Windows x64
echo ========================================

dotnet publish -c %CONFIG% -r win-x64 --self-contained false -o %OUTPUT%\win-x64

if %ERRORLEVEL% neq 0 (
    echo Windows publish failed.
    exit /b %ERRORLEVEL%
)


echo.
echo ========================================
echo Publishing Linux x64
echo ========================================

dotnet publish -c %CONFIG% -r linux-x64 --self-contained false -o %OUTPUT%\linux-x64

if %ERRORLEVEL% neq 0 (
    echo Linux publish failed.
    exit /b %ERRORLEVEL%
)


echo.
echo ========================================
echo Publish completed successfully.
echo ========================================

pause