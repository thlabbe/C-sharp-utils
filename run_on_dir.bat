ECHO OFF
REM usage :
REM    run_on_dir.bat <list> <command>
REM
REM    <list>    : output from a DIR command with only files to process
REM    <command> : command to execute against each files from the list
REM
REM example :
REM    run_on_dir.bat dir.txt echo

set "list=%1"
set "command=%2"

for /F "tokens=4" %%a in (%list%) do (
  call %command% %%a
)