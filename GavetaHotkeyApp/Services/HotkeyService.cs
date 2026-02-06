using GavetaHotkeyApp.Models;
using GavetaHotkeyApp.Native;

namespace GavetaHotkeyApp.Services;

/// <summary>
/// Serviço para registro e gerenciamento de hotkeys globais
/// </summary>
public class HotkeyService : IDisposable
{
    private readonly LogService _logService;
    private readonly Dictionary<int, HotkeyConfig> _registeredHotkeys = new();
    private IntPtr _windowHandle;
    private int _nextHotkeyId = 1;
    private bool _disposed;

    public event EventHandler<HotkeyConfig>? HotkeyPressed;

    public HotkeyService(LogService logService)
    {
        _logService = logService;
    }

    public void SetWindowHandle(IntPtr handle)
    {
        _windowHandle = handle;
    }

    /// <summary>
    /// Registra todos os hotkeys da configuração
    /// </summary>
    public void RegisterHotkeys(List<HotkeyConfig> hotkeys)
    {
        // Remove hotkeys existentes primeiro
        UnregisterAllHotkeys();

        foreach (var hotkey in hotkeys)
        {
            RegisterHotkey(hotkey);
        }
    }

    /// <summary>
    /// Registra um único hotkey
    /// </summary>
    public bool RegisterHotkey(HotkeyConfig hotkey)
    {
        if (_windowHandle == IntPtr.Zero)
        {
            _logService.LogError("", "Handle da janela não definido");
            return false;
        }

        try
        {
            uint modifiers = NativeMethods.MOD_NOREPEAT; // Evita repetição

            if (hotkey.Ctrl) modifiers |= NativeMethods.MOD_CONTROL;
            if (hotkey.Shift) modifiers |= NativeMethods.MOD_SHIFT;
            if (hotkey.Alt) modifiers |= NativeMethods.MOD_ALT;

            uint vk = GetVirtualKeyCode(hotkey.Key);
            if (vk == 0)
            {
                _logService.LogError("", $"Tecla inválida: {hotkey.Key}");
                return false;
            }

            int id = _nextHotkeyId++;

            if (NativeMethods.RegisterHotKey(_windowHandle, id, modifiers, vk))
            {
                _registeredHotkeys[id] = hotkey;
                _logService.LogInfo($"Hotkey registrado: {hotkey}");
                return true;
            }
            else
            {
                _logService.LogError("", $"Falha ao registrar hotkey: {hotkey}");
                return false;
            }
        }
        catch (Exception ex)
        {
            _logService.LogError("", $"Erro ao registrar hotkey: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Remove todos os hotkeys registrados
    /// </summary>
    public void UnregisterAllHotkeys()
    {
        foreach (var id in _registeredHotkeys.Keys.ToList())
        {
            NativeMethods.UnregisterHotKey(_windowHandle, id);
            _logService.LogInfo($"Hotkey removido: {_registeredHotkeys[id]}");
        }
        _registeredHotkeys.Clear();
    }

    /// <summary>
    /// Processa mensagem de hotkey do Windows
    /// </summary>
    public void ProcessHotkeyMessage(int hotkeyId)
    {
        if (_registeredHotkeys.TryGetValue(hotkeyId, out var hotkey))
        {
            HotkeyPressed?.Invoke(this, hotkey);
        }
    }

    /// <summary>
    /// Converte nome da tecla para código virtual
    /// </summary>
    private static uint GetVirtualKeyCode(string key)
    {
        if (string.IsNullOrEmpty(key))
            return 0;

        key = key.ToUpperInvariant();

        // Letras A-Z
        if (key.Length == 1 && char.IsLetter(key[0]))
        {
            return (uint)key[0];
        }

        // Números 0-9
        if (key.Length == 1 && char.IsDigit(key[0]))
        {
            return (uint)key[0];
        }

        // Teclas de função F1-F24
        if (key.StartsWith("F") && int.TryParse(key[1..], out int fNum) && fNum >= 1 && fNum <= 24)
        {
            return (uint)(0x70 + fNum - 1); // VK_F1 = 0x70
        }

        // Teclas especiais
        return key switch
        {
            "SPACE" => 0x20,
            "ENTER" => 0x0D,
            "TAB" => 0x09,
            "ESCAPE" or "ESC" => 0x1B,
            "BACKSPACE" => 0x08,
            "DELETE" or "DEL" => 0x2E,
            "INSERT" or "INS" => 0x2D,
            "HOME" => 0x24,
            "END" => 0x23,
            "PAGEUP" or "PGUP" => 0x21,
            "PAGEDOWN" or "PGDN" => 0x22,
            "UP" => 0x26,
            "DOWN" => 0x28,
            "LEFT" => 0x25,
            "RIGHT" => 0x27,
            "NUMPAD0" => 0x60,
            "NUMPAD1" => 0x61,
            "NUMPAD2" => 0x62,
            "NUMPAD3" => 0x63,
            "NUMPAD4" => 0x64,
            "NUMPAD5" => 0x65,
            "NUMPAD6" => 0x66,
            "NUMPAD7" => 0x67,
            "NUMPAD8" => 0x68,
            "NUMPAD9" => 0x69,
            _ => 0
        };
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            UnregisterAllHotkeys();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
