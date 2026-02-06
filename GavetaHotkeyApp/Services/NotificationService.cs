using System.Media;

namespace GavetaHotkeyApp.Services;

/// <summary>
/// Serviço de notificações visuais e sonoras
/// </summary>
public class NotificationService
{
    private readonly NotifyIcon _notifyIcon;
    private readonly LogService _logService;
    private SoundPlayer? _soundPlayer;
    private readonly string _soundPath;

    public bool PlaySoundEnabled { get; set; } = true;
    public bool ShowNotificationEnabled { get; set; } = true;

    public NotificationService(NotifyIcon notifyIcon, LogService logService)
    {
        _notifyIcon = notifyIcon;
        _logService = logService;
        _soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "success.wav");
        InitializeSoundPlayer();
    }

    private void InitializeSoundPlayer()
    {
        try
        {
            if (File.Exists(_soundPath))
            {
                _soundPlayer = new SoundPlayer(_soundPath);
                _soundPlayer.Load();
            }
        }
        catch (Exception ex)
        {
            _logService.LogError("", $"Erro ao carregar som: {ex.Message}");
        }
    }

    /// <summary>
    /// Exibe notificação de sucesso
    /// </summary>
    public void ShowSuccess(string message = "🧾 Gaveta aberta com sucesso!")
    {
        if (ShowNotificationEnabled)
        {
            _notifyIcon.ShowBalloonTip(2000, "GavetaHotkeyApp", message, ToolTipIcon.Info);
        }

        if (PlaySoundEnabled)
        {
            PlaySuccessSound();
        }
    }

    /// <summary>
    /// Exibe notificação de erro
    /// </summary>
    public void ShowError(string message = "❌ Falha ao abrir a gaveta")
    {
        if (ShowNotificationEnabled)
        {
            _notifyIcon.ShowBalloonTip(3000, "GavetaHotkeyApp - Erro", message, ToolTipIcon.Error);
        }

        // Som de erro do sistema
        SystemSounds.Exclamation.Play();
    }

    /// <summary>
    /// Exibe notificação informativa
    /// </summary>
    public void ShowInfo(string message)
    {
        if (ShowNotificationEnabled)
        {
            _notifyIcon.ShowBalloonTip(2000, "GavetaHotkeyApp", message, ToolTipIcon.Info);
        }
    }

    /// <summary>
    /// Reproduz som de sucesso
    /// </summary>
    private void PlaySuccessSound()
    {
        try
        {
            if (_soundPlayer != null)
            {
                _soundPlayer.Play();
            }
            else
            {
                // Som padrão do sistema se o arquivo não existir
                SystemSounds.Asterisk.Play();
            }
        }
        catch (Exception ex)
        {
            _logService.LogError("", $"Erro ao reproduzir som: {ex.Message}");
        }
    }
}
