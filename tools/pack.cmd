@echo off

for /f "tokens=* USEBACKQ" %%f in (`type version`) do set version=%%f
set releaseconfig=%1
if "%releaseconfig%" == "" set releaseconfig=Release

:packbin
echo Packing binary...
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%version%-bin.zip "..\LineNumbers.Core\bin\%releaseconfig%\net6.0\*"
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%version%-demo.zip "..\LineNumbers\bin\%releaseconfig%\net6.0\*"
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%version%-bin48.zip "..\LineNumbers.Core\bin\%releaseconfig%\net48\*"
if %errorlevel% == 0 goto :complete
echo There was an error trying to pack binary (%errorlevel%).
goto :finished

:complete
move %temp%\%version%-bin.zip
move %temp%\%version%-demo.zip
move %temp%\%version%-bin48.zip

echo Pack successful.
:finished
