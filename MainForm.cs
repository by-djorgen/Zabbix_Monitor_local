using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using ZabbixMonitor.Models;
using ZabbixMonitor.Services;
using FormsTimer = System.Windows.Forms.Timer;

namespace ZabbixMonitor;

public partial class MainForm : Form
{
    private readonly SettingsService _settingsService;
    private readonly AppSettings _settings;
    private NavigationPolicy _navigationPolicy;
    private readonly FormsTimer _refreshTimer = new();
    private bool _isNavigating;
    private bool _isBrowserReady;
    private bool _isFullscreen;
    private bool _isExitRequested;
    private FormBorderStyle _savedBorderStyle;
    private FormWindowState _savedWindowState;
    private Rectangle _savedBounds;
    private string _currentUrl;

    public MainForm(SettingsService settingsService, AppSettings settings, LaunchOptions launchOptions)
    {
        _settingsService = settingsService;
        _settings = settings;
        _currentUrl = launchOptions.Url;
        _navigationPolicy = BuildNavigationPolicy(_currentUrl);

        InitializeComponent();
        InitializeFormState();
        InitializeRefreshTimer(launchOptions);
        InitializeTray();
        _ = InitializeBrowserAsync(launchOptions);
    }

    private void InitializeFormState()
    {
        Text = "Zabbix Monitor";
        statusLabel.Text = "Инициализация WebView2...";
        lastRefreshLabel.Text = "Последнее обновление: -";
        SetApplicationIcon();

        _settings.LastUrl = _currentUrl;
        _settingsService.Save(_settings);
    }

    private void InitializeRefreshTimer(LaunchOptions launchOptions)
    {
        _refreshTimer.Interval = SettingsService.ClampRefreshInterval(launchOptions.AutoRefreshIntervalSeconds) * 1000;
        _refreshTimer.Tick += (_, _) => RefreshPageInternal("Auto refresh");
    }

    private void InitializeTray()
    {
        trayIcon.Icon = Icon ?? SystemIcons.Application;
        trayIcon.Visible = true;
    }

    private async System.Threading.Tasks.Task InitializeBrowserAsync(LaunchOptions launchOptions)
    {
        try
        {
            await webView.EnsureCoreWebView2Async();
            ConfigureWebView();
            _isBrowserReady = true;
            webView.CoreWebView2.Navigate(_currentUrl);
            FileLogger.Info($"Opening URL: {_currentUrl}");
            statusLabel.Text = "Загрузка страницы...";

            if (launchOptions.StartFullscreen)
            {
                EnterFullscreen();
            }

            if (launchOptions.StartMinimizedToTray)
            {
                HideToTray();
            }
        }
        catch (WebView2RuntimeNotFoundException ex)
        {
            FileLogger.Error("WebView2 Runtime is missing.", ex);
            ShowFatalError("Не найден WebView2 Runtime. Установите Microsoft Edge WebView2 Runtime и перезапустите приложение.");
        }
        catch (Exception ex)
        {
            FileLogger.Error("WebView2 initialization failed.", ex);
            ShowFatalError($"Ошибка инициализации WebView2: {ex.Message}");
        }
    }

    private void ConfigureWebView()
    {
        CoreWebView2 core = webView.CoreWebView2;
        core.NavigationStarting += CoreWebView2_NavigationStarting;
        core.NavigationCompleted += CoreWebView2_NavigationCompleted;
        core.NewWindowRequested += CoreWebView2_NewWindowRequested;
        core.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
        core.ServerCertificateErrorDetected += CoreWebView2_ServerCertificateErrorDetected;
        core.ProcessFailed += CoreWebView2_ProcessFailed;
        core.DownloadStarting += CoreWebView2_DownloadStarting;

        core.Settings.AreBrowserAcceleratorKeysEnabled = false;
        core.Settings.AreDefaultContextMenusEnabled = false;
        core.Settings.AreDefaultScriptDialogsEnabled = true;
        core.Settings.IsPasswordAutosaveEnabled = false;
        core.Settings.IsGeneralAutofillEnabled = false;
        core.Settings.IsZoomControlEnabled = false;
        core.Settings.IsStatusBarEnabled = false;
    }

    private void CoreWebView2_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
    {
        _isNavigating = true;
        statusLabel.Text = "Загрузка...";

        if (_navigationPolicy.IsAllowed(e.Uri))
        {
            return;
        }

        e.Cancel = true;
        _isNavigating = false;
        statusLabel.Text = "Переход заблокирован";
        FileLogger.Warning($"Blocked navigation attempt: {e.Uri}");
        MessageBox.Show(
            "Переход на внешний домен запрещен политикой безопасности.",
            "Навигация заблокирована",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
    }

    private void CoreWebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        _isNavigating = false;
        if (e.IsSuccess)
        {
            if (_settings.AutoRefreshEnabled && !_refreshTimer.Enabled)
            {
                _refreshTimer.Start();
            }
            statusLabel.Text = "Готово";
            return;
        }

        string message = e.WebErrorStatus switch
        {
            CoreWebView2WebErrorStatus.CannotConnect => "Не удалось подключиться к сайту. Проверьте сеть и адрес.",
            CoreWebView2WebErrorStatus.Timeout => "Превышено время ожидания ответа от сайта.",
            CoreWebView2WebErrorStatus.HostNameNotResolved => "Не удалось определить адрес хоста.",
            CoreWebView2WebErrorStatus.CertificateCommonNameIsIncorrect or
                CoreWebView2WebErrorStatus.CertificateExpired or
                CoreWebView2WebErrorStatus.CertificateIsInvalid => "Ошибка сертификата при подключении к сайту.",
            _ => $"Ошибка загрузки страницы: {e.WebErrorStatus}"
        };

        statusLabel.Text = "Ошибка загрузки";
        FileLogger.Warning($"Navigation failed: {e.WebErrorStatus}");
        MessageBox.Show(message, "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void CoreWebView2_NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
    {
        e.Handled = true;
        FileLogger.Warning($"Blocked new window: {e.Uri}");
        statusLabel.Text = "Всплывающее окно заблокировано";
    }

    private void CoreWebView2_DocumentTitleChanged(object? sender, object e)
    {
        if (webView.CoreWebView2 is null)
        {
            return;
        }

        string pageTitle = string.IsNullOrWhiteSpace(webView.CoreWebView2.DocumentTitle)
            ? "Zabbix"
            : webView.CoreWebView2.DocumentTitle;
        Text = $"Zabbix Monitor - {pageTitle}";
    }

    private void CoreWebView2_ServerCertificateErrorDetected(object? sender, CoreWebView2ServerCertificateErrorDetectedEventArgs e)
    {
        e.Action = CoreWebView2ServerCertificateErrorAction.Cancel;
        FileLogger.Warning($"Certificate error: {e.ErrorStatus} for {e.RequestUri}");
        statusLabel.Text = "Ошибка сертификата";
        MessageBox.Show(
            "Обнаружена ошибка сертификата. Загрузка страницы остановлена.",
            "Ошибка сертификата",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    private void CoreWebView2_ProcessFailed(object? sender, CoreWebView2ProcessFailedEventArgs e)
    {
        FileLogger.Error($"WebView2 process failed: {e.ProcessFailedKind}");
        statusLabel.Text = "Ошибка процесса WebView2";
    }

    private void CoreWebView2_DownloadStarting(object? sender, CoreWebView2DownloadStartingEventArgs e)
    {
        e.Cancel = true;
        FileLogger.Warning($"Blocked file download: {e.ResultFilePath}");
        statusLabel.Text = "Скачивание заблокировано";
    }

    private void RefreshPageInternal(string source)
    {
        if (!_isBrowserReady || webView.CoreWebView2 is null || _isNavigating)
        {
            return;
        }

        webView.CoreWebView2.Reload();
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        lastRefreshLabel.Text = $"Последнее обновление: {timestamp}";
        statusLabel.Text = "Обновление страницы...";
        FileLogger.Info($"{source}: page reload at {timestamp}");
    }

    private void SetApplicationIcon()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "icon.ico");
        if (!File.Exists(path))
        {
            return;
        }

        Icon = new Icon(path);
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F5 || keyData == (Keys.Control | Keys.R))
        {
            RefreshPageInternal("Manual refresh");
            return true;
        }

        if (keyData == Keys.F11)
        {
            ToggleFullscreen();
            return true;
        }

        if (keyData == Keys.Escape && _isFullscreen)
        {
            ExitFullscreen();
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void ToggleFullscreen()
    {
        if (_isFullscreen)
        {
            ExitFullscreen();
        }
        else
        {
            EnterFullscreen();
        }
    }

    private void EnterFullscreen()
    {
        if (_isFullscreen)
        {
            return;
        }

        _savedBorderStyle = FormBorderStyle;
        _savedWindowState = WindowState;
        _savedBounds = Bounds;

        FormBorderStyle = FormBorderStyle.None;
        WindowState = FormWindowState.Normal;
        Bounds = Screen.FromControl(this).Bounds;
        _isFullscreen = true;
        topPanel.Visible = false;
        statusStrip.Visible = false;
        fullscreenButton.Text = "Окно";
    }

    private void ExitFullscreen()
    {
        if (!_isFullscreen)
        {
            return;
        }

        FormBorderStyle = _savedBorderStyle;
        Bounds = _savedBounds;
        WindowState = _savedWindowState;
        _isFullscreen = false;
        topPanel.Visible = true;
        statusStrip.Visible = true;
        fullscreenButton.Text = "Полный экран";
    }

    private void HideToTray()
    {
        Hide();
        ShowInTaskbar = false;
        WindowState = FormWindowState.Minimized;
        statusLabel.Text = "Свернуто в трей";
    }

    private void RestoreFromTray()
    {
        Show();
        ShowInTaskbar = true;
        WindowState = FormWindowState.Normal;
        Activate();
    }

    private void ShowSettings()
    {
        using var settingsForm = new StartForm(_settingsService, _settings);
        if (settingsForm.ShowDialog() != DialogResult.OK || settingsForm.Result is null)
        {
            return;
        }

        _refreshTimer.Interval = SettingsService.ClampRefreshInterval(_settings.AutoRefreshIntervalSeconds) * 1000;
        if (_settings.AutoRefreshEnabled && !_refreshTimer.Enabled)
        {
            _refreshTimer.Start();
        }
        else if (!_settings.AutoRefreshEnabled && _refreshTimer.Enabled)
        {
            _refreshTimer.Stop();
        }

        if (!string.Equals(_currentUrl, settingsForm.Result.Url, StringComparison.OrdinalIgnoreCase) &&
            webView.CoreWebView2 is not null)
        {
            _currentUrl = settingsForm.Result.Url;
            _navigationPolicy = BuildNavigationPolicy(_currentUrl);
            webView.CoreWebView2.Navigate(settingsForm.Result.Url);
            FileLogger.Info($"Navigate to new URL from settings: {settingsForm.Result.Url}");
        }

        if (_settings.StartInFullscreen && !_isFullscreen)
        {
            EnterFullscreen();
        }
        else if (!_settings.StartInFullscreen && _isFullscreen)
        {
            ExitFullscreen();
        }
    }

    private void ShowFatalError(string message)
    {
        MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        _isExitRequested = true;
        Close();
    }

    private void refreshButton_Click(object sender, EventArgs e) => RefreshPageInternal("Manual button refresh");
    private void fullscreenButton_Click(object sender, EventArgs e) => ToggleFullscreen();
    private void openSettingsButton_Click(object sender, EventArgs e) => ShowSettings();
    private void trayOpenMenuItem_Click(object sender, EventArgs e) => RestoreFromTray();
    private void trayRefreshMenuItem_Click(object sender, EventArgs e) => RefreshPageInternal("Tray refresh");
    private void traySettingsMenuItem_Click(object sender, EventArgs e) => ShowSettings();
    private void trayExitMenuItem_Click(object sender, EventArgs e)
    {
        _isExitRequested = true;
        Close();
    }

    private void trayIcon_DoubleClick(object sender, EventArgs e) => RestoreFromTray();

    private void MainForm_Resize(object sender, EventArgs e)
    {
        if (_settings.MinimizeToTray && WindowState == FormWindowState.Minimized && !_isExitRequested)
        {
            HideToTray();
        }
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (!_isExitRequested && _settings.MinimizeToTray && e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            HideToTray();
            return;
        }

        trayIcon.Visible = false;
        trayIcon.Dispose();
        _refreshTimer.Stop();
        _refreshTimer.Dispose();
        UnsubscribeWebViewEvents();
        FileLogger.Info("Application shutdown.");
    }

    private NavigationPolicy BuildNavigationPolicy(string url) => new([url]);

    private void UnsubscribeWebViewEvents()
    {
        if (webView.CoreWebView2 is null)
        {
            return;
        }

        CoreWebView2 core = webView.CoreWebView2;
        core.NavigationStarting -= CoreWebView2_NavigationStarting;
        core.NavigationCompleted -= CoreWebView2_NavigationCompleted;
        core.NewWindowRequested -= CoreWebView2_NewWindowRequested;
        core.DocumentTitleChanged -= CoreWebView2_DocumentTitleChanged;
        core.ServerCertificateErrorDetected -= CoreWebView2_ServerCertificateErrorDetected;
        core.ProcessFailed -= CoreWebView2_ProcessFailed;
        core.DownloadStarting -= CoreWebView2_DownloadStarting;
    }
}

