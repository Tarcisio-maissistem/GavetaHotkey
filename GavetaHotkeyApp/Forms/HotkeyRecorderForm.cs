using GavetaHotkeyApp.Models;

namespace GavetaHotkeyApp.Forms;

/// <summary>
/// Formulário modal para captura de atalhos de teclado
/// </summary>
public partial class HotkeyRecorderForm : Form
{
    private bool _ctrl;
    private bool _shift;
    private bool _alt;
    private string _key = "";

    public HotkeyConfig? RecordedHotkey { get; private set; }

    public HotkeyRecorderForm()
    {
        InitializeComponent();
        this.KeyPreview = true;
    }

    private void HotkeyRecorderForm_KeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;
        e.SuppressKeyPress = true;

        // ESC cancela
        if (e.KeyCode == Keys.Escape)
        {
            RecordedHotkey = null;
            DialogResult = DialogResult.Cancel;
            Close();
            return;
        }

        // Atualiza modificadores
        _ctrl = e.Control;
        _shift = e.Shift;
        _alt = e.Alt;

        // Verifica se é apenas modificador ou tecla válida
        if (e.KeyCode != Keys.ControlKey && 
            e.KeyCode != Keys.ShiftKey && 
            e.KeyCode != Keys.Menu &&
            e.KeyCode != Keys.LControlKey &&
            e.KeyCode != Keys.RControlKey &&
            e.KeyCode != Keys.LShiftKey &&
            e.KeyCode != Keys.RShiftKey &&
            e.KeyCode != Keys.LMenu &&
            e.KeyCode != Keys.RMenu)
        {
            // É uma tecla válida
            _key = e.KeyCode.ToString();

            // Precisa ter pelo menos um modificador
            if (!_ctrl && !_shift && !_alt)
            {
                lblStatus.Text = "⚠️ Use pelo menos CTRL, SHIFT ou ALT";
                lblStatus.ForeColor = Color.OrangeRed;
                return;
            }

            // Cria o hotkey
            RecordedHotkey = new HotkeyConfig
            {
                Ctrl = _ctrl,
                Shift = _shift,
                Alt = _alt,
                Key = _key
            };

            DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            // Mostra estado atual dos modificadores
            UpdateModifierDisplay();
        }
    }

    private void HotkeyRecorderForm_KeyUp(object sender, KeyEventArgs e)
    {
        _ctrl = e.Control;
        _shift = e.Shift;
        _alt = e.Alt;
        UpdateModifierDisplay();
    }

    private void UpdateModifierDisplay()
    {
        var parts = new List<string>();
        if (_ctrl) parts.Add("CTRL");
        if (_shift) parts.Add("SHIFT");
        if (_alt) parts.Add("ALT");
        parts.Add("...");

        lblCurrentKeys.Text = string.Join(" + ", parts);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        RecordedHotkey = null;
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
