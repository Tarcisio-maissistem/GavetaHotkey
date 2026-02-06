@echo off
chcp 65001 > nul
title GavetaHotkeyApp - Desinstalador
color 0E

echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║           GAVETAHOTKEYAPP - DESINSTALADOR                  ║
echo ╚════════════════════════════════════════════════════════════╝
echo.

set "INSTALL_DIR=%ProgramFiles%\GavetaHotkeyApp"
set "APP_NAME=GavetaHotkeyApp"
set "DESKTOP=%USERPROFILE%\Desktop"

echo [AVISO] Este processo ira remover o GavetaHotkeyApp do seu sistema.
echo.
set /p CONFIRM="Deseja continuar? (S/N): "
if /i not "%CONFIRM%"=="S" (
    echo Desinstalacao cancelada.
    pause
    exit /b 0
)

echo.
echo [1/4] Encerrando aplicacao se estiver em execucao...
taskkill /f /im GavetaHotkeyApp.exe >nul 2>&1
echo        Concluido!
echo.

echo [2/4] Removendo do inicio automatico...
reg delete "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" /v "GavetaHotkeyApp" /f >nul 2>&1
echo        Concluido!
echo.

echo [3/4] Removendo arquivos de instalacao...
if exist "%INSTALL_DIR%" (
    rmdir /s /q "%INSTALL_DIR%"
    echo        Diretorio removido: %INSTALL_DIR%
) else (
    echo        Diretorio nao encontrado.
)
echo.

echo [4/4] Removendo atalho da area de trabalho...
if exist "%DESKTOP%\GavetaHotkeyApp.lnk" (
    del "%DESKTOP%\GavetaHotkeyApp.lnk"
    echo        Atalho removido!
) else (
    echo        Atalho nao encontrado.
)
echo.

echo ╔════════════════════════════════════════════════════════════╗
echo ║            DESINSTALACAO CONCLUIDA COM SUCESSO!            ║
echo ╚════════════════════════════════════════════════════════════╝
echo.
echo Pressione qualquer tecla para sair...
pause > nul
