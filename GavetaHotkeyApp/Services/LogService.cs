namespace GavetaHotkeyApp.Services;

/// <summary>
/// Serviço de logging para arquivo
/// </summary>
public class LogService
{
    private readonly string _logPath;
    private readonly object _lock = new();

    public LogService()
    {
        var appDir = AppDomain.CurrentDomain.BaseDirectory;
        _logPath = Path.Combine(appDir, "logs.txt");
    }

    public void Log(string message, string printerName = "", string status = "INFO")
    {
        try
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var logEntry = string.IsNullOrEmpty(printerName)
                ? $"[{timestamp}] [{status}] {message}"
                : $"[{timestamp}] [{printerName}] [{status}] {message}";

            lock (_lock)
            {
                File.AppendAllText(_logPath, logEntry + Environment.NewLine);
            }
        }
        catch
        {
            // Silently fail logging
        }
    }

    public void LogSuccess(string printerName, string action)
    {
        Log(action, printerName, "SUCCESS");
    }

    public void LogError(string printerName, string error)
    {
        Log(error, printerName, "ERROR");
    }

    public void LogInfo(string message)
    {
        Log(message, "", "INFO");
    }
}
