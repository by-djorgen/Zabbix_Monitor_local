using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZabbixMonitor.Models;

namespace ZabbixMonitor.Services;

public sealed class SettingsService
{
    private readonly string _settingsPath;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public SettingsService()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string appDirectory = Path.Combine(appDataPath, "ZabbixMonitor");
        Directory.CreateDirectory(appDirectory);
        _settingsPath = Path.Combine(appDirectory, "settings.json");
    }

    public AppSettings Load()
    {
        try
        {
            if (!File.Exists(_settingsPath))
            {
                return new AppSettings();
            }

            string json = File.ReadAllText(_settingsPath);
            AppSettings? settings = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions);
            if (settings is null)
            {
                return new AppSettings();
            }

            return Normalize(settings);
        }
        catch (Exception ex)
        {
            FileLogger.Error("Failed to load settings. Defaults will be used.", ex);
            return new AppSettings();
        }
    }

    public void Save(AppSettings settings)
    {
        try
        {
            AppSettings normalized = Normalize(settings);
            string json = JsonSerializer.Serialize(normalized, JsonOptions);
            File.WriteAllText(_settingsPath, json);
        }
        catch (Exception ex)
        {
            FileLogger.Error("Failed to save settings.", ex);
        }
    }

    public static int ClampRefreshInterval(int value) => Math.Clamp(value, 10, 3600);

    private static AppSettings Normalize(AppSettings settings)
    {
        settings.LastUrl ??= string.Empty;
        settings.SavedProfiles ??= new List<string>();

        settings.SavedProfiles = settings.SavedProfiles
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => UrlValidator.TryNormalizeHttpUrl(x, out string? normalized) ? normalized : null)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        settings.AutoRefreshIntervalSeconds = ClampRefreshInterval(settings.AutoRefreshIntervalSeconds);

        if (!string.IsNullOrWhiteSpace(settings.LastUrl) &&
            UrlValidator.TryNormalizeHttpUrl(settings.LastUrl, out string? normalizedLastUrl))
        {
            settings.LastUrl = normalizedLastUrl!;
        }
        else
        {
            settings.LastUrl = string.Empty;
        }

        if (settings.AutoStartWithLastUrl && !UrlValidator.TryNormalizeHttpUrl(settings.LastUrl, out _))
        {
            settings.AutoStartWithLastUrl = false;
        }

        return settings;
    }
}
