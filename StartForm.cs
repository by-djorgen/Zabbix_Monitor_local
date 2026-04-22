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
    private readonly Button _cancelButton = new();
    private readonly Label _refreshLabel = new();

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
        Text = "Zabbix Monitor";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        AutoScaleMode = AutoScaleMode.Dpi;
        MaximizeBox = false;
        MinimizeBox = false;
        Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        ClientSize = new Size(720, 560);
        MinimumSize = new Size(720, 560);
        AutoScroll = true;
        Padding = new Padding(12);

        var rootLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5
        };
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var profileGroup = new GroupBox
        {
            Dock = DockStyle.Fill,
            Text = "URL и профили",
            Padding = new Padding(10, 12, 10, 10)
        };
        var profileLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 3
        };
        profileLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        profileLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        profileLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        profileLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        profileLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var urlLabel = new Label
        {
            Text = "Адрес Zabbix:",
            AutoSize = true,
            Anchor = AnchorStyles.Left
        };
        profileLayout.SetColumnSpan(urlLabel, 2);

        _urlComboBox.Dock = DockStyle.Fill;
        _urlComboBox.DropDownStyle = ComboBoxStyle.DropDown;
        _urlComboBox.TabIndex = 0;

        _deleteProfileButton.Text = "Удалить";
        _deleteProfileButton.Dock = DockStyle.Fill;
        _deleteProfileButton.TabIndex = 1;
        _deleteProfileButton.Click += (_, _) => DeleteSelectedProfile();

        _rememberAddressCheckBox.Text = "Запомнить адрес в профилях";
        _rememberAddressCheckBox.AutoSize = true;
        _rememberAddressCheckBox.Dock = DockStyle.Left;
        _rememberAddressCheckBox.Checked = true;
        _rememberAddressCheckBox.TabIndex = 2;

        profileLayout.Controls.Add(urlLabel, 0, 0);
        profileLayout.Controls.Add(_urlComboBox, 0, 1);
        profileLayout.Controls.Add(_deleteProfileButton, 1, 1);
        profileLayout.Controls.Add(_rememberAddressCheckBox, 0, 2);
        profileGroup.Controls.Add(profileLayout);

        var startupGroup = new GroupBox
        {
            Dock = DockStyle.Fill,
            Text = "Режим запуска",
            Padding = new Padding(10, 10, 10, 8)
        };
        var startupLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5
        };
        for (int i = 0; i < 5; i++)
        {
            startupLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }

        _autoStartWithLastUrlCheckBox.Text = "Автозапуск с последним адресом";
        _autoStartWithLastUrlCheckBox.AutoSize = true;
        _autoStartWithLastUrlCheckBox.TabIndex = 3;

        _startFullscreenCheckBox.Text = "Открывать в полноэкранном режиме";
        _startFullscreenCheckBox.AutoSize = true;
        _startFullscreenCheckBox.TabIndex = 4;

        _launchMinimizedCheckBox.Text = "Запускать свернутым в трей";
        _launchMinimizedCheckBox.AutoSize = true;
        _launchMinimizedCheckBox.TabIndex = 5;

        _minimizeToTrayCheckBox.Text = "Сворачивать окно в трей";
        _minimizeToTrayCheckBox.AutoSize = true;
        _minimizeToTrayCheckBox.TabIndex = 6;

        _startWithWindowsCheckBox.Text = "Запускать вместе с Windows";
        _startWithWindowsCheckBox.AutoSize = true;
        _startWithWindowsCheckBox.TabIndex = 7;

        startupLayout.Controls.Add(_autoStartWithLastUrlCheckBox, 0, 0);
        startupLayout.Controls.Add(_startFullscreenCheckBox, 0, 1);
        startupLayout.Controls.Add(_launchMinimizedCheckBox, 0, 2);
        startupLayout.Controls.Add(_minimizeToTrayCheckBox, 0, 3);
        startupLayout.Controls.Add(_startWithWindowsCheckBox, 0, 4);
        startupGroup.Controls.Add(startupLayout);

        var refreshGroup = new GroupBox
        {
            Dock = DockStyle.Fill,
            Text = "Обновление",
            Padding = new Padding(10, 10, 10, 8)
        };
        var refreshLayout = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            WrapContents = false,
            AutoSize = false,
            FlowDirection = FlowDirection.LeftToRight
        };

        _autoRefreshCheckBox.Text = "Автообновление";
        _autoRefreshCheckBox.AutoSize = true;
        _autoRefreshCheckBox.Margin = new Padding(0, 6, 16, 0);
        _autoRefreshCheckBox.TabIndex = 8;
        _autoRefreshCheckBox.CheckedChanged += (_, _) => UpdateRefreshControlsState();

        _refreshLabel.Text = "Интервал (сек):";
        _refreshLabel.AutoSize = true;
        _refreshLabel.Margin = new Padding(0, 6, 8, 0);

        _refreshIntervalNumeric.Minimum = 10;
        _refreshIntervalNumeric.Maximum = 3600;
        _refreshIntervalNumeric.Width = 90;
        _refreshIntervalNumeric.TabIndex = 9;

        refreshLayout.Controls.Add(_autoRefreshCheckBox);
        refreshLayout.Controls.Add(_refreshLabel);
        refreshLayout.Controls.Add(_refreshIntervalNumeric);
        refreshGroup.Controls.Add(refreshLayout);

        _openButton.Text = "Открыть";
        _openButton.Size = new Size(140, 32);
        _openButton.BackColor = Color.FromArgb(33, 115, 70);
        _openButton.ForeColor = Color.White;
        _openButton.FlatStyle = FlatStyle.Flat;
        _openButton.FlatAppearance.BorderSize = 0;
        _openButton.TabIndex = 10;
        _openButton.Click += (_, _) => OpenClicked();

        _cancelButton.Text = "Отмена";
        _cancelButton.Size = new Size(120, 32);
        _cancelButton.DialogResult = DialogResult.Cancel;
        _cancelButton.TabIndex = 11;

        var actionsLayout = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(0, 6, 0, 0)
        };
        actionsLayout.Controls.Add(_openButton);
        actionsLayout.Controls.Add(_cancelButton);

        rootLayout.Controls.Add(profileGroup, 0, 0);
        rootLayout.Controls.Add(startupGroup, 0, 1);
        rootLayout.Controls.Add(refreshGroup, 0, 2);
        rootLayout.Controls.Add(new Panel { Dock = DockStyle.Fill, MinimumSize = new Size(1, 8) }, 0, 3);
        rootLayout.Controls.Add(actionsLayout, 0, 4);
        Controls.Add(rootLayout);

        AcceptButton = _openButton;
        CancelButton = _cancelButton;
        Shown += (_, _) => _urlComboBox.Focus();
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

    private void UpdateRefreshControlsState()
    {
        bool enabled = _autoRefreshCheckBox.Checked;
        _refreshIntervalNumeric.Enabled = enabled;
        _refreshLabel.Enabled = enabled;
    }

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
                "Введите корректный адрес в формате http://... или https://...",
                "Некорректный адрес",
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
