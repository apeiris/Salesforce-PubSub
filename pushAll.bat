@echo off
setlocal

:: Allow commit messages to be passed as args
set SUBMSG=%1
set MAINMSG=%2

if "%SUBMSG%"=="" set SUBMSG=Updated submodule
if "%MAINMSG%"=="" set MAINMSG=Updated submodule pointer in main repo

cd SqlServerLib
git add .
git commit -m "%SUBMSG%"
git push

cd ..
git add SqlServerLib
git commit -m "%MAINMSG%"
git push

echo === Done ===
endlocal
pause
