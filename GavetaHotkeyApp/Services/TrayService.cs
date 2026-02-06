using Microsoft.Win32;

namespace GavetaHotkeyApp.Services;

/// <summary>
/// Serviço para gerenciamento do ícone na bandeja do sistema
/// </summary>
public class TrayService : IDisposable
{
    private readonly NotifyIcon _notifyIcon;
    private readonly LogService _logService;
    private bool _disposed;

    public event EventHandler? OpenConfigRequested;
    public event EventHandler? TestDrawerRequested;
    public event EventHandler? ExitRequested;

    public NotifyIcon NotifyIcon => _notifyIcon;

    public TrayService(LogService logService)
    {
        _logService = logService;
        _notifyIcon = new NotifyIcon
        {
            Text = "GavetaHotkeyApp - Clique para configurar",
            Visible = true
        };

        // Tenta carregar ícone customizado, senão usa ícone padrão
        LoadIcon();

        // Configura menu de contexto
        CreateContextMenu();

        // Duplo-clique abre configurações
        _notifyIcon.DoubleClick += (s, e) => OpenConfigRequested?.Invoke(this, EventArgs.Empty);
    }

    private void LoadIcon()
    {
        try
        {
            var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "icon.ico");
            if (File.Exists(iconPath))
            {
                _notifyIcon.Icon = new Icon(iconPath);
            }
            else
            {
                // Usa ícone do próprio executável
                _notifyIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
        }
        catch
        {
            _notifyIcon.Icon = SystemIcons.Application;
        }
    }

    private void CreateContextMenu()
    {
        var contextMenu = new ContextMenuStrip();

        var configItem = new ToolStripMenuItem("⚙️ Configurações");
        configItem.Click += (s, e) => OpenConfigRequested?.Invoke(this, EventArgs.Empty);
        contextMenu.Items.Add(configItem);

        var testItem = new ToolStripMenuItem("🧪 Testar Gaveta");
        testItem.Click += (s, e) => TestDrawerRequested?.Invoke(this, EventArgs.Empty);
        contextMenu.Items.Add(testItem);

        contextMenu.Items.Add(new ToolStripSeparator());

        var exitItem = new ToolStripMenuItem("❌ Sair");
        exitItem.Click += (s, e) => ExitRequested?.Invoke(this, EventArgs.Empty);
        contextMenu.Items.Add(exitItem);

        _notifyIcon.ContextMenuStrip = contextMenu;
    }

    /// <summary>
    /// Registra ou remove a aplicação do início automático do Windows
    /// </summary>
    public void SetStartupWithWindows(bool enable)
    {
        const string keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        const string valueName = "GavetaHotkeyApp";

        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(keyPath, true);
            if (key == null)
            {
                _logService.LogError("", "Não foi possível acessar registro do Windows");
                return;
            }

            if (enable)
            {
                var exePath = Application.ExecutablePath;
                key.SetValue(valueName, $"\"{exePath}\"");
                _logService.LogInfo("Início automático ativado");
            }
            else
            {
                key.DeleteValue(valueName, false);
                _logService.LogInfo("Início automático desativado");
            }
        }
        catch (Exception ex)
        {
            _logService.LogError("", $"Erro ao configurar início automático: {ex.Message}");
        }
    }

    /// <summary>
    /// Verifica se o início automático está ativado
    /// </summary>
    public bool IsStartupEnabled()
    {
        const string keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        const string valueName = "GavetaHotkeyApp";

        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(keyPath, false);
            return key?.GetValue(valueName) != null;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
