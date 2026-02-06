namespace GavetaHotkeyApp.Models;

/// <summary>
/// Representa a configuração de um atalho de teclado
/// </summary>
public class HotkeyConfig
{
    public bool Ctrl { get; set; }
    public bool Shift { get; set; }
    public bool Alt { get; set; }
    public string Key { get; set; } = "G";

    public override string ToString()
    {
        var parts = new List<string>();
        if (Ctrl) parts.Add("CTRL");
        if (Shift) parts.Add("SHIFT");
        if (Alt) parts.Add("ALT");
        parts.Add(Key.ToUpper());
        return string.Join(" + ", parts);
    }

    public override bool Equals(object? obj)
    {
        if (obj is HotkeyConfig other)
        {
            return Ctrl == other.Ctrl && 
                   Shift == other.Shift && 
                   Alt == other.Alt && 
                   Key.Equals(other.Key, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Ctrl, Shift, Alt, Key.ToUpperInvariant());
    }
}
