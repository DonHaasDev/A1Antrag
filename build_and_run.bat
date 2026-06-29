@echo off
title A1Antrag - Build und Start

echo.
echo ============================================
echo  A1Antrag - Build und Start
echo ============================================
echo.

rem Pruefen ob Config.cs vorhanden ist (enthaelt Zugangsdaten, nicht im Repo)
if not exist "A1Antrag\Config.cs" (
    echo [FEHLER] Config.cs nicht gefunden!
    echo.
    echo Bitte die Datei anlegen:
    echo   A1Antrag\Config.cs
    echo.
    echo Vorlage:
    echo   A1Antrag\Config.example.cs
    echo   ^(kopieren nach Config.cs und Passwort eintragen^)
    echo.
    pause
    exit /b 1
)

echo [1/2] Bauen ...
echo.
dotnet build A1Antrag.sln --configuration Debug --nologo

if errorlevel 1 (
    echo.
    echo [FEHLER] Build fehlgeschlagen. Siehe Fehlermeldung oben.
    echo.
    pause
    exit /b 1
)

echo.
echo [2/2] Starten ...
echo.
start "" "A1Antrag\bin\Debug\net10.0-windows\A1Antrag.exe"
