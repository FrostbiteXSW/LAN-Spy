namespace LAN_Spy.View {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.开始ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启动所有模块ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止所有模块ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.扫描ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启动扫描模块ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.扫描主机ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.侦测主机ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.毒化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启动毒化模块ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.监视ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启动监视模块ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SplitLine = new System.Windows.Forms.Label();
            this.HostList = new System.Windows.Forms.DataGridView();
            this.HostIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HostMAC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HostListMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加到目标组1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加到目标组2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HostList)).BeginInit();
            this.HostListMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始ToolStripMenuItem,
            this.扫描ToolStripMenuItem,
            this.毒化ToolStripMenuItem,
            this.监视ToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.MainMenu.Size = new System.Drawing.Size(876, 34);
            this.MainMenu.TabIndex = 0;
            // 
            // 开始ToolStripMenuItem
            // 
            this.开始ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动所有模块ToolStripMenuItem,
            this.停止所有模块ToolStripMenuItem,
            this.关于ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.开始ToolStripMenuItem.Name = "开始ToolStripMenuItem";
            this.开始ToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
            this.开始ToolStripMenuItem.Text = "开始";
            // 
            // 启动所有模块ToolStripMenuItem
            // 
            this.启动所有模块ToolStripMenuItem.Name = "启动所有模块ToolStripMenuItem";
            this.启动所有模块ToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.启动所有模块ToolStripMenuItem.Size = new System.Drawing.Size(200, 30);
            this.启动所有模块ToolStripMenuItem.Text = "启动所有模块";
            this.启动所有模块ToolStripMenuItem.Click += new System.EventHandler(this.启动所有模块ToolStripMenuItem_Click);
            // 
            // 停止所有模块ToolStripMenuItem
            // 
            this.停止所有模块ToolStripMenuItem.Name = "停止所有模块ToolStripMenuItem";
            this.停止所有模块ToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.停止所有模块ToolStripMenuItem.Size = new System.Drawing.Size(200, 30);
            this.停止所有模块ToolStripMenuItem.Text = "停止所有模块";
            this.停止所有模块ToolStripMenuItem.Click += new System.EventHandler(this.停止所有模块ToolStripMenuItem_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(200, 30);
            this.关于ToolStripMenuItem.Text = "关于...";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(200, 30);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 扫描ToolStripMenuItem
            // 
            this.扫描ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动扫描模块ToolStripMenuItem,
            this.扫描主机ToolStripMenuItem,
            this.侦测主机ToolStripMenuItem});
            this.扫描ToolStripMenuItem.Name = "扫描ToolStripMenuItem";
            this.扫描ToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
            this.扫描ToolStripMenuItem.Text = "扫描";
            // 
            // 启动扫描模块ToolStripMenuItem
            // 
            this.启动扫描模块ToolStripMenuItem.Name = "启动扫描模块ToolStripMenuItem";
            this.启动扫描模块ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.启动扫描模块ToolStripMenuItem.Text = "启动模块";
            this.启动扫描模块ToolStripMenuItem.Click += new System.EventHandler(this.启动扫描模块ToolStripMenuItem_Click);
            // 
            // 扫描主机ToolStripMenuItem
            // 
            this.扫描主机ToolStripMenuItem.Enabled = false;
            this.扫描主机ToolStripMenuItem.Name = "扫描主机ToolStripMenuItem";
            this.扫描主机ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.扫描主机ToolStripMenuItem.Text = "扫描主机";
            this.扫描主机ToolStripMenuItem.Click += new System.EventHandler(this.扫描主机ToolStripMenuItem_Click);
            // 
            // 侦测主机ToolStripMenuItem
            // 
            this.侦测主机ToolStripMenuItem.Enabled = false;
            this.侦测主机ToolStripMenuItem.Name = "侦测主机ToolStripMenuItem";
            this.侦测主机ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.侦测主机ToolStripMenuItem.Text = "侦测主机";
            this.侦测主机ToolStripMenuItem.Click += new System.EventHandler(this.侦测主机ToolStripMenuItem_Click);
            // 
            // 毒化ToolStripMenuItem
            // 
            this.毒化ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动毒化模块ToolStripMenuItem});
            this.毒化ToolStripMenuItem.Name = "毒化ToolStripMenuItem";
            this.毒化ToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
            this.毒化ToolStripMenuItem.Text = "毒化";
            // 
            // 启动毒化模块ToolStripMenuItem
            // 
            this.启动毒化模块ToolStripMenuItem.Name = "启动毒化模块ToolStripMenuItem";
            this.启动毒化模块ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.启动毒化模块ToolStripMenuItem.Text = "启动模块";
            this.启动毒化模块ToolStripMenuItem.Click += new System.EventHandler(this.启动毒化模块ToolStripMenuItem_Click);
            // 
            // 监视ToolStripMenuItem
            // 
            this.监视ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动监视模块ToolStripMenuItem});
            this.监视ToolStripMenuItem.Name = "监视ToolStripMenuItem";
            this.监视ToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
            this.监视ToolStripMenuItem.Text = "监视";
            // 
            // 启动监视模块ToolStripMenuItem
            // 
            this.启动监视模块ToolStripMenuItem.Name = "启动监视模块ToolStripMenuItem";
            this.启动监视模块ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.启动监视模块ToolStripMenuItem.Text = "启动模块";
            // 
            // SplitLine
            // 
            this.SplitLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SplitLine.Location = new System.Drawing.Point(0, 40);
            this.SplitLine.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SplitLine.Name = "SplitLine";
            this.SplitLine.Size = new System.Drawing.Size(876, 3);
            this.SplitLine.TabIndex = 1;
            // 
            // HostList
            // 
            this.HostList.AllowUserToAddRows = false;
            this.HostList.AllowUserToDeleteRows = false;
            this.HostList.AllowUserToResizeRows = false;
            this.HostList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.HostList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.HostList.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.HostList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.HostList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.HostList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.HostIP,
            this.HostMAC});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.HostList.DefaultCellStyle = dataGridViewCellStyle5;
            this.HostList.Location = new System.Drawing.Point(18, 48);
            this.HostList.Margin = new System.Windows.Forms.Padding(4);
            this.HostList.Name = "HostList";
            this.HostList.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.HostList.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.HostList.RowHeadersVisible = false;
            this.HostList.RowTemplate.Height = 23;
            this.HostList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.HostList.Size = new System.Drawing.Size(840, 225);
            this.HostList.TabIndex = 2;
            // 
            // HostIP
            // 
            this.HostIP.HeaderText = "主机IP";
            this.HostIP.Name = "HostIP";
            this.HostIP.ReadOnly = true;
            // 
            // HostMAC
            // 
            this.HostMAC.HeaderText = "主机MAC";
            this.HostMAC.Name = "HostMAC";
            this.HostMAC.ReadOnly = true;
            // 
            // HostListMenuStrip
            // 
            this.HostListMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.HostListMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制ToolStripMenuItem,
            this.添加到目标组1ToolStripMenuItem,
            this.添加到目标组2ToolStripMenuItem});
            this.HostListMenuStrip.Name = "HostListMenuStrip";
            this.HostListMenuStrip.Size = new System.Drawing.Size(241, 121);
            this.HostListMenuStrip.Opened += new System.EventHandler(this.HostListMenuStrip_Opened);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(199, 28);
            this.复制ToolStripMenuItem.Text = "复制选中项";
            this.复制ToolStripMenuItem.Click += new System.EventHandler(this.复制ToolStripMenuItem_Click);
            // 
            // 添加到目标组1ToolStripMenuItem
            // 
            this.添加到目标组1ToolStripMenuItem.Name = "添加到目标组1ToolStripMenuItem";
            this.添加到目标组1ToolStripMenuItem.Size = new System.Drawing.Size(199, 28);
            this.添加到目标组1ToolStripMenuItem.Text = "添加到目标组1";
            // 
            // 添加到目标组2ToolStripMenuItem
            // 
            this.添加到目标组2ToolStripMenuItem.Name = "添加到目标组2ToolStripMenuItem";
            this.添加到目标组2ToolStripMenuItem.Size = new System.Drawing.Size(199, 28);
            this.添加到目标组2ToolStripMenuItem.Text = "添加到目标组2";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 542);
            this.Controls.Add(this.HostList);
            this.Controls.Add(this.SplitLine);
            this.Controls.Add(this.MainMenu);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.MainMenu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "GUI for LAN Spy";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HostList)).EndInit();
            this.HostListMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem 开始ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启动所有模块ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止所有模块ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 扫描ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 毒化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 监视ToolStripMenuItem;
        private System.Windows.Forms.Label SplitLine;
        private System.Windows.Forms.ToolStripMenuItem 扫描主机ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 侦测主机ToolStripMenuItem;
        private System.Windows.Forms.DataGridView HostList;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostMAC;
        private System.Windows.Forms.ToolStripMenuItem 启动扫描模块ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启动毒化模块ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启动监视模块ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip HostListMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加到目标组1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加到目标组2ToolStripMenuItem;
    }
}