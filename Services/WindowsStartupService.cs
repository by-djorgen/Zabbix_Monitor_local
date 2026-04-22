using Microsoft.Win32;

namespace ZabbixMonitor.Services;

public static class WindowsStartupService
{
    private const string RunRegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string AppName = "ZabbixMonitor";

    public static void SetEnabled(bool enabled)
    {
        try
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunRegistryPath, writable: true);
            if (key is null)
            {
                FileLogger.Warning("Failed to open Run registry key.");
                return;
            }

            if (enabled)
            {
                string executablePath = System.Windows.Forms.Application.ExecutablePath;
                key.SetValue(AppName, $"\"{executablePath}\"");
                FileLogger.Info("Windows autostart enabled.");
            }
            else
            {
                key.DeleteValue(AppName, throwOnMissingValue: false);
                FileLogger.Info("Windows autostart disabled.");
            }
        }
        catch (Exception ex)
        {
            FileLogger.Error("Failed to update Windows autostart.", ex);
        }
    }
}
