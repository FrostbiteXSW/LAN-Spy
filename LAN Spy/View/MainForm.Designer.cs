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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.HostListMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加到目标组1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加到目标组2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PoisonerTab = new System.Windows.Forms.TabPage();
            this.Target2ListContainer = new System.Windows.Forms.GroupBox();
            this.Target2List = new System.Windows.Forms.ListBox();
            this.Target1ListContainer = new System.Windows.Forms.GroupBox();
            this.Target1List = new System.Windows.Forms.ListBox();
            this.ScannerTab = new System.Windows.Forms.TabPage();
            this.HostList = new System.Windows.Forms.DataGridView();
            this.HostIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HostMAC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MainTabContainer = new System.Windows.Forms.TabControl();
            this.WatcherTab = new System.Windows.Forms.TabPage();
            this.TargetListMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制选中项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.从目标组移除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectionList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConnectionListMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制选中项ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.断开此连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始毒化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始监视ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.HostListMenuStrip.SuspendLayout();
            this.PoisonerTab.SuspendLayout();
            this.Target2ListContainer.SuspendLayout();
            this.Target1ListContainer.SuspendLayout();
            this.ScannerTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HostList)).BeginInit();
            this.MainTabContainer.SuspendLayout();
            this.WatcherTab.SuspendLayout();
            this.TargetListMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionList)).BeginInit();
            this.ConnectionListMenuStrip.SuspendLayout();
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
            this.MainMenu.Size = new System.Drawing.Size(584, 25);
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
            this.开始ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.开始ToolStripMenuItem.Text = "开始";
            // 
            // 启动所有模块ToolStripMenuItem
            // 
            this.启动所有模块ToolStripMenuItem.Name = "启动所有模块ToolStripMenuItem";
            this.启动所有模块ToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.启动所有模块ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.启动所有模块ToolStripMenuItem.Text = "启动所有模块";
            this.启动所有模块ToolStripMenuItem.Click += new System.EventHandler(this.启动所有模块ToolStripMenuItem_Click);
            // 
            // 停止所有模块ToolStripMenuItem
            // 
            this.停止所有模块ToolStripMenuItem.Name = "停止所有模块ToolStripMenuItem";
            this.停止所有模块ToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.停止所有模块ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.停止所有模块ToolStripMenuItem.Text = "停止所有模块";
            this.停止所有模块ToolStripMenuItem.Click += new System.EventHandler(this.停止所有模块ToolStripMenuItem_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.关于ToolStripMenuItem.Text = "关于...";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
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
            this.扫描ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.扫描ToolStripMenuItem.Text = "扫描";
            // 
            // 启动扫描模块ToolStripMenuItem
            // 
            this.启动扫描模块ToolStripMenuItem.Name = "启动扫描模块ToolStripMenuItem";
            this.启动扫描模块ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.启动扫描模块ToolStripMenuItem.Text = "启动模块";
            this.启动扫描模块ToolStripMenuItem.Click += new System.EventHandler(this.启动扫描模块ToolStripMenuItem_Click);
            // 
            // 扫描主机ToolStripMenuItem
            // 
            this.扫描主机ToolStripMenuItem.Enabled = false;
            this.扫描主机ToolStripMenuItem.Name = "扫描主机ToolStripMenuItem";
            this.扫描主机ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.扫描主机ToolStripMenuItem.Text = "扫描主机";
            this.扫描主机ToolStripMenuItem.Click += new System.EventHandler(this.扫描主机ToolStripMenuItem_Click);
            // 
            // 侦测主机ToolStripMenuItem
            // 
            this.侦测主机ToolStripMenuItem.Enabled = false;
            this.侦测主机ToolStripMenuItem.Name = "侦测主机ToolStripMenuItem";
            this.侦测主机ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.侦测主机ToolStripMenuItem.Text = "侦测主机";
            this.侦测主机ToolStripMenuItem.Click += new System.EventHandler(this.侦测主机ToolStripMenuItem_Click);
            // 
            // 毒化ToolStripMenuItem
            // 
            this.毒化ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动毒化模块ToolStripMenuItem,
            this.开始毒化ToolStripMenuItem});
            this.毒化ToolStripMenuItem.Name = "毒化ToolStripMenuItem";
            this.毒化ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.毒化ToolStripMenuItem.Text = "毒化";
            // 
            // 启动毒化模块ToolStripMenuItem
            // 
            this.启动毒化模块ToolStripMenuItem.Name = "启动毒化模块ToolStripMenuItem";
            this.启动毒化模块ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.启动毒化模块ToolStripMenuItem.Text = "启动模块";
            this.启动毒化模块ToolStripMenuItem.Click += new System.EventHandler(this.启动毒化模块ToolStripMenuItem_Click);
            // 
            // 监视ToolStripMenuItem
            // 
            this.监视ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动监视模块ToolStripMenuItem,
            this.开始监视ToolStripMenuItem});
            this.监视ToolStripMenuItem.Name = "监视ToolStripMenuItem";
            this.监视ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.监视ToolStripMenuItem.Text = "监视";
            // 
            // 启动监视模块ToolStripMenuItem
            // 
            this.启动监视模块ToolStripMenuItem.Name = "启动监视模块ToolStripMenuItem";
            this.启动监视模块ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.启动监视模块ToolStripMenuItem.Text = "启动模块";
            // 
            // SplitLine
            // 
            this.SplitLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SplitLine.Location = new System.Drawing.Point(0, 27);
            this.SplitLine.Name = "SplitLine";
            this.SplitLine.Size = new System.Drawing.Size(584, 2);
            this.SplitLine.TabIndex = 1;
            // 
            // HostListMenuStrip
            // 
            this.HostListMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.HostListMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制ToolStripMenuItem,
            this.添加到目标组1ToolStripMenuItem,
            this.添加到目标组2ToolStripMenuItem});
            this.HostListMenuStrip.Name = "HostListMenuStrip";
            this.HostListMenuStrip.Size = new System.Drawing.Size(156, 70);
            this.HostListMenuStrip.Opened += new System.EventHandler(this.HostListMenuStrip_Opened);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.复制ToolStripMenuItem.Text = "复制选中项";
            this.复制ToolStripMenuItem.Click += new System.EventHandler(this.复制ToolStripMenuItem_Click);
            // 
            // 添加到目标组1ToolStripMenuItem
            // 
            this.添加到目标组1ToolStripMenuItem.Name = "添加到目标组1ToolStripMenuItem";
            this.添加到目标组1ToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.添加到目标组1ToolStripMenuItem.Text = "添加到目标组1";
            // 
            // 添加到目标组2ToolStripMenuItem
            // 
            this.添加到目标组2ToolStripMenuItem.Name = "添加到目标组2ToolStripMenuItem";
            this.添加到目标组2ToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.添加到目标组2ToolStripMenuItem.Text = "添加到目标组2";
            // 
            // PoisonerTab
            // 
            this.PoisonerTab.Controls.Add(this.Target2ListContainer);
            this.PoisonerTab.Controls.Add(this.Target1ListContainer);
            this.PoisonerTab.Location = new System.Drawing.Point(4, 22);
            this.PoisonerTab.Name = "PoisonerTab";
            this.PoisonerTab.Padding = new System.Windows.Forms.Padding(3);
            this.PoisonerTab.Size = new System.Drawing.Size(576, 311);
            this.PoisonerTab.TabIndex = 1;
            this.PoisonerTab.Text = "毒化目标";
            this.PoisonerTab.UseVisualStyleBackColor = true;
            // 
            // Target2ListContainer
            // 
            this.Target2ListContainer.Controls.Add(this.Target2List);
            this.Target2ListContainer.Location = new System.Drawing.Point(290, 6);
            this.Target2ListContainer.Name = "Target2ListContainer";
            this.Target2ListContainer.Size = new System.Drawing.Size(280, 299);
            this.Target2ListContainer.TabIndex = 3;
            this.Target2ListContainer.TabStop = false;
            this.Target2ListContainer.Text = "目标组2";
            // 
            // Target2List
            // 
            this.Target2List.FormattingEnabled = true;
            this.Target2List.ItemHeight = 12;
            this.Target2List.Location = new System.Drawing.Point(6, 20);
            this.Target2List.Name = "Target2List";
            this.Target2List.Size = new System.Drawing.Size(268, 268);
            this.Target2List.TabIndex = 1;
            // 
            // Target1ListContainer
            // 
            this.Target1ListContainer.Controls.Add(this.Target1List);
            this.Target1ListContainer.Location = new System.Drawing.Point(6, 6);
            this.Target1ListContainer.Name = "Target1ListContainer";
            this.Target1ListContainer.Size = new System.Drawing.Size(280, 299);
            this.Target1ListContainer.TabIndex = 2;
            this.Target1ListContainer.TabStop = false;
            this.Target1ListContainer.Text = "目标组1";
            // 
            // Target1List
            // 
            this.Target1List.FormattingEnabled = true;
            this.Target1List.ItemHeight = 12;
            this.Target1List.Location = new System.Drawing.Point(6, 20);
            this.Target1List.Name = "Target1List";
            this.Target1List.Size = new System.Drawing.Size(268, 268);
            this.Target1List.TabIndex = 0;
            // 
            // ScannerTab
            // 
            this.ScannerTab.Controls.Add(this.HostList);
            this.ScannerTab.Location = new System.Drawing.Point(4, 22);
            this.ScannerTab.Name = "ScannerTab";
            this.ScannerTab.Padding = new System.Windows.Forms.Padding(3);
            this.ScannerTab.Size = new System.Drawing.Size(576, 311);
            this.ScannerTab.TabIndex = 0;
            this.ScannerTab.Text = "主机列表";
            this.ScannerTab.UseVisualStyleBackColor = true;
            // 
            // HostList
            // 
            this.HostList.AllowUserToAddRows = false;
            this.HostList.AllowUserToDeleteRows = false;
            this.HostList.AllowUserToResizeRows = false;
            this.HostList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.HostList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.HostList.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.HostList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.HostList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.HostList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.HostIP,
            this.HostMAC});
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.HostList.DefaultCellStyle = dataGridViewCellStyle14;
            this.HostList.Location = new System.Drawing.Point(0, 0);
            this.HostList.Name = "HostList";
            this.HostList.ReadOnly = true;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.HostList.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.HostList.RowHeadersVisible = false;
            this.HostList.RowTemplate.Height = 23;
            this.HostList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.HostList.Size = new System.Drawing.Size(576, 308);
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
            // MainTabContainer
            // 
            this.MainTabContainer.Controls.Add(this.ScannerTab);
            this.MainTabContainer.Controls.Add(this.PoisonerTab);
            this.MainTabContainer.Controls.Add(this.WatcherTab);
            this.MainTabContainer.Location = new System.Drawing.Point(0, 27);
            this.MainTabContainer.Name = "MainTabContainer";
            this.MainTabContainer.SelectedIndex = 0;
            this.MainTabContainer.Size = new System.Drawing.Size(584, 337);
            this.MainTabContainer.TabIndex = 3;
            // 
            // WatcherTab
            // 
            this.WatcherTab.Controls.Add(this.ConnectionList);
            this.WatcherTab.Location = new System.Drawing.Point(4, 22);
            this.WatcherTab.Name = "WatcherTab";
            this.WatcherTab.Padding = new System.Windows.Forms.Padding(3);
            this.WatcherTab.Size = new System.Drawing.Size(576, 311);
            this.WatcherTab.TabIndex = 2;
            this.WatcherTab.Text = "连接列表";
            this.WatcherTab.UseVisualStyleBackColor = true;
            // 
            // TargetListMenuStrip
            // 
            this.TargetListMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制选中项ToolStripMenuItem,
            this.从目标组移除ToolStripMenuItem});
            this.TargetListMenuStrip.Name = "TargetListMenuStrip";
            this.TargetListMenuStrip.Size = new System.Drawing.Size(149, 48);
            // 
            // 复制选中项ToolStripMenuItem
            // 
            this.复制选中项ToolStripMenuItem.Name = "复制选中项ToolStripMenuItem";
            this.复制选中项ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.复制选中项ToolStripMenuItem.Text = "复制选中项";
            // 
            // 从目标组移除ToolStripMenuItem
            // 
            this.从目标组移除ToolStripMenuItem.Name = "从目标组移除ToolStripMenuItem";
            this.从目标组移除ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.从目标组移除ToolStripMenuItem.Text = "从目标组移除";
            // 
            // ConnectionList
            // 
            this.ConnectionList.AllowUserToAddRows = false;
            this.ConnectionList.AllowUserToDeleteRows = false;
            this.ConnectionList.AllowUserToResizeRows = false;
            this.ConnectionList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ConnectionList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.ConnectionList.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ConnectionList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.ConnectionList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConnectionList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ConnectionList.DefaultCellStyle = dataGridViewCellStyle17;
            this.ConnectionList.Location = new System.Drawing.Point(0, 1);
            this.ConnectionList.Name = "ConnectionList";
            this.ConnectionList.ReadOnly = true;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ConnectionList.RowHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.ConnectionList.RowHeadersVisible = false;
            this.ConnectionList.RowTemplate.Height = 23;
            this.ConnectionList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ConnectionList.Size = new System.Drawing.Size(576, 308);
            this.ConnectionList.TabIndex = 3;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "源地址";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "目标地址";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // ConnectionListMenuStrip
            // 
            this.ConnectionListMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制选中项ToolStripMenuItem1,
            this.断开此连接ToolStripMenuItem});
            this.ConnectionListMenuStrip.Name = "ConnectionListMenuStrip";
            this.ConnectionListMenuStrip.Size = new System.Drawing.Size(137, 48);
            // 
            // 复制选中项ToolStripMenuItem1
            // 
            this.复制选中项ToolStripMenuItem1.Name = "复制选中项ToolStripMenuItem1";
            this.复制选中项ToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.复制选中项ToolStripMenuItem1.Text = "复制选中项";
            // 
            // 断开此连接ToolStripMenuItem
            // 
            this.断开此连接ToolStripMenuItem.Name = "断开此连接ToolStripMenuItem";
            this.断开此连接ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.断开此连接ToolStripMenuItem.Text = "断开此连接";
            // 
            // 开始毒化ToolStripMenuItem
            // 
            this.开始毒化ToolStripMenuItem.Enabled = false;
            this.开始毒化ToolStripMenuItem.Name = "开始毒化ToolStripMenuItem";
            this.开始毒化ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.开始毒化ToolStripMenuItem.Text = "开始毒化";
            // 
            // 开始监视ToolStripMenuItem
            // 
            this.开始监视ToolStripMenuItem.Enabled = false;
            this.开始监视ToolStripMenuItem.Name = "开始监视ToolStripMenuItem";
            this.开始监视ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.开始监视ToolStripMenuItem.Text = "开始监视";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.MainTabContainer);
            this.Controls.Add(this.SplitLine);
            this.Controls.Add(this.MainMenu);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.MainMenu;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "GUI for LAN Spy";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.HostListMenuStrip.ResumeLayout(false);
            this.PoisonerTab.ResumeLayout(false);
            this.Target2ListContainer.ResumeLayout(false);
            this.Target1ListContainer.ResumeLayout(false);
            this.ScannerTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HostList)).EndInit();
            this.MainTabContainer.ResumeLayout(false);
            this.WatcherTab.ResumeLayout(false);
            this.TargetListMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionList)).EndInit();
            this.ConnectionListMenuStrip.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem 启动扫描模块ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启动毒化模块ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启动监视模块ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip HostListMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加到目标组1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加到目标组2ToolStripMenuItem;
        private System.Windows.Forms.TabPage PoisonerTab;
        private System.Windows.Forms.TabPage ScannerTab;
        private System.Windows.Forms.DataGridView HostList;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostMAC;
        private System.Windows.Forms.TabControl MainTabContainer;
        private System.Windows.Forms.TabPage WatcherTab;
        private System.Windows.Forms.ListBox Target2List;
        private System.Windows.Forms.ListBox Target1List;
        private System.Windows.Forms.GroupBox Target2ListContainer;
        private System.Windows.Forms.GroupBox Target1ListContainer;
        private System.Windows.Forms.DataGridView ConnectionList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.ContextMenuStrip TargetListMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 复制选中项ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 从目标组移除ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ConnectionListMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 复制选中项ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 断开此连接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始毒化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始监视ToolStripMenuItem;
    }
}