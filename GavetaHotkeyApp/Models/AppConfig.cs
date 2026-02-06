namespace GavetaHotkeyApp.Models;

/// <summary>
/// Configuração principal da aplicação
/// </summary>
public class AppConfig
{
    public string PrinterName { get; set; } = "";
    public List<HotkeyConfig> Hotkeys { get; set; } = new()
    {
        new HotkeyConfig { Ctrl = true, Shift = true, Alt = false, Key = "G" }
    };
    public bool StartupWithWindows { get; set; } = false;
    public bool PlaySound { get; set; } = true;
    public bool ShowNotification { get; set; } = true;
    public bool MinimizeToTray { get; set; } = true;
}
