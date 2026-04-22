using System.Collections.Generic;

namespace ZabbixMonitor.Models;

public sealed class AppSettings
{
    public string LastUrl { get; set; } = string.Empty;
    public List<string> SavedProfiles { get; set; } = new();
    public bool AutoStartWithLastUrl { get; set; }
    public bool StartInFullscreen { get; set; } = true;
    public bool AutoRefreshEnabled { get; set; }
    public int AutoRefreshIntervalSeconds { get; set; } = 60;
    public bool LaunchMinimizedToTray { get; set; }
    public bool MinimizeToTray { get; set; } = true;
    public bool StartWithWindows { get; set; }
}
