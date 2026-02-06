using System.Text.Json;
using GavetaHotkeyApp.Models;

namespace GavetaHotkeyApp.Services;

/// <summary>
/// Serviço de gerenciamento de configuração JSON
/// </summary>
public class ConfigService
{
    private readonly string _configPath;
    private readonly LogService _logService;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public AppConfig Config { get; private set; } = new();

    public ConfigService(LogService logService)
    {
        _logService = logService;
        var appDir = AppDomain.CurrentDomain.BaseDirectory;
        _configPath = Path.Combine(appDir, "config.json");
    }

    public void Load()
    {
        try
        {
            if (File.Exists(_configPath))
            {
                var json = File.ReadAllText(_configPath);
                Config = JsonSerializer.Deserialize<AppConfig>(json, _jsonOptions) ?? new AppConfig();
                _logService.LogInfo("Configuração carregada com sucesso");
            }
            else
            {
                Config = new AppConfig();
                Save();
                _logService.LogInfo("Arquivo de configuração criado com valores padrão");
            }
        }
        catch (Exception ex)
        {
            _logService.LogError("", $"Erro ao carregar configuração: {ex.Message}");
            Config = new AppConfig();
        }
    }

    public void Save()
    {
        try
        {
            var json = JsonSerializer.Serialize(Config, _jsonOptions);
            File.WriteAllText(_configPath, json);
            _logService.LogInfo("Configuração salva com sucesso");
        }
        catch (Exception ex)
        {
            _logService.LogError("", $"Erro ao salvar configuração: {ex.Message}");
            throw;
        }
    }

    public void Export(string path)
    {
        try
        {
            var json = JsonSerializer.Serialize(Config, _jsonOptions);
            File.WriteAllText(path, json);
            _logService.LogInfo($"Configuração exportada para: {path}");
        }
        catch (Exception ex)
        {
            _logService.LogError("", $"Erro ao exportar configuração: {ex.Message}");
            throw;
        }
    }

    public void Import(string path)
    {
        try
        {
            var json = File.ReadAllText(path);
            var importedConfig = JsonSerializer.Deserialize<AppConfig>(json, _jsonOptions);
            if (importedConfig != null)
            {
                Config = importedConfig;
                Save();
                _logService.LogInfo($"Configuração importada de: {path}");
            }
        }
        catch (Exception ex)
        {
            _logService.LogError("", $"Erro ao importar configuração: {ex.Message}");
            throw;
        }
    }
}
