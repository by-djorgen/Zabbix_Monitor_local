using System;
using System.Threading;
using System.Windows.Forms;
using ZabbixMonitor.Models;
using ZabbixMonitor.Services;

namespace ZabbixMonitor;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += OnThreadException;
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

        FileLogger.Initialize();
        FileLogger.Info("Application started.");

        var settingsService = new SettingsService();
        var appSettings = settingsService.Load();

        if (appSettings.AutoStartWithLastUrl && UrlValidator.TryNormalizeHttpUrl(appSettings.LastUrl, out string? autoUrl))
        {
            var autoOptions = new LaunchOptions(
                autoUrl!,
                appSettings.StartInFullscreen,
                appSettings.AutoRefreshEnabled,
                appSettings.AutoRefreshIntervalSeconds,
                appSettings.LaunchMinimizedToTray);

            Application.Run(new MainForm(settingsService, appSettings, autoOptions));
            return;
        }

        using var startForm = new StartForm(settingsService, appSettings);
        if (startForm.ShowDialog() != DialogResult.OK || startForm.Result is null)
        {
            FileLogger.Info("Application closed from startup form.");
            return;
        }

        Application.Run(new MainForm(settingsService, appSettings, startForm.Result));
    }

    private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
    {
        FileLogger.Error("Unhandled UI exception.", e.Exception);
        MessageBox.Show(
            $"Произошла непредвиденная ошибка:\n{e.Exception.Message}",
            "Ошибка",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    private static void OnUnhandledException(object? sender, UnhandledExceptionEventArgs e)
    {
        Exception? ex = e.ExceptionObject as Exception;
        FileLogger.Error("Unhandled non-UI exception.", ex);
    }
}

