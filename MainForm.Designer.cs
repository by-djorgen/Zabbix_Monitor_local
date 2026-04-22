namespace ZabbixMonitor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer? components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.topPanel = new System.Windows.Forms.Panel();
            this.openSettingsButton = new System.Windows.Forms.Button();
            this.fullscreenButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lastRefreshLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trayOpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trayRefreshMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.traySettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trayExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainLayout.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView)).BeginInit();
            this.trayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.topPanel, 0, 0);
            this.mainLayout.Controls.Add(this.webView, 0, 1);
            this.mainLayout.Controls.Add(this.statusStrip, 0, 2);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 3;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.mainLayout.Size = new System.Drawing.Size(1400, 900);
            this.mainLayout.TabIndex = 0;
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.openSettingsButton);
            this.topPanel.Controls.Add(this.fullscreenButton);
            this.topPanel.Controls.Add(this.refreshButton);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topPanel.Location = new System.Drawing.Point(3, 3);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1394, 38);
            this.topPanel.TabIndex = 0;
            // 
            // openSettingsButton
            // 
            this.openSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openSettingsButton.Location = new System.Drawing.Point(1288, 6);
            this.openSettingsButton.Name = "openSettingsButton";
            this.openSettingsButton.Size = new System.Drawing.Size(98, 26);
            this.openSettingsButton.TabIndex = 2;
            this.openSettingsButton.Text = "Настройки";
            this.openSettingsButton.UseVisualStyleBackColor = true;
            this.openSettingsButton.Click += new System.EventHandler(this.openSettingsButton_Click);
            // 
            // fullscreenButton
            // 
            this.fullscreenButton.Location = new System.Drawing.Point(117, 6);
            this.fullscreenButton.Name = "fullscreenButton";
            this.fullscreenButton.Size = new System.Drawing.Size(103, 26);
            this.fullscreenButton.TabIndex = 1;
            this.fullscreenButton.Text = "Fullscreen";
            this.fullscreenButton.UseVisualStyleBackColor = true;
            this.fullscreenButton.Click += new System.EventHandler(this.fullscreenButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(9, 6);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(103, 26);
            this.refreshButton.TabIndex = 0;
            this.refreshButton.Text = "Обновить";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.lastRefreshLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 876);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1400, 24);
            this.statusStrip.TabIndex = 2;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(68, 17);
            this.statusLabel.Text = "Готово";
            // 
            // lastRefreshLabel
            // 
            this.lastRefreshLabel.Name = "lastRefreshLabel";
            this.lastRefreshLabel.Size = new System.Drawing.Size(140, 17);
            this.lastRefreshLabel.Text = "Последнее обновление: -";
            // 
            // webView
            // 
            this.webView.AllowExternalDrop = false;
            this.webView.CreationProperties = null;
            this.webView.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView.Location = new System.Drawing.Point(3, 47);
            this.webView.Name = "webView";
            this.webView.Size = new System.Drawing.Size(1394, 826);
            this.webView.TabIndex = 1;
            this.webView.ZoomFactor = 1.0D;
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayMenu;
            this.trayIcon.Text = "Zabbix Monitor";
            this.trayIcon.DoubleClick += new System.EventHandler(this.trayIcon_DoubleClick);
            // 
            // trayMenu
            // 
            this.trayMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayOpenMenuItem,
            this.trayRefreshMenuItem,
            this.traySettingsMenuItem,
            this.trayExitMenuItem});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(142, 92);
            // 
            // trayOpenMenuItem
            // 
            this.trayOpenMenuItem.Name = "trayOpenMenuItem";
            this.trayOpenMenuItem.Size = new System.Drawing.Size(141, 22);
            this.trayOpenMenuItem.Text = "Открыть";
            this.trayOpenMenuItem.Click += new System.EventHandler(this.trayOpenMenuItem_Click);
            // 
            // trayRefreshMenuItem
            // 
            this.trayRefreshMenuItem.Name = "trayRefreshMenuItem";
            this.trayRefreshMenuItem.Size = new System.Drawing.Size(141, 22);
            this.trayRefreshMenuItem.Text = "Обновить";
            this.trayRefreshMenuItem.Click += new System.EventHandler(this.trayRefreshMenuItem_Click);
            // 
            // traySettingsMenuItem
            // 
            this.traySettingsMenuItem.Name = "traySettingsMenuItem";
            this.traySettingsMenuItem.Size = new System.Drawing.Size(141, 22);
            this.traySettingsMenuItem.Text = "Настройки";
            this.traySettingsMenuItem.Click += new System.EventHandler(this.traySettingsMenuItem_Click);
            // 
            // trayExitMenuItem
            // 
            this.trayExitMenuItem.Name = "trayExitMenuItem";
            this.trayExitMenuItem.Size = new System.Drawing.Size(141, 22);
            this.trayExitMenuItem.Text = "Выход";
            this.trayExitMenuItem.Click += new System.EventHandler(this.trayExitMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1400, 900);
            this.Controls.Add(this.mainLayout);
            this.Name = "MainForm";
            this.Text = "Zabbix Monitor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.mainLayout.ResumeLayout(false);
            this.mainLayout.PerformLayout();
            this.topPanel.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView)).EndInit();
            this.trayMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button fullscreenButton;
        private System.Windows.Forms.Button openSettingsButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripStatusLabel lastRefreshLabel;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem trayOpenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trayRefreshMenuItem;
        private System.Windows.Forms.ToolStripMenuItem traySettingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trayExitMenuItem;
    }
}

