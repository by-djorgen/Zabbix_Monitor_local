namespace ZabbixMonitor.Models;

public sealed record LaunchOptions(
    string Url,
    bool StartFullscreen,
    bool AutoRefreshEnabled,
    int AutoRefreshIntervalSeconds,
    bool StartMinimizedToTray);
