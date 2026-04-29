@echo off
setlocal enabledelayedexpansion

echo Listing all bin and obj directories...
set "counter=0"

for /d /r %%d in (bin obj) do (
    if exist "%%d" (
        set /a counter+=1
        set "dirList[!counter!]=%%d"
        echo !counter!. %%d
    )
)

if %counter%==0 (
    echo No bin or obj directories found.
    pause
    exit /b
)

:menu
echo.
echo Choose an option:
echo [A] Delete ALL listed directories
echo [N] Delete a specific directory by entering its number
echo [E] Exit without deleting
set /p choice="Enter your choice (A/N/E): "

if /i "%choice%"=="A" (
    for /l %%i in (1,1,%counter%) do (
        rd /s /q "!dirList[%%i]!"
        echo Deleted: !dirList[%%i]!
    )
    echo All directories deleted.
    pause
    exit /b
)

if /i "%choice%"=="N" (
    set /p delNum="Enter the number of the directory to delete: "
    if defined dirList[%delNum%] (
        rd /s /q "!dirList[%delNum%]!"
        echo Deleted: !dirList[%delNum%]!
    ) else (
        echo Invalid selection.
    )
    goto menu
)

if /i "%choice%"=="E" (
    echo Exiting script...
    pause
    exit /b
)

echo Invalid choice, try again.
goto menu
