namespace ZabbixMonitor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null!;

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
            this.rightActionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.openSettingsButton = new System.Windows.Forms.Button();
            this.leftActionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.fullscreenButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusSpring = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(245, 246, 248);
            this.topPanel.Controls.Add(this.rightActionsPanel);
            this.topPanel.Controls.Add(this.leftActionsPanel);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topPanel.Location = new System.Drawing.Point(3, 3);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1394, 38);
            this.topPanel.TabIndex = 0;
            // 
            // rightActionsPanel
            // 
            this.rightActionsPanel.AutoSize = true;
            this.rightActionsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.rightActionsPanel.Controls.Add(this.openSettingsButton);
            this.rightActionsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightActionsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.rightActionsPanel.Location = new System.Drawing.Point(1278, 0);
            this.rightActionsPanel.Name = "rightActionsPanel";
            this.rightActionsPanel.Padding = new System.Windows.Forms.Padding(0, 5, 8, 0);
            this.rightActionsPanel.Size = new System.Drawing.Size(116, 38);
            this.rightActionsPanel.TabIndex = 1;
            this.rightActionsPanel.WrapContents = false;
            // 
            // openSettingsButton
            // 
            this.openSettingsButton.Location = new System.Drawing.Point(3, 8);
            this.openSettingsButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.openSettingsButton.Name = "openSettingsButton";
            this.openSettingsButton.Size = new System.Drawing.Size(105, 26);
            this.openSettingsButton.TabIndex = 2;
            this.openSettingsButton.Text = "Настройки";
            this.openSettingsButton.UseVisualStyleBackColor = true;
            this.openSettingsButton.Click += new System.EventHandler(this.openSettingsButton_Click);
            // 
            // leftActionsPanel
            // 
            this.leftActionsPanel.AutoSize = true;
            this.leftActionsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.leftActionsPanel.Controls.Add(this.refreshButton);
            this.leftActionsPanel.Controls.Add(this.fullscreenButton);
            this.leftActionsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftActionsPanel.Location = new System.Drawing.Point(0, 0);
            this.leftActionsPanel.Name = "leftActionsPanel";
            this.leftActionsPanel.Padding = new System.Windows.Forms.Padding(8, 5, 0, 0);
            this.leftActionsPanel.Size = new System.Drawing.Size(227, 38);
            this.leftActionsPanel.TabIndex = 0;
            this.leftActionsPanel.WrapContents = false;
            // 
            // fullscreenButton
            // 
            this.fullscreenButton.Location = new System.Drawing.Point(116, 8);
            this.fullscreenButton.Margin = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.fullscreenButton.Name = "fullscreenButton";
            this.fullscreenButton.Size = new System.Drawing.Size(111, 26);
            this.fullscreenButton.TabIndex = 1;
            this.fullscreenButton.Text = "Полный экран";
            this.fullscreenButton.UseVisualStyleBackColor = true;
            this.fullscreenButton.Click += new System.EventHandler(this.fullscreenButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(11, 8);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(105, 26);
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
            this.statusSpring,
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
            // statusSpring
            // 
            this.statusSpring.Name = "statusSpring";
            this.statusSpring.Size = new System.Drawing.Size(1177, 17);
            this.statusSpring.Spring = true;
            // 
            // lastRefreshLabel
            // 
            this.lastRefreshLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
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
        private System.Windows.Forms.FlowLayoutPanel rightActionsPanel;
        private System.Windows.Forms.FlowLayoutPanel leftActionsPanel;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button fullscreenButton;
        private System.Windows.Forms.Button openSettingsButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripStatusLabel statusSpring;
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

