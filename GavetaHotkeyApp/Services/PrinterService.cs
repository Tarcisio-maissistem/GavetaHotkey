using System.Drawing.Printing;
using System.Runtime.InteropServices;
using GavetaHotkeyApp.Native;

namespace GavetaHotkeyApp.Services;

/// <summary>
/// Serviço para comunicação com impressoras ESC/POS
/// </summary>
public class PrinterService
{
    private readonly LogService _logService;

    // Comando ESC/POS para abrir gaveta: ESC p 0 25 250
    private static readonly byte[] OpenDrawerCommand = { 27, 112, 0, 25, 250 };

    public PrinterService(LogService logService)
    {
        _logService = logService;
    }

    /// <summary>
    /// Lista todas as impressoras instaladas no Windows
    /// </summary>
    public List<string> GetInstalledPrinters()
    {
        var printers = new List<string>();
        foreach (string printer in PrinterSettings.InstalledPrinters)
        {
            printers.Add(printer);
        }
        return printers;
    }

    /// <summary>
    /// Verifica se uma impressora existe no sistema
    /// </summary>
    public bool PrinterExists(string printerName)
    {
        if (string.IsNullOrWhiteSpace(printerName))
            return false;

        return GetInstalledPrinters().Contains(printerName, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Envia comando ESC/POS para abrir a gaveta
    /// </summary>
    public bool OpenDrawer(string printerName)
    {
        if (string.IsNullOrWhiteSpace(printerName))
        {
            _logService.LogError(printerName, "Nome da impressora não especificado");
            return false;
        }

        if (!PrinterExists(printerName))
        {
            _logService.LogError(printerName, "Impressora não encontrada no sistema");
            return false;
        }

        try
        {
            return SendRawDataToPrinter(printerName, OpenDrawerCommand);
        }
        catch (Exception ex)
        {
            _logService.LogError(printerName, $"Erro ao abrir gaveta: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Envia dados raw para a impressora usando WinAPI
    /// </summary>
    private bool SendRawDataToPrinter(string printerName, byte[] data)
    {
        IntPtr hPrinter = IntPtr.Zero;
        bool success = false;

        try
        {
            // Abre a impressora
            if (!NativeMethods.OpenPrinter(printerName, out hPrinter, IntPtr.Zero))
            {
                _logService.LogError(printerName, $"Não foi possível abrir a impressora. Erro: {Marshal.GetLastWin32Error()}");
                return false;
            }

            // Configura o documento
            var docInfo = new NativeMethods.DOCINFO
            {
                pDocName = "GavetaHotkeyApp - Abrir Gaveta",
                pOutputFile = null,
                pDatatype = "RAW"
            };

            // Inicia o documento
            if (!NativeMethods.StartDocPrinter(hPrinter, 1, ref docInfo))
            {
                _logService.LogError(printerName, $"Não foi possível iniciar documento. Erro: {Marshal.GetLastWin32Error()}");
                return false;
            }

            try
            {
                // Inicia a página
                if (!NativeMethods.StartPagePrinter(hPrinter))
                {
                    _logService.LogError(printerName, $"Não foi possível iniciar página. Erro: {Marshal.GetLastWin32Error()}");
                    return false;
                }

                try
                {
                    // Aloca memória não gerenciada para os dados
                    IntPtr pBytes = Marshal.AllocHGlobal(data.Length);
                    try
                    {
                        Marshal.Copy(data, 0, pBytes, data.Length);

                        // Envia os dados
                        if (NativeMethods.WritePrinter(hPrinter, pBytes, data.Length, out int written))
                        {
                            success = (written == data.Length);
                            if (success)
                            {
                                _logService.LogSuccess(printerName, "Gaveta aberta com sucesso");
                            }
                            else
                            {
                                _logService.LogError(printerName, $"Dados parcialmente enviados: {written}/{data.Length} bytes");
                            }
                        }
                        else
                        {
                            _logService.LogError(printerName, $"Falha ao enviar dados. Erro: {Marshal.GetLastWin32Error()}");
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(pBytes);
                    }
                }
                finally
                {
                    NativeMethods.EndPagePrinter(hPrinter);
                }
            }
            finally
            {
                NativeMethods.EndDocPrinter(hPrinter);
            }
        }
        finally
        {
            if (hPrinter != IntPtr.Zero)
            {
                NativeMethods.ClosePrinter(hPrinter);
            }
        }

        return success;
    }
}
