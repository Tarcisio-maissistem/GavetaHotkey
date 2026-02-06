using GavetaHotkeyApp.Models;
using GavetaHotkeyApp.Native;
using GavetaHotkeyApp.Services;

namespace GavetaHotkeyApp.Forms;

/// <summary>
/// Formulário principal de configuração
/// </summary>
public partial class MainForm : Form
{
    private readonly ConfigService _configService;
    private readonly PrinterService _printerService;
    private readonly HotkeyService _hotkeyService;
    private readonly NotificationService _notificationService;
    private readonly TrayService _trayService;
    private readonly LogService _logService;
    private DateTime? _lastExecution;

    public MainForm(
        ConfigService configService,
        PrinterService printerService,
        HotkeyService hotkeyService,
        NotificationService notificationService,
        TrayService trayService,
        LogService logService)
    {
        _configService = configService;
        _printerService = printerService;
        _hotkeyService = hotkeyService;
        _notificationService = notificationService;
        _trayService = trayService;
        _logService = logService;

        InitializeComponent();

        // Configura o handle para o serviço de hotkeys
        _hotkeyService.SetWindowHandle(this.Handle);
        _hotkeyService.HotkeyPressed += OnHotkeyPressed;

        // Registra hotkeys da configuração
        _hotkeyService.RegisterHotkeys(_configService.Config.Hotkeys);

        // Carrega dados na interface
        LoadPrinters();
        LoadConfigToUI();
    }

    protected override void WndProc(ref Message m)
    {
        // Processa mensagens de hotkey
        if (m.Msg == NativeMethods.WM_HOTKEY)
        {
            int hotkeyId = m.WParam.ToInt32();
            _hotkeyService.ProcessHotkeyMessage(hotkeyId);
        }
        base.WndProc(ref m);
    }

    private void OnHotkeyPressed(object? sender, HotkeyConfig hotkey)
    {
        ExecuteDrawerOpen();
    }

    private void ExecuteDrawerOpen()
    {
        var printerName = _configService.Config.PrinterName;
        
        if (string.IsNullOrEmpty(printerName))
        {
            _notificationService.ShowError("Nenhuma impressora configurada!");
            return;
        }

        bool success = _printerService.OpenDrawer(printerName);
        _lastExecution = DateTime.Now;

        if (success)
        {
            _notificationService.ShowSuccess();
            UpdateLastExecutionLabel(true);
        }
        else
        {
            _notificationService.ShowError($"Falha ao abrir gaveta na impressora {printerName}");
            UpdateLastExecutionLabel(false);
        }
    }

    private void UpdateLastExecutionLabel(bool success)
    {
        if (_lastExecution.HasValue)
        {
            var icon = success ? "✔" : "❌";
            var status = success ? "Sucesso" : "Falha";
            lblLastExecution.Text = $"{icon} {status} às {_lastExecution.Value:HH:mm:ss}";
            lblLastExecution.ForeColor = success ? Color.LightGreen : Color.Salmon;
        }
    }

    #region Printer Block

    private void LoadPrinters()
    {
        cmbPrinter.Items.Clear();
        var printers = _printerService.GetInstalledPrinters();
        foreach (var printer in printers)
        {
            cmbPrinter.Items.Add(printer);
        }
    }

    private void btnRefreshPrinters_Click(object sender, EventArgs e)
    {
        LoadPrinters();
        ValidatePrinterSelection();
    }

    private void cmbPrinter_SelectedIndexChanged(object sender, EventArgs e)
    {
        ValidatePrinterSelection();
    }

    private void ValidatePrinterSelection()
    {
        var selectedPrinter = cmbPrinter.SelectedItem?.ToString() ?? "";
        if (_printerService.PrinterExists(selectedPrinter))
        {
            lblPrinterStatus.Text = "✔ Impressora encontrada";
            lblPrinterStatus.ForeColor = Color.LightGreen;
        }
        else if (string.IsNullOrEmpty(selectedPrinter))
        {
            lblPrinterStatus.Text = "⚠ Selecione uma impressora";
            lblPrinterStatus.ForeColor = Color.Orange;
        }
        else
        {
            lblPrinterStatus.Text = "❌ Impressora não encontrada";
            lblPrinterStatus.ForeColor = Color.Salmon;
        }
    }

    #endregion

    #region Hotkey Block

    private void btnAddHotkey_Click(object sender, EventArgs e)
    {
        using var recorder = new HotkeyRecorderForm();
        if (recorder.ShowDialog(this) == DialogResult.OK && recorder.RecordedHotkey != null)
        {
            // Verifica duplicata
            var exists = lstHotkeys.Items.Cast<HotkeyConfig>()
                .Any(h => h.Equals(recorder.RecordedHotkey));

            if (exists)
            {
                MessageBox.Show("Este atalho já está configurado!", "Atenção", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lstHotkeys.Items.Add(recorder.RecordedHotkey);
        }
    }

    private void btnRemoveHotkey_Click(object sender, EventArgs e)
    {
        if (lstHotkeys.SelectedItem != null)
        {
            lstHotkeys.Items.Remove(lstHotkeys.SelectedItem);
        }
        else
        {
            MessageBox.Show("Selecione um atalho para remover.", "Atenção",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    #endregion

    #region Test Block

    private void btnTestDrawer_Click(object sender, EventArgs e)
    {
        var printerName = cmbPrinter.SelectedItem?.ToString();
        if (string.IsNullOrEmpty(printerName))
        {
            MessageBox.Show("Selecione uma impressora primeiro!", "Atenção",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Salva temporariamente para o teste
        var originalPrinter = _configService.Config.PrinterName;
        _configService.Config.PrinterName = printerName;

        ExecuteDrawerOpen();

        // Restaura se não salvou
        if (!_saved)
        {
            _configService.Config.PrinterName = originalPrinter;
        }
    }

    #endregion

    #region Save/Load Config

    private bool _saved = false;

    private void LoadConfigToUI()
    {
        // Impressora
        if (!string.IsNullOrEmpty(_configService.Config.PrinterName))
        {
            var index = cmbPrinter.Items.IndexOf(_configService.Config.PrinterName);
            if (index >= 0)
            {
                cmbPrinter.SelectedIndex = index;
            }
            else
            {
                cmbPrinter.Text = _configService.Config.PrinterName;
            }
        }
        ValidatePrinterSelection();

        // Hotkeys
        lstHotkeys.Items.Clear();
        foreach (var hotkey in _configService.Config.Hotkeys)
        {
            lstHotkeys.Items.Add(hotkey);
        }

        // Comportamento
        chkStartup.Checked = _trayService.IsStartupEnabled();
        chkMinimizeToTray.Checked = _configService.Config.MinimizeToTray;
        chkShowNotification.Checked = _configService.Config.ShowNotification;
        chkPlaySound.Checked = _configService.Config.PlaySound;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        // Valida impressora
        var printerName = cmbPrinter.SelectedItem?.ToString() ?? cmbPrinter.Text;
        if (string.IsNullOrWhiteSpace(printerName))
        {
            MessageBox.Show("Selecione uma impressora!", "Erro",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!_printerService.PrinterExists(printerName))
        {
            var result = MessageBox.Show(
                $"A impressora '{printerName}' não foi encontrada. Deseja salvar mesmo assim?",
                "Impressora não encontrada",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
                return;
        }

        // Valida hotkeys
        if (lstHotkeys.Items.Count == 0)
        {
            MessageBox.Show("Configure pelo menos um atalho!", "Erro",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Salva configuração
        _configService.Config.PrinterName = printerName;
        _configService.Config.Hotkeys = lstHotkeys.Items.Cast<HotkeyConfig>().ToList();
        _configService.Config.StartupWithWindows = chkStartup.Checked;
        _configService.Config.MinimizeToTray = chkMinimizeToTray.Checked;
        _configService.Config.ShowNotification = chkShowNotification.Checked;
        _configService.Config.PlaySound = chkPlaySound.Checked;

        try
        {
            _configService.Save();
            _saved = true;

            // Atualiza serviços
            _notificationService.ShowNotificationEnabled = chkShowNotification.Checked;
            _notificationService.PlaySoundEnabled = chkPlaySound.Checked;
            _trayService.SetStartupWithWindows(chkStartup.Checked);

            // Re-registra hotkeys
            _hotkeyService.RegisterHotkeys(_configService.Config.Hotkeys);

            MessageBox.Show("Configuração salva com sucesso!", "Sucesso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    #endregion

    #region Export/Import

    private void btnExport_Click(object sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog
        {
            Filter = "Arquivo JSON|*.json",
            FileName = "config_gaveta.json",
            Title = "Exportar Configuração"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                _configService.Export(dialog.FileName);
                MessageBox.Show("Configuração exportada com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao exportar: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "Arquivo JSON|*.json",
            Title = "Importar Configuração"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                _configService.Import(dialog.FileName);
                LoadConfigToUI();
                
                // Re-registra hotkeys
                _hotkeyService.RegisterHotkeys(_configService.Config.Hotkeys);
                
                MessageBox.Show("Configuração importada com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao importar: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    #endregion

    #region Form Events

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // Ao fechar a janela, minimiza para bandeja ao invés de sair
        if (e.CloseReason == CloseReason.UserClosing && _configService.Config.MinimizeToTray)
        {
            e.Cancel = true;
            this.Hide();
            _notificationService.ShowInfo("GavetaHotkeyApp continua rodando na bandeja");
        }
        base.OnFormClosing(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        
        // Se configurado para minimizar, esconde a janela principal
        if (_configService.Config.MinimizeToTray && Environment.GetCommandLineArgs().Contains("--minimized"))
        {
            this.Hide();
            this.ShowInTaskbar = false;
        }
    }

    #endregion
}
