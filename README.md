# GavetaHotkeyApp

Aplicação desktop leve e eficiente para Windows que permite a automação da abertura de gavetas de dinheiro conectadas a impressoras térmicas (Epson ou compatíveis ESC/POS) através de atalhos de teclado globais.

## 🎯 Objetivo e Funcionalidade

O **GavetaHotkeyApp** resolve o problema comum de PDVs (Pontos de Venda) que precisam abrir a gaveta de dinheiro sem a necessidade de imprimir um cupom fiscal ou navegar por menus complexos do software de vendas. Com um simples comando de teclado (Hotkeys), o sinal elétrico é enviado para a impressora, que por sua vez aciona o solenoide da gaveta.

### Principais Características
- **Execução em Segundo Plano:** Ocupa recursos mínimos e reside na bandeja do sistema (System Tray).
- **Atalhos Globais:** Funciona mesmo quando a aplicação não está em foco (ex: você está no navegador ou em outro software).
- **Múltiplos Atalhos:** Permite configurar várias combinações de teclas para a mesma ação.
- **Feedback Imediato:** Notificações visuais e sonoras confirmam a execução do comando.
- **Configuração Flexível:** Interface simples para troca de impressora e definição de teclas.

---

## ⚙️ Casos de Uso

1. **Agilidade no Troco:** Abrir a gaveta rapidamente para fornecer troco sem emitir nova nota.
2. **Conferência de Caixa:** Acesso rápido ao dinheiro para contagem durante o turno.
3. **Sistemas Legados:** Adicionar funcionalidade de abertura de gaveta a softwares antigos que não possuem essa opção nativa.
4. **Independência de Software:** Funciona independente do software de frente de caixa que está sendo utilizado.

---

## 🚀 Instalação e Requisitos

### Pré-requisitos
- **Windows 10 ou 11 (64-bit)**.
- **.NET 8 SDK** (O instalador tenta baixar automaticamente, mas pode ser instalado via [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/8.0)).
- **Impressora Térmica** com suporte a comandos ESC/POS instalada no Windows.

### Como Instalar
1. Clique com o botão direito em `instalar.bat` e selecione **"Executar como Administrador"**.
2. O instalador irá verificar o .NET, compilar o executável e criar um atalho na Área de Trabalho.
3. A aplicação será instalada em `C:\Program Files\GavetaHotkeyApp`.

---

## 🛠️ Detalhes Técnicos

### Funções Internas
- **PrinterService:** Utiliza WinAPI (`winspool.drv`) para enviar comandos RAW (ESC/POS) diretamente para o spooler da impressora. O comando padrão é `27, 112, 0, 25, 250`.
- **HotkeyService:** Registra teclas no Windows via `user32.dll` (`RegisterHotKey`), garantindo que a captura ocorra em nível de sistema.
- **ConfigService:** Gerencia o arquivo `config.json` para persistência das preferências do usuário.
- **TrayService:** Gerencia a persistência na bandeja e o menu de contexto.

### Inicialização e Serviço
Embora a aplicação seja um executável WinForms (para permitir a interface de configuração), ela se comporta como um serviço:
- **Início Automático:** Pode ser configurada para iniciar com o Windows via Registro (`HKCU\Software\Microsoft\Windows\CurrentVersion\Run`).
- **Instância Única:** Utiliza um `Mutex` para garantir que apenas uma cópia do app esteja rodando, evitando conflitos de porta de impressora ou hotkeys.

---

## 📂 Estrutura do Projeto
- `MainForm`: Interface principal organizando seleção de impressora, atalhos, comportamento e testes.
- `HotkeyRecorderForm`: Modal especializado na captura intuitiva de teclas (o usuário apenas pressiona as teclas, o sistema detecta).
- `NativeMethods`: Ponte para as funcionalidades de baixo nível do Windows.

---

## 📊 Logs
O sistema gera um arquivo `logs.txt` no diretório de instalação que registra cada tentativa de abertura, o timestamp e o status (Sucesso ou Erro), facilitando o diagnóstico em caso de problemas de hardware.

## ❌ Desinstalação
Para remover completamente, utilize o `desinstalar.bat` como Administrador. Ele removerá os arquivos, o atalho e a chave de registro de inicialização.

---
*Desenvolvido para máxima performance e utilidade profissional em ambientes de varejo.*
