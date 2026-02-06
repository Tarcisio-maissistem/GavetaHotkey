using GavetaHotkeyApp.Forms;
using GavetaHotkeyApp.Services;

namespace GavetaHotkeyApp;

static class Program
{
    private static Mutex? _mutex;
    private const string MutexName = "GavetaHotkeyApp_SingleInstance";

    [STAThread]
    static void Main()
    {
        // Verificação de instância única
        _mutex = new Mutex(true, MutexName, out bool createdNew);
        
        if (!createdNew)
        {
            MessageBox.Show(
                "GavetaHotkeyApp já está em execução.\nVerifique a bandeja do sistema.",
                "Aplicação já em execução",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        try
        {
            ApplicationConfiguration.Initialize();
            
            // Inicializa serviços
            var logService = new LogService();
            logService.LogInfo("=== Aplicação iniciada ===");

            var configService = new ConfigService(logService);
            configService.Load();

            var printerService = new PrinterService(logService);
            
            // Valida impressora configurada
            if (!string.IsNullOrEmpty(configService.Config.PrinterName))
            {
                if (!printerService.PrinterExists(configService.Config.PrinterName))
                {
                    logService.LogError(configService.Config.PrinterName, "Impressora configurada não encontrada");
                }
            }

            var hotkeyService = new HotkeyService(logService);
            var trayService = new TrayService(logService);

            var notificationService = new NotificationService(trayService.NotifyIcon, logService)
            {
                PlaySoundEnabled = configService.Config.PlaySound,
                ShowNotificationEnabled = configService.Config.ShowNotification
            };

            // Cria o formulário principal
            var mainForm = new MainForm(
                configService,
                printerService,
                hotkeyService,
                notificationService,
                trayService,
                logService);

            // Configura eventos da bandeja
            trayService.OpenConfigRequested += (s, e) =>
            {
                mainForm.Show();
                mainForm.BringToFront();
                mainForm.WindowState = FormWindowState.Normal;
            };

            trayService.TestDrawerRequested += (s, e) =>
            {
                var printer = configService.Config.PrinterName;
                if (string.IsNullOrEmpty(printer))
                {
                    notificationService.ShowError("Nenhuma impressora configurada!");
                    return;
                }

                bool success = printerService.OpenDrawer(printer);
                if (success)
                {
                    notificationService.ShowSuccess();
                }
                else
                {
                    notificationService.ShowError($"Falha ao abrir gaveta na impressora {printer}");
                }
            };

            trayService.ExitRequested += (s, e) =>
            {
                hotkeyService.Dispose();
                trayService.Dispose();
                logService.LogInfo("=== Aplicação encerrada ===");
                Application.Exit();
            };

            // Executa a aplicação
            // Se deve iniciar minimizado
            if (configService.Config.MinimizeToTray)
            {
                mainForm.WindowState = FormWindowState.Minimized;
                mainForm.ShowInTaskbar = false;
                mainForm.Hide();
            }

            Application.Run(mainForm);
        }
        finally
        {
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
        }
    }
}
