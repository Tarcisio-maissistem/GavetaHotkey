@echo off
chcp 65001 > nul
title GavetaHotkeyApp - Instalador Completo
color 0A

echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║           GAVETAHOTKEYAPP - INSTALADOR COMPLETO            ║
echo ║         Abertura de Gaveta via Atalho de Teclado           ║
echo ╚════════════════════════════════════════════════════════════╝
echo.

:: Verificar se está sendo executado como administrador
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [ERRO] Este instalador precisa ser executado como Administrador!
    echo        Clique com botao direito e selecione "Executar como administrador"
    echo.
    pause
    exit /b 1
)

:: Diretórios
set "INSTALL_DIR=%ProgramFiles%\GavetaHotkeyApp"
set "APP_NAME=GavetaHotkeyApp"
set "EXE_NAME=GavetaHotkeyApp.exe"

:: ═══════════════════════════════════════════════════════════════
:: [1] VERIFICAR E INSTALAR .NET SDK
:: ═══════════════════════════════════════════════════════════════
echo [1/6] Verificando .NET SDK...

:: Verificar se dotnet existe
where dotnet >nul 2>&1
if %errorLevel% neq 0 (
    echo        .NET nao encontrado. Instalando...
    goto INSTALL_DOTNET
)

:: Verificar versão do SDK
set "DOTNET_OK=0"
for /f "tokens=*" %%i in ('dotnet --list-sdks 2^>nul ^| findstr /b "8."') do set "DOTNET_OK=1"

if "%DOTNET_OK%"=="0" (
    echo        .NET 8 SDK nao encontrado. Instalando...
    goto INSTALL_DOTNET
)

echo        .NET 8 SDK encontrado!
goto SKIP_DOTNET_INSTALL

:INSTALL_DOTNET
echo.
echo        Baixando script de instalacao oficial da Microsoft...
echo.

:: Criar pasta temporária
if not exist "%TEMP%\GavetaInstall" mkdir "%TEMP%\GavetaInstall"

:: Baixar o script oficial dotnet-install.ps1
powershell -Command "& {[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; try { Invoke-WebRequest -Uri 'https://dot.net/v1/dotnet-install.ps1' -OutFile '%TEMP%\GavetaInstall\dotnet-install.ps1' -UseBasicParsing } catch { Write-Host 'DOWNLOAD_ERROR'; exit 1 }}"

if not exist "%TEMP%\GavetaInstall\dotnet-install.ps1" (
    echo.
    echo [ERRO] Falha ao baixar o script de instalacao!
    echo.
    echo        Opcoes:
    echo        1. Verifique sua conexao com a internet
    echo        2. Baixe e instale manualmente o .NET 8 SDK:
    echo           https://dotnet.microsoft.com/download/dotnet/8.0
    echo        3. Apos instalar, execute este instalador novamente.
    echo.
    pause
    exit /b 1
)

echo        Script baixado! Instalando .NET 8 SDK...
echo        (Isso pode levar alguns minutos, aguarde...)
echo.

:: Executar o script de instalação do .NET 8 SDK
powershell -ExecutionPolicy Bypass -File "%TEMP%\GavetaInstall\dotnet-install.ps1" -Channel 8.0 -InstallDir "%ProgramFiles%\dotnet"

if %errorLevel% neq 0 (
    echo.
    echo [AVISO] Houve um problema na instalacao automatica.
    echo        Tentando metodo alternativo...
    echo.
    
    :: Método alternativo: instalar para o usuário atual
    powershell -ExecutionPolicy Bypass -File "%TEMP%\GavetaInstall\dotnet-install.ps1" -Channel 8.0
)

:: Adicionar ao PATH da sessão atual
set "PATH=%PATH%;%ProgramFiles%\dotnet;%LOCALAPPDATA%\Microsoft\dotnet"

:: Limpar arquivo temporário
del "%TEMP%\GavetaInstall\dotnet-install.ps1" >nul 2>&1

echo.
echo        .NET 8 SDK instalado!

:SKIP_DOTNET_INSTALL
echo.

:: Verificar novamente se dotnet funciona
where dotnet >nul 2>&1
if %errorLevel% neq 0 (
    :: Tentar encontrar dotnet em locais conhecidos
    if exist "%ProgramFiles%\dotnet\dotnet.exe" (
        set "PATH=%PATH%;%ProgramFiles%\dotnet"
    ) else if exist "%LOCALAPPDATA%\Microsoft\dotnet\dotnet.exe" (
        set "PATH=%PATH%;%LOCALAPPDATA%\Microsoft\dotnet"
    ) else (
        echo.
        echo [ERRO] .NET SDK nao foi detectado apos instalacao.
        echo        Por favor, reinicie o computador e execute novamente.
        echo        Ou instale manualmente: https://dotnet.microsoft.com/download/dotnet/8.0
        pause
        exit /b 1
    )
)

:: ═══════════════════════════════════════════════════════════════
:: [2] COMPILAR APLICAÇÃO
:: ═══════════════════════════════════════════════════════════════
echo [2/6] Compilando aplicacao...
cd /d "%~dp0"

:: Restaurar pacotes
dotnet restore GavetaHotkeyApp\GavetaHotkeyApp.csproj --verbosity quiet 2>nul

:: Publicar como single file auto-contido
dotnet publish GavetaHotkeyApp\GavetaHotkeyApp.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish --verbosity quiet

if %errorLevel% neq 0 (
    echo.
    echo [ERRO] Falha ao compilar a aplicacao!
    echo        Verifique se todos os arquivos do projeto estao presentes.
    echo.
    echo        Tentando com mais detalhes...
    dotnet publish GavetaHotkeyApp\GavetaHotkeyApp.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
    if %errorLevel% neq 0 (
        pause
        exit /b 1
    )
)

echo        Compilacao concluida!
echo.

:: ═══════════════════════════════════════════════════════════════
:: [3] CRIAR DIRETÓRIO DE INSTALAÇÃO
:: ═══════════════════════════════════════════════════════════════
echo [3/6] Criando diretorio de instalacao...
if not exist "%INSTALL_DIR%" (
    mkdir "%INSTALL_DIR%"
)
if not exist "%INSTALL_DIR%\Resources" (
    mkdir "%INSTALL_DIR%\Resources"
)

echo        Diretorio: %INSTALL_DIR%
echo.

:: ═══════════════════════════════════════════════════════════════
:: [4] COPIAR ARQUIVOS
:: ═══════════════════════════════════════════════════════════════
echo [4/6] Copiando arquivos...

:: Copiar executável
copy /Y "publish\%EXE_NAME%" "%INSTALL_DIR%\" >nul

:: Copiar config.json se existir
if exist "GavetaHotkeyApp\config.json" (
    copy /Y "GavetaHotkeyApp\config.json" "%INSTALL_DIR%\" >nul
)

:: Copiar recursos
if exist "GavetaHotkeyApp\Resources\success.wav" (
    copy /Y "GavetaHotkeyApp\Resources\success.wav" "%INSTALL_DIR%\Resources\" >nul
)
if exist "GavetaHotkeyApp\Resources\icon.ico" (
    copy /Y "GavetaHotkeyApp\Resources\icon.ico" "%INSTALL_DIR%\Resources\" >nul
)

echo        Arquivos copiados!
echo.

:: ═══════════════════════════════════════════════════════════════
:: [5] CRIAR ATALHO NA ÁREA DE TRABALHO
:: ═══════════════════════════════════════════════════════════════
echo [5/6] Criando atalho na area de trabalho...
set "DESKTOP=%USERPROFILE%\Desktop"
set "SHORTCUT=%DESKTOP%\GavetaHotkeyApp.lnk"

:: Criar VBScript temporário para criar atalho
echo Set oWS = WScript.CreateObject("WScript.Shell") > "%temp%\CreateShortcut.vbs"
echo sLinkFile = "%SHORTCUT%" >> "%temp%\CreateShortcut.vbs"
echo Set oLink = oWS.CreateShortcut(sLinkFile) >> "%temp%\CreateShortcut.vbs"
echo oLink.TargetPath = "%INSTALL_DIR%\%EXE_NAME%" >> "%temp%\CreateShortcut.vbs"
echo oLink.WorkingDirectory = "%INSTALL_DIR%" >> "%temp%\CreateShortcut.vbs"
echo oLink.Description = "GavetaHotkeyApp - Abertura de Gaveta por Atalho" >> "%temp%\CreateShortcut.vbs"
echo oLink.Save >> "%temp%\CreateShortcut.vbs"
cscript /nologo "%temp%\CreateShortcut.vbs"
del "%temp%\CreateShortcut.vbs"

echo        Atalho criado!
echo.

:: ═══════════════════════════════════════════════════════════════
:: [6] CONFIGURAÇÕES FINAIS
:: ═══════════════════════════════════════════════════════════════
echo [6/6] Finalizando...

:: Adicionar regra de firewall (caso necessário)
netsh advfirewall firewall delete rule name="GavetaHotkeyApp" >nul 2>&1
netsh advfirewall firewall add rule name="GavetaHotkeyApp" dir=in action=allow program="%INSTALL_DIR%\%EXE_NAME%" enable=yes >nul 2>&1

echo        Pronto!
echo.

:: ═══════════════════════════════════════════════════════════════
:: CONCLUÍDO
:: ═══════════════════════════════════════════════════════════════
echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║              INSTALACAO CONCLUIDA COM SUCESSO!             ║
echo ╚════════════════════════════════════════════════════════════╝
echo.
echo   Localizacao: %INSTALL_DIR%
echo.
echo   ┌─────────────────────────────────────────────────────────┐
echo   │ COMO USAR:                                              │
echo   │                                                         │
echo   │ 1. Execute o atalho "GavetaHotkeyApp" na area de        │
echo   │    trabalho                                             │
echo   │                                                         │
echo   │ 2. Selecione sua impressora termica na lista            │
echo   │                                                         │
echo   │ 3. Clique em "Adicionar" e pressione o atalho desejado  │
echo   │    (ex: CTRL + SHIFT + G)                               │
echo   │                                                         │
echo   │ 4. Clique em "Salvar"                                   │
echo   │                                                         │
echo   │ 5. O app fica na bandeja do sistema aguardando!         │
echo   └─────────────────────────────────────────────────────────┘
echo.

set /p EXECUTAR="Deseja executar o aplicativo agora? (S/N): "
if /i "%EXECUTAR%"=="S" (
    start "" "%INSTALL_DIR%\%EXE_NAME%"
)

echo.
echo Pressione qualquer tecla para sair...
pause > nul
