# C-sharp-utils
small tools in c#

## Compilation command:
c:\Windows\Microsoft.NET\Framework\v3.5\csc <sourceToCompile.cs>

- printhex.cs dump file in hexadecimal.

# MS-DOS Snippets
Tricky but usefull snippets


## DIR to list files only

```dosbatch DIR /A-D <path> > <somewhere>\dir.txt```

The output of the previous DIR command could be redirected to a file. So it's easy to manually exclude certain lines from the output file. 
After what, the list could be used to apply the same process to the list of files.

## for loop on lines of a file

```bat
for /F "tokens=*" %%A in (dir.txt) do [process] %%A
```
sauce : https://stackoverflow.com/questions/155932/how-do-you-loop-through-each-line-in-a-text-file-using-a-windows-batch-file

For example, let's get a simple 'post-processed' DIR output :
```
20/09/2017  07:38                 0 IntelCpHDCPSvc.log
20/09/2017  07:38                 0 IntelCPHS.log
20/09/2017  09:31           127ÿ526 IntelGFX.log
20/09/2017  09:31            21ÿ804 IntelGFXCoin.log
```

Some interresting facts here :
- the files size are formatted, with a thousands separator `ÿ` or `0xFF` (high-value), not a space.
- File names are on the fourth column

```
c:\Temp>for /F "tokens=4" %a in (c:\temp\dir.txt) do echo %a

c:\Temp>echo IntelCpHDCPSvc.log
IntelCpHDCPSvc.log

c:\Temp>echo IntelCPHS.log
IntelCPHS.log

c:\Temp>echo IntelGFX.log
IntelGFX.log

c:\Temp>echo IntelGFXCoin.log
IntelGFXCoin.log

c:\Temp>
```

I could elaborate a bit more, in a batch script :
```bat
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
```

```
c:\Temp>run_on_dir.bat dir.txt echo

c:\Temp>ECHO OFF
IntelCpHDCPSvc.log
IntelCPHS.log
IntelGFX.log
IntelGFXCoin.log

c:\Temp>
```