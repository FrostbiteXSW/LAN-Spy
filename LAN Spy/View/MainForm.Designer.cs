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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle38 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle39 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle40 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle41 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle42 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle43 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle44 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle45 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle46 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle47 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle48 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.开始毒化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.监视ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启动监视模块ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始监视ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SplitLine = new System.Windows.Forms.Label();
            this.HostListMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.主机列表复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加到目标组1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加到目标组2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PoisonerTab = new System.Windows.Forms.TabPage();
            this.Target2ListContainer = new System.Windows.Forms.GroupBox();
            this.Target1ListContainer = new System.Windows.Forms.GroupBox();
            this.ScannerTab = new System.Windows.Forms.TabPage();
            this.HostList = new System.Windows.Forms.DataGridView();
            this.HostIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HostMAC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MainTabContainer = new System.Windows.Forms.TabControl();
            this.WatcherTab = new System.Windows.Forms.TabPage();
            this.ConnectionList = new System.Windows.Forms.DataGridView();
            this.SrcAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DstAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetListMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.毒化目标复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.从目标组移除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectionListMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.连接列表复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.断开此连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Target1List = new System.Windows.Forms.DataGridView();
            this.Target2List = new System.Windows.Forms.DataGridView();
            this.Target1IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Target1MAC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Target2IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Target2MAC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MainMenu.SuspendLayout();
            this.HostListMenuStrip.SuspendLayout();
            this.PoisonerTab.SuspendLayout();
            this.Target2ListContainer.SuspendLayout();
            this.Target1ListContainer.SuspendLayout();
            this.ScannerTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HostList)).BeginInit();
            this.MainTabContainer.SuspendLayout();
            this.WatcherTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionList)).BeginInit();
            this.TargetListMenuStrip.SuspendLayout();
            this.ConnectionListMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Target1List)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Target2List)).BeginInit();
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
            this.启动毒化模块ToolStripMenuItem,
            this.开始毒化ToolStripMenuItem});
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
            // 开始毒化ToolStripMenuItem
            // 
            this.开始毒化ToolStripMenuItem.Enabled = false;
            this.开始毒化ToolStripMenuItem.Name = "开始毒化ToolStripMenuItem";
            this.开始毒化ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.开始毒化ToolStripMenuItem.Text = "开始毒化";
            this.开始毒化ToolStripMenuItem.EnabledChanged += new System.EventHandler(this.开始毒化ToolStripMenuItem_EnabledChanged);
            // 
            // 监视ToolStripMenuItem
            // 
            this.监视ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动监视模块ToolStripMenuItem,
            this.开始监视ToolStripMenuItem});
            this.监视ToolStripMenuItem.Name = "监视ToolStripMenuItem";
            this.监视ToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
            this.监视ToolStripMenuItem.Text = "监视";
            // 
            // 启动监视模块ToolStripMenuItem
            // 
            this.启动监视模块ToolStripMenuItem.Name = "启动监视模块ToolStripMenuItem";
            this.启动监视模块ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.启动监视模块ToolStripMenuItem.Text = "启动模块";
            this.启动监视模块ToolStripMenuItem.Click += new System.EventHandler(this.启动监视模块ToolStripMenuItem_Click);
            // 
            // 开始监视ToolStripMenuItem
            // 
            this.开始监视ToolStripMenuItem.Enabled = false;
            this.开始监视ToolStripMenuItem.Name = "开始监视ToolStripMenuItem";
            this.开始监视ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.开始监视ToolStripMenuItem.Text = "开始监视";
            this.开始监视ToolStripMenuItem.EnabledChanged += new System.EventHandler(this.开始监视ToolStripMenuItem_EnabledChanged);
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
            // HostListMenuStrip
            // 
            this.HostListMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.HostListMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.主机列表复制ToolStripMenuItem,
            this.添加到目标组1ToolStripMenuItem,
            this.添加到目标组2ToolStripMenuItem});
            this.HostListMenuStrip.Name = "HostListMenuStrip";
            this.HostListMenuStrip.Size = new System.Drawing.Size(200, 88);
            this.HostListMenuStrip.Opened += new System.EventHandler(this.HostListMenuStrip_Opened);
            // 
            // 主机列表复制ToolStripMenuItem
            // 
            this.主机列表复制ToolStripMenuItem.Name = "主机列表复制ToolStripMenuItem";
            this.主机列表复制ToolStripMenuItem.Size = new System.Drawing.Size(199, 28);
            this.主机列表复制ToolStripMenuItem.Text = "复制选中项";
            this.主机列表复制ToolStripMenuItem.Click += new System.EventHandler(this.DataGridView复制ToolStripMenuItem_Click);
            // 
            // 添加到目标组1ToolStripMenuItem
            // 
            this.添加到目标组1ToolStripMenuItem.Enabled = false;
            this.添加到目标组1ToolStripMenuItem.Name = "添加到目标组1ToolStripMenuItem";
            this.添加到目标组1ToolStripMenuItem.Size = new System.Drawing.Size(240, 28);
            this.添加到目标组1ToolStripMenuItem.Text = "添加到目标组1";
            this.添加到目标组1ToolStripMenuItem.Click += new System.EventHandler(this.添加到目标组ToolStripMenuItem_Click);
            // 
            // 添加到目标组2ToolStripMenuItem
            // 
            this.添加到目标组2ToolStripMenuItem.Enabled = false;
            this.添加到目标组2ToolStripMenuItem.Name = "添加到目标组2ToolStripMenuItem";
            this.添加到目标组2ToolStripMenuItem.Size = new System.Drawing.Size(240, 28);
            this.添加到目标组2ToolStripMenuItem.Text = "添加到目标组2";
            this.添加到目标组2ToolStripMenuItem.Click += new System.EventHandler(this.添加到目标组ToolStripMenuItem_Click);
            // 
            // PoisonerTab
            // 
            this.PoisonerTab.Controls.Add(this.Target2ListContainer);
            this.PoisonerTab.Controls.Add(this.Target1ListContainer);
            this.PoisonerTab.Location = new System.Drawing.Point(4, 28);
            this.PoisonerTab.Margin = new System.Windows.Forms.Padding(4);
            this.PoisonerTab.Name = "PoisonerTab";
            this.PoisonerTab.Padding = new System.Windows.Forms.Padding(4);
            this.PoisonerTab.Size = new System.Drawing.Size(868, 474);
            this.PoisonerTab.TabIndex = 1;
            this.PoisonerTab.Text = "毒化目标";
            this.PoisonerTab.UseVisualStyleBackColor = true;
            // 
            // Target2ListContainer
            // 
            this.Target2ListContainer.Controls.Add(this.Target2List);
            this.Target2ListContainer.Location = new System.Drawing.Point(435, 9);
            this.Target2ListContainer.Margin = new System.Windows.Forms.Padding(4);
            this.Target2ListContainer.Name = "Target2ListContainer";
            this.Target2ListContainer.Padding = new System.Windows.Forms.Padding(4);
            this.Target2ListContainer.Size = new System.Drawing.Size(420, 448);
            this.Target2ListContainer.TabIndex = 3;
            this.Target2ListContainer.TabStop = false;
            this.Target2ListContainer.Text = "目标组2";
            // 
            // Target1ListContainer
            // 
            this.Target1ListContainer.Controls.Add(this.Target1List);
            this.Target1ListContainer.Location = new System.Drawing.Point(9, 9);
            this.Target1ListContainer.Margin = new System.Windows.Forms.Padding(4);
            this.Target1ListContainer.Name = "Target1ListContainer";
            this.Target1ListContainer.Padding = new System.Windows.Forms.Padding(4);
            this.Target1ListContainer.Size = new System.Drawing.Size(420, 448);
            this.Target1ListContainer.TabIndex = 2;
            this.Target1ListContainer.TabStop = false;
            this.Target1ListContainer.Text = "目标组1";
            // 
            // ScannerTab
            // 
            this.ScannerTab.Controls.Add(this.HostList);
            this.ScannerTab.Location = new System.Drawing.Point(4, 28);
            this.ScannerTab.Margin = new System.Windows.Forms.Padding(4);
            this.ScannerTab.Name = "ScannerTab";
            this.ScannerTab.Padding = new System.Windows.Forms.Padding(4);
            this.ScannerTab.Size = new System.Drawing.Size(868, 474);
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
            dataGridViewCellStyle37.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle37.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle37.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle37.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle37.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle37.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle37.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.HostList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle37;
            this.HostList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.HostList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.HostIP,
            this.HostMAC});
            dataGridViewCellStyle38.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle38.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle38.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle38.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle38.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle38.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle38.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.HostList.DefaultCellStyle = dataGridViewCellStyle38;
            this.HostList.Location = new System.Drawing.Point(9, 9);
            this.HostList.Margin = new System.Windows.Forms.Padding(4);
            this.HostList.Name = "HostList";
            this.HostList.ReadOnly = true;
            dataGridViewCellStyle39.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle39.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle39.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle39.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle39.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle39.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle39.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.HostList.RowHeadersDefaultCellStyle = dataGridViewCellStyle39;
            this.HostList.RowHeadersVisible = false;
            this.HostList.RowTemplate.Height = 23;
            this.HostList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.HostList.Size = new System.Drawing.Size(846, 448);
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
            this.MainTabContainer.Location = new System.Drawing.Point(0, 40);
            this.MainTabContainer.Margin = new System.Windows.Forms.Padding(4);
            this.MainTabContainer.Name = "MainTabContainer";
            this.MainTabContainer.SelectedIndex = 0;
            this.MainTabContainer.Size = new System.Drawing.Size(876, 506);
            this.MainTabContainer.TabIndex = 3;
            // 
            // WatcherTab
            // 
            this.WatcherTab.Controls.Add(this.ConnectionList);
            this.WatcherTab.Location = new System.Drawing.Point(4, 28);
            this.WatcherTab.Margin = new System.Windows.Forms.Padding(4);
            this.WatcherTab.Name = "WatcherTab";
            this.WatcherTab.Padding = new System.Windows.Forms.Padding(4);
            this.WatcherTab.Size = new System.Drawing.Size(868, 474);
            this.WatcherTab.TabIndex = 2;
            this.WatcherTab.Text = "连接列表";
            this.WatcherTab.UseVisualStyleBackColor = true;
            // 
            // ConnectionList
            // 
            this.ConnectionList.AllowUserToAddRows = false;
            this.ConnectionList.AllowUserToDeleteRows = false;
            this.ConnectionList.AllowUserToResizeRows = false;
            this.ConnectionList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ConnectionList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.ConnectionList.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle40.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle40.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle40.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle40.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle40.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle40.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle40.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ConnectionList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle40;
            this.ConnectionList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConnectionList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SrcAddress,
            this.DstAddress});
            dataGridViewCellStyle41.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle41.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle41.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle41.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle41.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle41.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle41.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ConnectionList.DefaultCellStyle = dataGridViewCellStyle41;
            this.ConnectionList.Location = new System.Drawing.Point(9, 9);
            this.ConnectionList.Margin = new System.Windows.Forms.Padding(4);
            this.ConnectionList.Name = "ConnectionList";
            this.ConnectionList.ReadOnly = true;
            dataGridViewCellStyle42.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle42.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle42.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle42.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle42.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle42.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle42.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ConnectionList.RowHeadersDefaultCellStyle = dataGridViewCellStyle42;
            this.ConnectionList.RowHeadersVisible = false;
            this.ConnectionList.RowTemplate.Height = 23;
            this.ConnectionList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ConnectionList.Size = new System.Drawing.Size(846, 448);
            this.ConnectionList.TabIndex = 3;
            // 
            // SrcAddress
            // 
            this.SrcAddress.HeaderText = "源地址";
            this.SrcAddress.Name = "SrcAddress";
            this.SrcAddress.ReadOnly = true;
            // 
            // DstAddress
            // 
            this.DstAddress.HeaderText = "目标地址";
            this.DstAddress.Name = "DstAddress";
            this.DstAddress.ReadOnly = true;
            // 
            // TargetListMenuStrip
            // 
            this.TargetListMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.TargetListMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.毒化目标复制ToolStripMenuItem,
            this.从目标组移除ToolStripMenuItem});
            this.TargetListMenuStrip.Name = "TargetListMenuStrip";
            this.TargetListMenuStrip.Size = new System.Drawing.Size(189, 60);
            // 
            // 毒化目标复制ToolStripMenuItem
            // 
            this.毒化目标复制ToolStripMenuItem.Name = "毒化目标复制ToolStripMenuItem";
            this.毒化目标复制ToolStripMenuItem.Size = new System.Drawing.Size(188, 28);
            this.毒化目标复制ToolStripMenuItem.Text = "复制选中项";
            // 
            // 从目标组移除ToolStripMenuItem
            // 
            this.从目标组移除ToolStripMenuItem.Name = "从目标组移除ToolStripMenuItem";
            this.从目标组移除ToolStripMenuItem.Size = new System.Drawing.Size(188, 28);
            this.从目标组移除ToolStripMenuItem.Text = "从目标组移除";
            // 
            // ConnectionListMenuStrip
            // 
            this.ConnectionListMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ConnectionListMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.连接列表复制ToolStripMenuItem,
            this.断开此连接ToolStripMenuItem});
            this.ConnectionListMenuStrip.Name = "ConnectionListMenuStrip";
            this.ConnectionListMenuStrip.Size = new System.Drawing.Size(241, 93);
            // 
            // 连接列表复制ToolStripMenuItem
            // 
            this.连接列表复制ToolStripMenuItem.Name = "连接列表复制ToolStripMenuItem";
            this.连接列表复制ToolStripMenuItem.Size = new System.Drawing.Size(170, 28);
            this.连接列表复制ToolStripMenuItem.Text = "复制选中项";
            this.连接列表复制ToolStripMenuItem.Click += new System.EventHandler(this.DataGridView复制ToolStripMenuItem_Click);
            // 
            // 断开此连接ToolStripMenuItem
            // 
            this.断开此连接ToolStripMenuItem.Name = "断开此连接ToolStripMenuItem";
            this.断开此连接ToolStripMenuItem.Size = new System.Drawing.Size(240, 28);
            this.断开此连接ToolStripMenuItem.Text = "断开此连接";
            // 
            // Target1List
            // 
            this.Target1List.AllowUserToAddRows = false;
            this.Target1List.AllowUserToDeleteRows = false;
            this.Target1List.AllowUserToResizeRows = false;
            this.Target1List.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Target1List.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.Target1List.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle43.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle43.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle43.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle43.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle43.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle43.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle43.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Target1List.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle43;
            this.Target1List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Target1List.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Target1IP,
            this.Target1MAC});
            dataGridViewCellStyle44.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle44.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle44.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle44.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle44.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle44.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle44.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Target1List.DefaultCellStyle = dataGridViewCellStyle44;
            this.Target1List.Location = new System.Drawing.Point(8, 29);
            this.Target1List.Margin = new System.Windows.Forms.Padding(4);
            this.Target1List.Name = "Target1List";
            this.Target1List.ReadOnly = true;
            dataGridViewCellStyle45.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle45.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle45.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle45.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle45.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle45.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle45.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Target1List.RowHeadersDefaultCellStyle = dataGridViewCellStyle45;
            this.Target1List.RowHeadersVisible = false;
            this.Target1List.RowTemplate.Height = 23;
            this.Target1List.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Target1List.Size = new System.Drawing.Size(404, 411);
            this.Target1List.TabIndex = 3;
            // 
            // Target2List
            // 
            this.Target2List.AllowUserToAddRows = false;
            this.Target2List.AllowUserToDeleteRows = false;
            this.Target2List.AllowUserToResizeRows = false;
            this.Target2List.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Target2List.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.Target2List.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle46.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle46.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle46.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle46.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle46.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle46.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle46.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Target2List.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle46;
            this.Target2List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Target2List.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Target2IP,
            this.Target2MAC});
            dataGridViewCellStyle47.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle47.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle47.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle47.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle47.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle47.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle47.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Target2List.DefaultCellStyle = dataGridViewCellStyle47;
            this.Target2List.Location = new System.Drawing.Point(8, 29);
            this.Target2List.Margin = new System.Windows.Forms.Padding(4);
            this.Target2List.Name = "Target2List";
            this.Target2List.ReadOnly = true;
            dataGridViewCellStyle48.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle48.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle48.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle48.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle48.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle48.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle48.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Target2List.RowHeadersDefaultCellStyle = dataGridViewCellStyle48;
            this.Target2List.RowHeadersVisible = false;
            this.Target2List.RowTemplate.Height = 23;
            this.Target2List.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Target2List.Size = new System.Drawing.Size(404, 411);
            this.Target2List.TabIndex = 4;
            // 
            // Target1IP
            // 
            this.Target1IP.HeaderText = "主机IP";
            this.Target1IP.Name = "Target1IP";
            this.Target1IP.ReadOnly = true;
            // 
            // Target1MAC
            // 
            this.Target1MAC.HeaderText = "主机MAC";
            this.Target1MAC.Name = "Target1MAC";
            this.Target1MAC.ReadOnly = true;
            // 
            // Target2IP
            // 
            this.Target2IP.HeaderText = "主机IP";
            this.Target2IP.Name = "Target2IP";
            this.Target2IP.ReadOnly = true;
            // 
            // Target2MAC
            // 
            this.Target2MAC.HeaderText = "主机MAC";
            this.Target2MAC.Name = "Target2MAC";
            this.Target2MAC.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 542);
            this.Controls.Add(this.MainTabContainer);
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
            this.HostListMenuStrip.ResumeLayout(false);
            this.PoisonerTab.ResumeLayout(false);
            this.Target2ListContainer.ResumeLayout(false);
            this.Target1ListContainer.ResumeLayout(false);
            this.ScannerTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HostList)).EndInit();
            this.MainTabContainer.ResumeLayout(false);
            this.WatcherTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionList)).EndInit();
            this.TargetListMenuStrip.ResumeLayout(false);
            this.ConnectionListMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Target1List)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Target2List)).EndInit();
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
        private System.Windows.Forms.ToolStripMenuItem 主机列表复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加到目标组1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加到目标组2ToolStripMenuItem;
        private System.Windows.Forms.TabPage PoisonerTab;
        private System.Windows.Forms.TabPage ScannerTab;
        private System.Windows.Forms.DataGridView HostList;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostMAC;
        private System.Windows.Forms.TabControl MainTabContainer;
        private System.Windows.Forms.TabPage WatcherTab;
        private System.Windows.Forms.GroupBox Target2ListContainer;
        private System.Windows.Forms.GroupBox Target1ListContainer;
        private System.Windows.Forms.DataGridView ConnectionList;
        private System.Windows.Forms.ContextMenuStrip TargetListMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 毒化目标复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 从目标组移除ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ConnectionListMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 连接列表复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 断开此连接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始毒化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始监视ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrcAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn DstAddress;
        private System.Windows.Forms.DataGridView Target2List;
        private System.Windows.Forms.DataGridView Target1List;
        private System.Windows.Forms.DataGridViewTextBoxColumn Target2IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Target2MAC;
        private System.Windows.Forms.DataGridViewTextBoxColumn Target1IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Target1MAC;
    }
}