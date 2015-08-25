@echo off
@cls
@CHCP 850 > NUL
@set var=
@set "yourDir=..\..\testdata\extendedBDD"
@for /f "usebackq tokens=*" %%a in (`dir "%yourDir%\R*8*.feature" /b `) do @call set var=%%var%% "%yourDir%\%%~a"
REM set var

bin\Debug\Gherkin.GRLCatalogueGenerator.exe %var%