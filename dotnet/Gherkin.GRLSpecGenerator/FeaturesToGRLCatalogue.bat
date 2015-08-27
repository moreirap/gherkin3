@echo off
@CHCP 850 > NUL
@set var=
@set fileNames=R48,R67,R63

@set "yourDir=D:\Dev\thesis\gherkin3\testdata\extendedBDD"
REM @for /f "usebackq tokens=*" %%a in (`dir "%yourDir%\*.feature" /b ^| findstr ".*" `) do @call set var=%%var%% "%yourDir%\%%~a"

call :parse "%fileNames%"
bin\Debug\Gherkin.GRLCatalogueGenerator.exe %var%
goto :end

:parse
set list=%1
set list=%list:"=%
FOR /f "tokens=1* delims=," %%a IN ("%list%") DO (
  if not "%%a" == "" call set var=%var% "%yourDir%\%%a.feature"
  if not "%%b" == "" call :parse "%%b"
)
exit /b

:end