using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace ZabbixMonitor
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string? initialUrl = PromptForUrl();
            if (string.IsNullOrWhiteSpace(initialUrl))
            {
                return;
            }

            Application.Run(new MainForm(initialUrl));
        }

        private static string? PromptForUrl()
        {
            const string defaultUrl = "https://";

            while (true)
            {
                string input = Interaction.InputBox(
                    "Введите адрес Zabbix, который нужно открыть при запуске:",
                    "Адрес Zabbix",
                    defaultUrl);

                if (string.IsNullOrWhiteSpace(input))
                {
                    return null;
                }

                input = input.Trim();

                if (Uri.TryCreate(input, UriKind.Absolute, out Uri? uri) &&
                    (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                {
                    return uri.ToString();
                }

                MessageBox.Show(
                    "Указан некорректный URL. Пожалуйста, введите полный адрес, начинающийся с http:// или https://.",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
    }
}

