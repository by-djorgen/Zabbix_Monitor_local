using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZabbixMonitor.Models;
using ZabbixMonitor.Services;

namespace ZabbixMonitor;

public sealed class StartForm : Form
{
    private readonly SettingsService _settingsService;
    private readonly AppSettings _settings;

    private readonly ComboBox _urlComboBox = new();
    private readonly CheckBox _rememberAddressCheckBox = new();
    private readonly CheckBox _autoStartWithLastUrlCheckBox = new();
    private readonly CheckBox _startFullscreenCheckBox = new();
    private readonly CheckBox _autoRefreshCheckBox = new();
    private readonly NumericUpDown _refreshIntervalNumeric = new();
    private readonly CheckBox _launchMinimizedCheckBox = new();
    private readonly CheckBox _minimizeToTrayCheckBox = new();
    private readonly CheckBox _startWithWindowsCheckBox = new();
    private readonly Button _openButton = new();
    private readonly Button _deleteProfileButton = new();

    public LaunchOptions? Result { get; private set; }

    public StartForm(SettingsService settingsService, AppSettings settings)
    {
        _settingsService = settingsService;
        _settings = settings;

        InitializeForm();
        LoadSettingsToUi();
    }

    private void InitializeForm()
    {
        Text = "Zabbix Monitor - Start";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        AutoScaleMode = AutoScaleMode.Dpi;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(560, 360);

        var titleLabel = new Label
        {
            Text = "Адрес Zabbix",
            AutoSize = true,
            Location = new Point(20, 20)
        };

        _urlComboBox.Location = new Point(20, 45);
        _urlComboBox.Width = 440;
        _urlComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _urlComboBox.DropDownStyle = ComboBoxStyle.DropDown;

        _deleteProfileButton.Text = "Удалить";
        _deleteProfileButton.Location = new Point(470, 43);
        _deleteProfileButton.Size = new Size(75, 28);
        _deleteProfileButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _deleteProfileButton.Click += (_, _) => DeleteSelectedProfile();

        _rememberAddressCheckBox.Text = "Запомнить адрес";
        _rememberAddressCheckBox.Location = new Point(20, 88);
        _rememberAddressCheckBox.AutoSize = true;
        _rememberAddressCheckBox.Checked = true;

        _autoStartWithLastUrlCheckBox.Text = "Автозапуск с последним адресом";
        _autoStartWithLastUrlCheckBox.Location = new Point(20, 118);
        _autoStartWithLastUrlCheckBox.AutoSize = true;

        _startFullscreenCheckBox.Text = "Запуск в полноэкранном режиме";
        _startFullscreenCheckBox.Location = new Point(20, 148);
        _startFullscreenCheckBox.AutoSize = true;

        _autoRefreshCheckBox.Text = "Автообновление";
        _autoRefreshCheckBox.Location = new Point(20, 178);
        _autoRefreshCheckBox.AutoSize = true;
        _autoRefreshCheckBox.CheckedChanged += (_, _) => UpdateRefreshControlsState();

        var refreshLabel = new Label
        {
            Text = "Интервал (сек):",
            AutoSize = true,
            Location = new Point(180, 180)
        };

        _refreshIntervalNumeric.Location = new Point(280, 176);
        _refreshIntervalNumeric.Minimum = 10;
        _refreshIntervalNumeric.Maximum = 3600;
        _refreshIntervalNumeric.Width = 100;

        _launchMinimizedCheckBox.Text = "Запускать свернутым в трей";
        _launchMinimizedCheckBox.Location = new Point(20, 208);
        _launchMinimizedCheckBox.AutoSize = true;

        _minimizeToTrayCheckBox.Text = "Сворачивать окно в трей";
        _minimizeToTrayCheckBox.Location = new Point(20, 238);
        _minimizeToTrayCheckBox.AutoSize = true;

        _startWithWindowsCheckBox.Text = "Запускать с Windows";
        _startWithWindowsCheckBox.Location = new Point(20, 268);
        _startWithWindowsCheckBox.AutoSize = true;

        _openButton.Text = "Открыть";
        _openButton.Size = new Size(120, 34);
        _openButton.Location = new Point(425, 305);
        _openButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        _openButton.Click += (_, _) => OpenClicked();

        var cancelButton = new Button
        {
            Text = "Отмена",
            Size = new Size(120, 34),
            Location = new Point(290, 305),
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            DialogResult = DialogResult.Cancel
        };

        Controls.AddRange(
        [
            titleLabel,
            _urlComboBox,
            _deleteProfileButton,
            _rememberAddressCheckBox,
            _autoStartWithLastUrlCheckBox,
            _startFullscreenCheckBox,
            _autoRefreshCheckBox,
            refreshLabel,
            _refreshIntervalNumeric,
            _launchMinimizedCheckBox,
            _minimizeToTrayCheckBox,
            _startWithWindowsCheckBox,
            _openButton,
            cancelButton
        ]);

        AcceptButton = _openButton;
        CancelButton = cancelButton;
    }

    private void LoadSettingsToUi()
    {
        foreach (string profile in _settings.SavedProfiles.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            _urlComboBox.Items.Add(profile);
        }

        _urlComboBox.Text = !string.IsNullOrWhiteSpace(_settings.LastUrl) ? _settings.LastUrl : "https://";
        _autoStartWithLastUrlCheckBox.Checked = _settings.AutoStartWithLastUrl;
        _startFullscreenCheckBox.Checked = _settings.StartInFullscreen;
        _autoRefreshCheckBox.Checked = _settings.AutoRefreshEnabled;
        _refreshIntervalNumeric.Value = SettingsService.ClampRefreshInterval(_settings.AutoRefreshIntervalSeconds);
        _launchMinimizedCheckBox.Checked = _settings.LaunchMinimizedToTray;
        _minimizeToTrayCheckBox.Checked = _settings.MinimizeToTray;
        _startWithWindowsCheckBox.Checked = _settings.StartWithWindows;
        UpdateRefreshControlsState();
    }

    private void UpdateRefreshControlsState() => _refreshIntervalNumeric.Enabled = _autoRefreshCheckBox.Checked;

    private void DeleteSelectedProfile()
    {
        string profile = (_urlComboBox.SelectedItem as string ?? _urlComboBox.Text).Trim();
        if (string.IsNullOrWhiteSpace(profile))
        {
            return;
        }

        _settings.SavedProfiles.RemoveAll(x => string.Equals(x, profile, StringComparison.OrdinalIgnoreCase));
        _urlComboBox.Items.Remove(profile);
        if (_urlComboBox.Text.Equals(profile, StringComparison.OrdinalIgnoreCase))
        {
            _urlComboBox.Text = string.Empty;
        }

        _settingsService.Save(_settings);
    }

    private void OpenClicked()
    {
        if (!UrlValidator.TryNormalizeHttpUrl(_urlComboBox.Text, out string? normalizedUrl))
        {
            MessageBox.Show(
                "Указан некорректный URL. Введите адрес в формате http://... или https://...",
                "Некорректный URL",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        _settings.LastUrl = normalizedUrl!;
        _settings.AutoStartWithLastUrl = _autoStartWithLastUrlCheckBox.Checked;
        _settings.StartInFullscreen = _startFullscreenCheckBox.Checked;
        _settings.AutoRefreshEnabled = _autoRefreshCheckBox.Checked;
        _settings.AutoRefreshIntervalSeconds = SettingsService.ClampRefreshInterval((int)_refreshIntervalNumeric.Value);
        _settings.LaunchMinimizedToTray = _launchMinimizedCheckBox.Checked;
        _settings.MinimizeToTray = _minimizeToTrayCheckBox.Checked;
        _settings.StartWithWindows = _startWithWindowsCheckBox.Checked;

        if (_rememberAddressCheckBox.Checked &&
            !_settings.SavedProfiles.Any(x => x.Equals(normalizedUrl, StringComparison.OrdinalIgnoreCase)))
        {
            _settings.SavedProfiles.Add(normalizedUrl!);
        }

        _settingsService.Save(_settings);
        WindowsStartupService.SetEnabled(_settings.StartWithWindows);

        Result = new LaunchOptions(
            normalizedUrl!,
            _settings.StartInFullscreen,
            _settings.AutoRefreshEnabled,
            _settings.AutoRefreshIntervalSeconds,
            _settings.LaunchMinimizedToTray);

        DialogResult = DialogResult.OK;
        Close();
    }
}
