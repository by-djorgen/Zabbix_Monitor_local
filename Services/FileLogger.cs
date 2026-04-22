using System;
using System.IO;

namespace ZabbixMonitor.Services;

public static class FileLogger
{
    private static readonly object Sync = new();
    private static string _logDirectory = string.Empty;
    private static string _logPath = string.Empty;

    public static void Initialize()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _logDirectory = Path.Combine(appDataPath, "ZabbixMonitor", "logs");
        Directory.CreateDirectory(_logDirectory);
        _logPath = Path.Combine(_logDirectory, $"zabbix-monitor-{DateTime.Now:yyyyMMdd}.log");
    }

    public static void Info(string message) => Write("INFO", message);
    public static void Warning(string message) => Write("WARN", message);
    public static void Error(string message, Exception? exception = null) =>
        Write("ERROR", exception is null ? message : $"{message} | {exception}");

    private static void Write(string level, string message)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_logPath))
            {
                Initialize();
            }

            string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}{Environment.NewLine}";
            lock (Sync)
            {
                File.AppendAllText(_logPath, line);
            }
        }
        catch
        {
            // Avoid hard crash because of logging failures.
        }
    }
}
