@echo off
@CHCP 850 > NUL
@set var=
@set fileNames=R54

@set "yourDir=D:\Dev\thesis\gherkin3\testdata\extendedBDD"

call :parse "%fileNames%"
bin\Debug\Gherkin.AstGenerator.exe %var%
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