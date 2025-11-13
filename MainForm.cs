using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace ZabbixMonitor
{
    public partial class MainForm : Form
    {
        private const int REFRESH_INTERVAL_SECONDS = 30; // Интервал обновления в секундах (30 секунд)
        
        private readonly Uri baseUri;
        private System.Windows.Forms.Timer? refreshTimer;
        private bool isNavigating = false;

        public MainForm() : this("about:blank")
        {
        }

        public MainForm(string initialUrl)
        {
            InitializeComponent();
            baseUri = CreateBaseUri(initialUrl);
            SetApplicationIcon();
            ApplyDarkTheme();
            InitializeRefreshTimer();
            InitializeBrowser();
        }

        private void SetApplicationIcon()
        {
            // Устанавливаем иконку приложения, если файл существует
            try
            {
                // Пробуем найти иконку в разных местах
                string[] possiblePaths = {
                    "icon.ico",
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "icon.ico"),
                    System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "icon.ico")
                };

                foreach (string path in possiblePaths)
                {
                    if (System.IO.File.Exists(path))
                    {
                        this.Icon = new Icon(path);
                        break;
                    }
                }
            }
            catch
            {
                // Игнорируем ошибку, если иконка не найдена
            }
        }

        private void InitializeRefreshTimer()
        {
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = REFRESH_INTERVAL_SECONDS * 1000; // Конвертируем секунды в миллисекунды
            refreshTimer.Tick += RefreshTimer_Tick;
        }

        private void RefreshTimer_Tick(object? sender, EventArgs e)
        {
            // Обновляем страницу, если не происходит навигация
            if (!isNavigating && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Reload();
            }
        }

        private void ApplyDarkTheme()
        {
            // Темная тема для формы
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;
            
            // Темный фон для WebView2
            webView.DefaultBackgroundColor = Color.FromArgb(30, 30, 30);
        }

        private async void InitializeBrowser()
        {
            try
            {
                // Инициализируем WebView2
                await webView.EnsureCoreWebView2Async();

                // Настраиваем обработчики событий
                webView.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
                webView.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
                webView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
                webView.CoreWebView2.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
                webView.CoreWebView2.ServerCertificateErrorDetected += CoreWebView2_ServerCertificateErrorDetected;

                // Настройки WebView2
                webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
                webView.CoreWebView2.Settings.IsPasswordAutosaveEnabled = false;
                webView.CoreWebView2.Settings.IsGeneralAutofillEnabled = false;

                // Загружаем сайт
                webView.CoreWebView2.Navigate(baseUri.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка инициализации браузера: {ex.Message}\n\nУбедитесь, что установлен Microsoft Edge WebView2 Runtime.",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CoreWebView2_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
            isNavigating = true;
            
            // Блокируем навигацию на другие сайты
            if (!IsUriWithinBase(e.Uri))
            {
                e.Cancel = true;
                isNavigating = false;
                MessageBox.Show(
                    "Навигация на другие сайты заблокирована.\nРазрешено открывать только Zabbix монитор.",
                    "Доступ ограничен",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void CoreWebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            isNavigating = false;
            
            // Запускаем таймер обновления после успешной загрузки страницы
            if (e.IsSuccess && refreshTimer != null && !refreshTimer.Enabled)
            {
                refreshTimer.Start();
            }
        }

        private void CoreWebView2_NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            // Блокируем открытие новых окон, но разрешаем навигацию внутри того же домена
            if (IsUriWithinBase(e.Uri))
            {
                // Разрешаем навигацию внутри того же сайта
                webView.CoreWebView2.Navigate(e.Uri);
            }
            e.Handled = true;
        }

        private void CoreWebView2_DocumentTitleChanged(object? sender, object e)
        {
            // Обновляем заголовок окна при изменении заголовка страницы
            if (webView.CoreWebView2 != null && !string.IsNullOrEmpty(webView.CoreWebView2.DocumentTitle))
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Text = $"Zabbix Monitor - {webView.CoreWebView2.DocumentTitle}";
                });
            }
        }

        private void CoreWebView2_ServerCertificateErrorDetected(object? sender, CoreWebView2ServerCertificateErrorDetectedEventArgs e)
        {
            // Игнорируем ошибки сертификатов для указанного сайта
            if (IsUriWithinBase(e.RequestUri))
            {
                e.Action = CoreWebView2ServerCertificateErrorAction.AlwaysAllow;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Останавливаем таймер при закрытии формы
            if (refreshTimer != null)
            {
                refreshTimer.Stop();
                refreshTimer.Dispose();
            }
            base.OnFormClosing(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Поддержка горячих клавиш для обновления страницы (F5 или Ctrl+R)
            if (keyData == Keys.F5 || (keyData == (Keys.Control | Keys.R)))
            {
                if (webView.CoreWebView2 != null && !isNavigating)
                {
                    webView.CoreWebView2.Reload();
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private static Uri CreateBaseUri(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
            {
                return new Uri("about:blank");
            }

            if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
            {
                string absolute = uri.AbsoluteUri;
                if (!absolute.EndsWith("/", StringComparison.Ordinal))
                {
                    absolute += "/";
                }
                return new Uri(absolute);
            }

            return uri;
        }

        private bool IsUriWithinBase(string? candidateUri)
        {
            if (string.IsNullOrWhiteSpace(candidateUri))
            {
                return false;
            }

            if (!Uri.TryCreate(candidateUri, UriKind.Absolute, out Uri? targetUri))
            {
                return false;
            }

            return targetUri.AbsoluteUri.StartsWith(baseUri.AbsoluteUri, StringComparison.OrdinalIgnoreCase);
        }
    }
}

